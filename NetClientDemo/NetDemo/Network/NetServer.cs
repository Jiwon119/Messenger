using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NetDemo.Loggers;
using NetDemo.Network.IOPacket;

namespace NetDemo.Network
{
    public abstract class NetServer
    {
        public static NetServer MakeSimpleServer(string ip, int port) 
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(ip), "ip must be not null or white space");
            Debug.Assert(0 < port && 65535 > port, "port number must be vaild");
            var s = new SimpleNetServer();            
            s.BindIP = ip;
            s.BindPort = port;
            s.CurSocketType = SocketType.Stream;
            s.CurProtoType = ProtocolType.Tcp;

            s._connPool = ConnectionPoolFactory.CreateConnectionPool(ConnectionType.TCP, s);

            return s;
        }

        public delegate void NetEventHandler(ConnectionID connId, string msg);

        public NetServer()
        {
            
        }

        private Socket _acceptor = null;
        private Thread _ioMainThread = null;
        private ManualResetEvent _ioWaiter = new ManualResetEvent(false);

        internal INetConnectionPool _connPool = null;

        public string BindIP { get; private set; }
        public int BindPort { get; private set; }
        public SocketType CurSocketType { get; protected set; }
        public ProtocolType CurProtoType { get; protected set; }

        public bool StartServer()
        {
            bool isValidIp = !string.IsNullOrWhiteSpace(BindIP);
            bool isValidPort = 0 < BindPort && 65535 > BindPort;
            Debug.Assert(isValidIp, "ip must be not null or white space");
            Debug.Assert(isValidPort, "port number must be vaild");
            if (!isValidIp || !isValidPort)
                return false;

            PacketPool.Init();
            RunAcceptor();

            return true;
        }

        public void TerminateServer()
        {
            _ioWaiter.Set();
            NLogger.Get.Log(NLog.LogLevel.Info, "ServerObject - WaitToJoinAcceptor called.");
            if (null != _ioMainThread)
            {
                NLogger.Get.Log(NLog.LogLevel.Info, "ServerObject - WaitToJoinAcceptor wait to join io thread");
                _ioMainThread.Join();
            }                
            else
            {
                NLogger.Get.Log(NLog.LogLevel.Info, "ServerObject - WaitToJoinAcceptor io thread is null");
            }             
        }

        public abstract void UpdateServer();
        protected virtual void OnErrorOccured(ErrorMessage em)
        {

        }
        
        #region internal logics
        protected void IOMain()
        {
            NLogger.Get.Log(NLog.LogLevel.Info, "IOMain() incomming");

            StartListen(); // acceptor bind, first accept

            NLogger.Get.Log(NLog.LogLevel.Info, "IOMain() running");

            _ioWaiter.WaitOne();

            NLogger.Get.Log(NLog.LogLevel.Info, "IOMain() cleanup start");

            CleanupAcceptor();
            CleanupConnections();

            NLogger.Get.Log(NLog.LogLevel.Info, "IOMain() cleanup finish");
        }
        protected void RunAcceptor()
        {
            NLogger.Get.Log(NLog.LogLevel.Info, "ServerObject - RunAcceptor called.");
            _ioMainThread = new Thread(IOMain);

            _ioMainThread.IsBackground = false;
            _ioMainThread.Start();
            NLogger.Get.Log(NLog.LogLevel.Info, "ServerObject - RunAcceptor io thread started");
        }

        protected void StartListen()
        {
            NLogger.Get.Log(NLog.LogLevel.Info, $"ServerObject - InitAcceptor. socket type : {CurSocketType}, protocol type : {CurProtoType}");

            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in ipHost.AddressList)
                {
                    NLogger.Get.Log(NLog.LogLevel.Info, $"host entry. {ip}");
                }

                NLogger.Get.Log(NLog.LogLevel.Info, $"prepare open socket. ip: {BindIP}, port {BindPort}");
                IPAddress ipAddr = IPAddress.Parse(BindIP);
                //IPAddress ipAddr = IPAddress.Any;

                NLogger.Get.Log(NLog.LogLevel.Info, $"Prepare init Acceptor. addr family: {ipAddr.AddressFamily}");
                _acceptor = new Socket(ipAddr.AddressFamily, CurSocketType, CurProtoType);
                NLogger.Get.Log(NLog.LogLevel.Info, $"socket created.");

                IPEndPoint endPoint = new IPEndPoint(ipAddr, BindPort);
                NLogger.Get.Log(NLog.LogLevel.Info, $"endpoint created. {endPoint}");

                _acceptor.Bind(endPoint);
                NLogger.Get.Log(NLog.LogLevel.Info, $"bind socket.");

                _acceptor.Listen(100);
                NLogger.Get.Log(NLog.LogLevel.Info, $"listen.");

                DoAcceptAsync();
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, $"InitAcceptor - exception occured. {e.Message}");
                OnErrorOccured(new ErrorMessage(1, "listen failed"));
                CleanupAcceptor();
            }
        }
        private void CleanupAcceptor()
        {
            try
            {
                if (null != _acceptor)
                {
                    _acceptor.Shutdown(SocketShutdown.Both);
                    _acceptor.Close();
                    _acceptor = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _acceptor = null;
                _ioWaiter.Set();
            }
        }
        private void DoAcceptAsync()
        {
            try
            {
                SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
                acceptArgs.Completed += AcceptArgs_Completed;
                if (!_acceptor.AcceptAsync(acceptArgs))
                {
                    if (acceptArgs.SocketError != SocketError.Success)
                    {
                        // completed
                        NLogger.Get.Log(NLog.LogLevel.Error, $"DoAcceptAsync - socket error: {acceptArgs.SocketError}");
                        OnErrorOccured(new ErrorMessage(1, "socket error"));
                    }
                    else
                    {
                        if (!ProcessAceeptComplete(acceptArgs.AcceptSocket))
                        {
                            NLogger.Get.Log(NLog.LogLevel.Error, "DoAcceptAsync - insert connection failed");
                            OnErrorOccured(new ErrorMessage(1, "connection failed"));
                        }
                    }

                    DoAcceptAsync();
                }
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                CleanupAcceptor();
            }
        }
        private void AcceptArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            do
            {
                if (SocketError.Success != e.SocketError)
                {
                    NLogger.Get.Log(NLog.LogLevel.Info, $"AcceptArgs_Completed - socket error : {e.SocketError}");
                    //CleanupAcceptor();
                    //return;
                    OnErrorOccured(new ErrorMessage(1, "socket error"));
                    break;
                }

                if (!ProcessAceeptComplete(e.AcceptSocket))
                {
                    NLogger.Get.Log(NLog.LogLevel.Error, "AcceptArgs_Completed - insert connection failed.");
                    OnErrorOccured(new ErrorMessage(1, "connection failed."));
                }

            } while (false);

            DoAcceptAsync();
        }
        private bool ProcessAceeptComplete(Socket sock)
        {
            if (null == sock)
            {
                NLogger.Get.Log(NLog.LogLevel.Info, "Accept Socket is null");
                OnErrorOccured(new ErrorMessage(1, "invalid socket"));
                return false;
            }

            NLogger.Get.Log(NLog.LogLevel.Info, "accept complete");

            ConnectionID curId = _connPool.AddNewConnection(sock);

            if (!_connPool.IsValidConnection(curId))
            {
                NLogger.Get.Log(NLog.LogLevel.Error, $"can't find connection id : {curId.Value}");
                OnErrorOccured(new ErrorMessage(1, "insert failed"));
                return false;
            }
            
            return true;
        }

        private void CleanupConnections()
        {
            if (null != _connPool)
            {
                _connPool.ClearAllConnections();
                _connPool = null;
            }
        }
        #endregion
    }

    public class SimpleNetServer : NetServer
        , IConnectionPoolEventReceiver
    {
        private static readonly string EmptyMessage = "";
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
            public ConnectionID Id;
            public string Packet;
        };

        private ConcurrentQueue<InChunk> _eventQueue = new ConcurrentQueue<InChunk>();

        // update server를 호출하는 thread에서 동작함.
        public event NetEventHandler OnOpen;
        public event NetEventHandler OnClose;
        public event NetEventHandler OnError;
        public event NetEventHandler OnMessage;

        public void SendMessage(ConnectionID id, string msg)
        {
            if (!id.IsValid)
            {
                return;
            }

            var conn = _connPool.GetConnection(id);
            if (null == conn)
                return;

            OutPacket oPacket = PacketPool.Get.GetOutPacket();
            oPacket.WritePacket(msg);
            conn.SendPacket(oPacket);
        }

        protected override void OnErrorOccured(ErrorMessage em)
        {
            _eventQueue.Enqueue(new InChunk()
            {
                Type = InChunkType.Error,
                Packet = em.ToJson()
            });
        }        

        public override void UpdateServer()
        {
            var count = _eventQueue.Count;
            for (var i = 0; i < count; ++i)
            {
                if (!_eventQueue.TryDequeue(out InChunk ic))
                    return;

                if (!ic.Id.IsValid)
                    continue;

                switch (ic.Type)
                {
                    case InChunkType.Open:
                    {
                        if (null == OnOpen)
                            NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnOpen");
                        else
                            OnOpen.Invoke(ic.Id, EmptyMessage);
                    }
                    break;
                    case InChunkType.Close:
                    {
                        if (null == OnClose)
                            NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnClose");
                        else
                            OnClose.Invoke(ic.Id, EmptyMessage);
                    }
                    break;
                    case InChunkType.Error:
                    {
                        if (null == OnError)
                            NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnError");
                        else
                            OnError.Invoke(ic.Id, ic.Packet);
                    }
                    break;
                    case InChunkType.Message:
                    {
                        if (null == OnMessage)
                            NLogger.Get.Log(NLog.LogLevel.Warn, "no listener - OnMessage");
                        else
                            OnMessage.Invoke(ic.Id, ic.Packet);
                    }
                    break;
                }
            }
        }

