using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Loggers;
using CommonLib.Network.IOPacket;

namespace CommonLib.Network
{
    public class SimpleNetClient
        : INetConnectionEventReceiver
    {
        // packet pool 없이 단일 connection만 관리한다.
        // 복수 서버 연결시 netclient를 복수개 만들어서 사용하자.
        private Socket _connector = null;
        private NetConnection _netConn = null;

        public static readonly ConnectionID ConnID = new ConnectionID(1);
        private static readonly string EmptyMessage = "";

        protected bool _isConnecting = false;
        protected bool _isConnected = false;

        private enum InChunkType
        {
            Open,
            Close,
            Error,
            Message
        }
        private struct InChunk
        {
            public InChunkType Type;
            public string Packet;
        };

        private ConcurrentQueue<InChunk> _eventQueue = new ConcurrentQueue<InChunk>();

        public delegate void NetClientEventHandler(string msg);
        // main thread에서 호출한다.
        public event NetClientEventHandler OnConnected;
        public event NetClientEventHandler OnDisconnected;
        public event NetClientEventHandler OnError;
        public event NetClientEventHandler OnMessage;

        public bool InitClient()
        {
            PacketPool.Init();
            _isConnected = false;
            _isConnecting = false;
            _netConn = null;

            return true;
        }

        public void TerminateClient()
        {
            if (null != _netConn)
            {
                _netConn.StopConnection();
                _netConn = null;
            }
        }

        public void ConnectToServer(string ip, int port)
        {
            DoConnectAsync(ip, port);
        }

        public void SendMessage(string msg)
        {
            if (null == _netConn)
                return;
            
            OutPacket oPacket = PacketPool.Get.GetOutPacket();
            oPacket.WritePacket(msg);
            _netConn.SendPacket(oPacket);
        }

        public void UpdateClient()
        {
            var count = _eventQueue.Count;
            for (var i = 0; i < count; ++i)
            {
                if (!_eventQueue.TryDequeue(out InChunk ic))
                    return;

                switch (ic.Type)
                {
                    case InChunkType.Open:
                        {
                            if (null == OnConnected)
                                NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnConnected");
                            else
                                OnConnected.Invoke(EmptyMessage);
                        }
                        break;
                    case InChunkType.Close:
                        {
                            if (null == OnDisconnected)
                                NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnDisconnected");
                            else
                                OnDisconnected.Invoke(EmptyMessage);
                        }
                        break;
                    case InChunkType.Error:
                        {
                            if (null == OnError)
                                NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnError");
                            else
                                OnError.Invoke(EmptyMessage);
                        }
                        break;
                    case InChunkType.Message:
                        {
                            if (null == OnMessage)
                                NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnMessage");
                            else
                                OnMessage.Invoke(ic.Packet);
                        }
                        break;
                }
            }
        }

        public async Task<bool> ConnectToServerAsync_NotUsed(string ip, int port)
        {
            if (_isConnecting
                || _isConnected
                || null != _netConn)
                return false;

            try
            {
                CancellationTokenSource source = new CancellationTokenSource(60 * 1000);
                CancellationToken token = source.Token;
                var connSock = await Task.Run<Socket>(() =>
                {
                    Socket sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    sock.Connect(ip, port);
                    if (!sock.Connected)
                        return null;

                    return sock;
                }, token);

                if (null == connSock)
                {
                    return false;
                }

                NetConnection conn = NetConnection.CreateConnection<TCPConnection>(
                    connSock, this, new ConnectionID(1)
                );

                if (null == conn)
                {
                    return false;
                }

                _netConn = conn;
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                return false;
            }

            return true;
        }

        private void DoConnectAsync(string ip, int port)
        {
            NLogger.Get.Log(NLog.LogLevel.Info, $"Dequeue connect request - ip: {ip}, port: {port}");

            try
            {
                var addr = IPAddress.Parse(ip);
                SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
                connectArgs.Completed += ConnectArgs_Completed;
                connectArgs.RemoteEndPoint = new IPEndPoint(addr, port);
                connectArgs.UserToken = ConnID;

                _connector = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (_connector.ConnectAsync(connectArgs))
                {
                    if (SocketError.Success != connectArgs.SocketError)
                    {
                        NLogger.Get.Log(NLog.LogLevel.Error, $"DoConnectAsync - Socket Error : {connectArgs.SocketError}");
                        //_eventReceiver?.OnConnectComplete(false, reqEp.ConnID);

                        _eventQueue.Enqueue(new InChunk()
                        {
                            Type = InChunkType.Error,
                            Packet = new ErrorMessage(1, "socket error").ToJson()
                        });
                    }
                    else
                    {
                        Socket connSock = connectArgs.ConnectSocket;
                        ConnectionID connEp = (ConnectionID)connectArgs.UserToken;
                        if (null == connSock)
                        {
                            // not yet
                            return;
                        }
                        if (!ProcessConnectComplete(connSock, connEp))
                        {
                            NLogger.Get.Log(NLog.LogLevel.Error, "DoconnectAsync - insert connection failed.");
                            _eventQueue.Enqueue(new InChunk()
                            {
                                Type = InChunkType.Error,
                                Packet = new ErrorMessage(1, "connection failed").ToJson()
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                return;
            }

        }

        private void ConnectArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            do
            {
                if (SocketError.Success != e.SocketError)
                {
                    NLogger.Get.Log(NLog.LogLevel.Error, $"ConnectArgs_Completed - Socket Error : {e.SocketError}");
                    _eventQueue.Enqueue(new InChunk()
                    {
                        Type = InChunkType.Error,
                        Packet = new ErrorMessage(1, $"Socket Error: {e.SocketError}").ToJson()                        
                    });
                    break;
                }

                Socket connSock = e.ConnectSocket;
                ConnectionID connEp = (ConnectionID)e.UserToken;

                if (!ProcessConnectComplete(connSock, connEp))
                {
                    NLogger.Get.Log(NLog.LogLevel.Error, "ConnectArgs_Completed - insert connection failed.");
                    _eventQueue.Enqueue(new InChunk()
                    {
                        Type = InChunkType.Error,
                        Packet = new ErrorMessage(1, "connection failed").ToJson()
                    });
                    return;
                }

            } while (false);
        }

        private bool ProcessConnectComplete(Socket sock, ConnectionID connVal)
        {
            if (null == sock)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, "ConnectArgs_Completed - socket is null");
                _eventQueue.Enqueue(new InChunk()
                {
                    Type = InChunkType.Error,
                    Packet = new ErrorMessage(1, "connection failed").ToJson()
                });
                return false;
            }

            NLogger.Get.Log(NLog.LogLevel.Info, "Connect Complete");
            _netConn = NetConnection.CreateConnection<TCPConnection>(sock, this, connVal);
            if (null == _netConn)
            {
                _eventQueue.Enqueue(new InChunk()
                {
                    Type = InChunkType.Error,
                    Packet = new ErrorMessage(1, "connection failed").ToJson()
                });
                return false;
            }

            _netConn.RunConnection();
            _eventQueue.Enqueue(new InChunk()
            {
                Type = InChunkType.Open,
                Packet = EmptyMessage
            });

            return true;
        }

        public void DisconnectToServer()
        {
            if (null == _netConn)
                return;

            _netConn.StopConnection();
            _netConn = null;
        }

        public void OnErrorConnection(ConnectionID id)
        {
            _eventQueue.Enqueue(new InChunk()
            {
                Type = InChunkType.Error,
                Packet = EmptyMessage
            });
        }

        public void OnReceivePacket(ConnectionID id, InPacket iPacket)
        {
            var val = iPacket.ReadString();
            _eventQueue.Enqueue(new InChunk()
            {
                Type = InChunkType.Message,
                Packet = val
            });
        }

        public void OnStopConnection(ConnectionID id)
        {
            _eventQueue.Enqueue(new InChunk()
            {
                Type = InChunkType.Close,
                Packet = EmptyMessage
            });
        }
    }

}