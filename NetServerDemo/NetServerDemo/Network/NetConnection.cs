using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using NetServerDemo.Loggers;
using NetServerDemo.Network.IOPacket;
using NetServerDemo.Network.NetObjects;

namespace NetServerDemo.Network
{
    public interface INetConnectionEventReceiver
    {
        void OnReceivePacket(ConnectionID id, InPacket iPacket);

        void OnStopConnection(ConnectionID id);
        void OnErrorConnection(ConnectionID id);
    }


    internal abstract class NetConnection : IDisposable
    {
        protected NetConnection()
        {
        }

        public static NetConnection CreateConnection<T>(Socket sock, INetConnectionEventReceiver recver, ConnectionID id) where T : NetConnection, new()
        {
            Debug.Assert(null != sock, "CreateConnection - socket must be not null");
            Debug.Assert(null != recver, "CreateConnection - receiver must be not null");
            Debug.Assert(false != id.IsValid, "CreateConnection - connection id must be valid");
            if (null == sock || null == recver || !id.IsValid)
                return null;

            NetConnection conn = new T();
            conn.ConnID = id;
            conn._connSocket = sock;
            conn._eventReceiver = recver;

            conn.InitConnection();

            return conn;
        }

        //socket
        protected INetConnectionEventReceiver _eventReceiver = null;
        protected Socket _connSocket = null;
        public ConnectionID ConnID { get; protected set; }
        private bool _connDisposed = false;

        protected virtual void InitConnection()
        {

        }

        public abstract void RunConnection();

        public virtual void StopConnection()
        {
            Dispose();
        }

        public abstract void SendPacket(OutPacket oPacket);

        public virtual void Dispose()
        {
            if (null != _eventReceiver)
            {
                _eventReceiver.OnStopConnection(ConnID);
                _eventReceiver = null;
            }

            if (null != _connSocket && !_connDisposed)
            {
                _connSocket.Shutdown(SocketShutdown.Both);
                _connSocket.Close();
                _connSocket.Dispose();
                _connSocket = null;
                _connDisposed = true;
            }
        }

        public bool IsConnected
        {
            get { return null != _connSocket && _connSocket.Connected; }
        }

        public bool IsValid
        {
            get { return null != _connSocket && ConnID.IsValid && null != _eventReceiver; }
        }
    }

    internal class TCPConnection : NetConnection
    {
        protected RecvPacketBuffer _receiveBuffer = new RecvPacketBuffer();
        protected SendPacketBuffer _sendBuffer = new SendPacketBuffer();
        protected long _isSending = 0; // 0 - wait, 1 - processing

        //ttl check
        private Timer _pingTimer = null;
        private bool _timerDisposed = false;

        public override void RunConnection()
        {
            if (!IsConnected)
            {
                NLogger.Get.Log(NLog.LogLevel.Info, "RunConnection - not connected.");
                return;
            }

            if (null == _pingTimer)
            {
                //_pingTimer = new Timer(DoPing, null, PingPeriod, PingPeriod);
            }

            DoReceiveAsync();
        }

        public override void SendPacket(OutPacket oPacket)
        {
            if (null == _sendBuffer)
                return;

            oPacket.SetPacketSize();

            _sendBuffer.PushPacket(oPacket);

            long IsWaiting = Interlocked.CompareExchange(ref _isSending, 1, 0);
            if (0 == IsWaiting)
            {
                // try sending
                _sendBuffer.FlushPackets();
                DoSendAsync();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (null != _pingTimer && !_timerDisposed)
            {
                _pingTimer.Dispose();
                _timerDisposed = true;
            }
        }

        #region send process
        protected void DoSendAsync()
        {
            do
            {
                if (!IsConnected)
                {
                    NLogger.Get.Log(NLog.LogLevel.Info, "DoSendAsync - not connected.");
                    break;
                }

                try
                {
                    if (_sendBuffer.IsSendComplete)
                        return;

                    SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                    sendArgs.Completed += SendArgs_Completed;
                    sendArgs.SetBuffer(_sendBuffer.SendBuffer.GetBuffer,
                        _sendBuffer.SendBuffer.CurPos,
                        _sendBuffer.SendBuffer.LeftDataSize);

                    if (!_connSocket.SendAsync(sendArgs))
                    {
                        if (sendArgs.SocketError != SocketError.Success)
                        {
                            NLogger.Get.Log(NLog.LogLevel.Info, $"SendPacket - receive failed. error code: {sendArgs.SocketError}");

                            StopConnection();
                            break;
                        }
                        else
                        {
                            ProcessSendComplete(sendArgs.BytesTransferred);
                        }
                    }
                }
                catch (Exception e)
                {
                    NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                    StopConnection();
                    break;
                }

            } while (false);

        }

        private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                NLogger.Get.Log(NLog.LogLevel.Info, $"SendArgs_Completed - Send error : {e.SocketError}");
                // cleanup
                StopConnection();
                return;
            }