#region impl interfaces
        public void OnAddedNewConnection(ConnectionID id)
        {
            if (!id.IsValid)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, $"invalid connection. {id.Value}");
                return;
            }

            _eventQueue.Enqueue(new InChunk() {
                Id = id,
                Type = InChunkType.Open,
                Packet = EmptyMessage
            });
        }

        public void OnErrorConnection(ConnectionID id)
        {
            if (!id.IsValid)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, $"invalid connection. {id.Value}");
                return;
            }

            _eventQueue.Enqueue(new InChunk() {
                Id = id,
                Type = InChunkType.Error,
                Packet = EmptyMessage
            });
        }

        public bool OnLoopbackPacket(ConnectionID id, OutPacket oPacket)
        {
            // not used
            throw new NotImplementedException();
        }

        public void OnReceivePacket(ConnectionID id, InPacket iPacket)
        {
            if (!id.IsValid)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, $"invalid connection. {id.Value}");
                return;
            }

            string val = iPacket.ReadString();
            PacketPool.Get.PutInPacket(iPacket);
            
            _eventQueue.Enqueue(new InChunk() {
                Id = id,
                Type = InChunkType.Message,
                Packet = val
            });
        }

        public void OnRemoveConnection(ConnectionID id)
        {
            if (!id.IsValid)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, $"invalid connection. {id.Value}");
                return;
            }

            _eventQueue.Enqueue(new InChunk() {
                Id = id,
                Type = InChunkType.Close,
                Packet = EmptyMessage
            });
        }
#endregion
    
    }
}