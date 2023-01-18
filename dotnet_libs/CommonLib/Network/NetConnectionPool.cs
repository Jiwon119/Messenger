using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using CommonLib.Network.IOPacket;

namespace CommonLib.Network
{
    public enum ConnectionType
    {
        TCP,
        UDP,
        MAX
    }
    internal interface INetConnectionPool
    {
        ConnectionID AddNewConnection(Socket sock);
        void RemoveConnection(ConnectionID id);
        bool IsValidConnection(ConnectionID id);
        NetConnection GetConnection(ConnectionID id);
        void ClearAllConnections();
        IConnectionPoolEventReceiver ConnectionPoolEventReceiver { get; }
    }

    internal interface IConnectionPoolEventReceiver
    {
        void OnReceivePacket(ConnectionID id, InPacket iPacket);
        bool OnLoopbackPacket(ConnectionID id, OutPacket oPacket);
        //void OnStop

        void OnAddedNewConnection(ConnectionID id);
        void OnRemoveConnection(ConnectionID id);
        void OnErrorConnection(ConnectionID id);
    }

    internal class ConnectionPoolFactory
    {        
        public static INetConnectionPool CreateConnectionPool(ConnectionType type, IConnectionPoolEventReceiver receiver)
        {
            INetConnectionPool pool = new NetConnectionPool(type, receiver);
            return pool;
        }
    }

    internal class NetConnectionPool
        : INetConnectionPool
        , INetConnectionEventReceiver
        , IDisposable
        //where T : NetConnection
    {
        public NetConnectionPool(ConnectionType type, IConnectionPoolEventReceiver recver)
        {
            Debug.Assert(type != ConnectionType.MAX, "NetConnectionPool - conn type must be not MAX");
            Debug.Assert(null != recver, "NetConnectionPool - event receiver must be not null");

            _connType = type;
            _eventReceiver = recver;
        }

        protected ConnectionType _connType = ConnectionType.MAX;
        protected IConnectionPoolEventReceiver _eventReceiver = null;
        protected ConcurrentDictionary<ConnectionID, NetConnection> _connectionDict = new ConcurrentDictionary<ConnectionID, NetConnection>();

        protected long _lastConnectionId = 0;

        public IConnectionPoolEventReceiver ConnectionPoolEventReceiver => _eventReceiver;

        public void Dispose()
        {
            ClearAllConnections();
        }

        public ConnectionID GetNewConnectionID()
        {
            long incVal = Interlocked.Increment(ref _lastConnectionId);
            return new ConnectionID(incVal);
        }
        public ConnectionID MakeValidConnectionID(long id)
        {
            ConnectionID connId = new ConnectionID(id);
            if (_connectionDict.ContainsKey(connId))
                return ConnectionID.Nil;

            return connId;
        }

        public ConnectionID AddNewConnection(Socket sock)
        {
            if (null == sock)
                return ConnectionID.Nil;

            ConnectionID newId = ConnectionID.Nil;
            while (true)
            {
                newId = GetNewConnectionID();
                if (!newId.IsValid)
                    continue;

                if (!_connectionDict.ContainsKey(newId))
                    break;
            }

            return AddNewConnection(sock, newId);
        }
        public ConnectionID AddNewConnection(Socket sock, ConnectionID id)
        {
            if (null == sock || !id.IsValid)
                return ConnectionID.Nil;

            NetConnection conn = null;
            switch (_connType)
            {
                case ConnectionType.TCP:
                    conn = NetConnection.CreateConnection<TCPConnection>(sock, this, id);
                    break;
                default:
                    break;
            }
                
            if (null == conn)
                return ConnectionID.Nil;

            if (!_connectionDict.TryAdd(conn.ConnID, conn))
                return ConnectionID.Nil;

            conn.RunConnection();
            _eventReceiver?.OnAddedNewConnection(conn.ConnID);
            return conn.ConnID;
        }
        public void RemoveConnection(ConnectionID id)
        {
            if (!id.IsValid)
                return;

            NetConnection conn = null;
            if (_connectionDict.TryRemove(id, out conn))
            {
                if (null != conn)
                {
                    _eventReceiver?.OnRemoveConnection(conn.ConnID);
                    conn.StopConnection();
                }   
            }
        }
        public bool IsValidConnection(ConnectionID id)
        {
            if (!id.IsValid)
                return false;

            NetConnection conn = null;
            if (!_connectionDict.TryGetValue(id, out conn))
            {
                return false;
            }

            if (conn.ConnID != id ||
                !conn.IsValid ||
                !conn.IsConnected)
                return false;

            return true;
        }

        public NetConnection GetConnection(ConnectionID id)
        {
            if (!id.IsValid)
                return null;

            NetConnection conn = null;
            if (!_connectionDict.TryGetValue(id, out conn))
                return null;

            return conn;
        }

        public void OnReceivePacket(ConnectionID id, InPacket iPacket)
        {
            if (null == _eventReceiver)
            {
                Debug.Assert(false, "OnReceivePacket - IConnectionPoolEventReceiver must be not null");
                return;
            }

            _eventReceiver?.OnReceivePacket(id, iPacket);
        }

        public void OnStopConnection(ConnectionID id)
        {
            RemoveConnection(id);
        }

        public void ClearAllConnections()
        {
            ForeachConnections((conn) => {
                if (null == conn) return;
                if (!conn.IsConnected || !conn.IsValid) return;

                _eventReceiver?.OnRemoveConnection(conn.ConnID);
                conn.StopConnection();
            });

            _connectionDict.Clear();
        }

        protected void ForeachConnections(Action<NetConnection> func)
        {
            if (null == func)
                return;

            foreach (NetConnection conn in _connectionDict.Values)
            {
                func(conn);
            }
        }

        public void OnErrorConnection(ConnectionID id)
        {
            _eventReceiver?.OnErrorConnection(id);
        }
    } // connection pool end
}