            int sendSize = e.BytesTransferred;
            ProcessSendComplete(sendSize);
        }

        private void ProcessSendComplete(int transferByte)
        {
            _sendBuffer.PopFrontBuffer(transferByte);

            if (_sendBuffer.IsSendComplete) // all transfer
            {
                _sendBuffer.ClearBuffer();
            }
            else
            {
                DoSendAsync();
                return;
            }

            if (_sendBuffer.IsQueueEmpty)
            {
                // marking send process finished
                long isWaiting = Interlocked.CompareExchange(ref _isSending, 0, 1);
            }
            else
            {
                // do next job
                _sendBuffer.FlushPackets();
                DoSendAsync();
            }
        }
        #endregion

        #region receive process
        protected void DoReceiveAsync()
        {
            if (!IsConnected)
            {
                NLogger.Get.Log(NLog.LogLevel.Info, "DoReceiveAsync - not connected.");
                return;
            }

            try
            {
                SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
                recvArgs.Completed += RecvArgs_Completed;
                //recvArgs.SocketFlags = SocketFlags.Partial;
                // note:
                // partial flag는 모든 운영체제, 소켓에서 지원하는건 아니다..
                // windows에서만 지원해주는듯.. 
                // 나중에 websocket도 생각해보자
                recvArgs.SocketFlags = SocketFlags.None;
                recvArgs.SetBuffer(_receiveBuffer.GetBuffer,
                    _receiveBuffer.LeftDataSize,
                    _receiveBuffer.LeftBufferSize);

                if (!_connSocket.ReceiveAsync(recvArgs))
                {
                    if (recvArgs.SocketError != SocketError.Success)
                    {
                        NLogger.Get.Log(NLog.LogLevel.Info, $"DoReceiveAsync - receive failed. error code: {recvArgs.SocketError}");

                        //cleanup
                        StopConnection();
                        return;
                    }
                    else
                    {
                        ProcessRecvComplete(recvArgs.BytesTransferred);
                    }
                }
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                //cleanup
                StopConnection();
                return;
            }
        }

        private void RecvArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                NLogger.Get.Log(NLog.LogLevel.Info, $"RecvArgs_Completed - receive failed. error code: {e.SocketError}");
                // cleanup
                StopConnection();
                return;
            }

            ProcessRecvComplete(e.BytesTransferred);
        }

        private void ProcessRecvComplete(int recvByte)
        {
            if (0 >= recvByte)
            {
                //close connection
                NLogger.Get.Log(NLog.LogLevel.Info, "RecvArgs_Completed - client disconnected.");
                StopConnection();
                return;
            }
            else
            {
                _receiveBuffer.AddBufferLength(recvByte);

                if (_receiveBuffer.LeftBufferSize <= 0)
                    _receiveBuffer.BufferExpend();

                bool parseSuccess = false;
                while (true)
                {
                    InPacket iPacket = PacketPool.Get.GetInPacket();
                    if (null == iPacket)
                    {
                        // something wrong
                        NLogger.Get.Log(NLog.LogLevel.Info, "RecvArgs_Completed - get inpacket failed.");
                        StopConnection();
                        return;
                    }

                    parseSuccess = iPacket.ParsePacket(_receiveBuffer);
                    if (!parseSuccess)
                    {
                        PacketPool.Get.PutInPacket(iPacket);
                        break;
                    }

                    _eventReceiver.OnReceivePacket(ConnID, iPacket);
                    _receiveBuffer.PopParseData();

                    if (0 >= _receiveBuffer.LeftDataSize)
                        break;
                } // while end

                //if (parseSuccess)
                    //_receiveBuffer.PopParseData();
            }// else end

            DoReceiveAsync();
        }
        #endregion

    }
}