using NetDemo.Network;
using NetDemo.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static NetDemo.Network.SimpleNetClient;

namespace NetDemo.Network
{
    internal class NetProxy : Singleton<NetProxy>
    {
        private DispatcherTimer _netTimer = null;
        private SimpleNetClient _netClient = null;
        private bool _isConnected = false;
        private bool _isConnecting = false;

        public NetClientEventHandler OnMessageEvent;

        protected override bool Init()
        {
            if (null == _netClient)
            {
                _netClient = new SimpleNetClient();
                _netClient.InitClient();

                // register event handlers
                _netClient.OnConnected += OnConnected_Recv;
                _netClient.OnDisconnected += OnDisconnected_Recv;
                _netClient.OnError += OnError_Recv;
                _netClient.OnMessage += OnMessage_Recv;
            }

            if (null == _netTimer)
            {
                _netTimer = new DispatcherTimer();
                _netTimer.Tick += NetworkTimer_Tick;
                // polling every 200ms
                _netTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                _netTimer.Start();
            }
            return base.Init();
        }

        public void ConnectToServer(string ip, int port)
        {
            if (null == _netClient)
                return;

            if (_isConnected)
            {
                //AddLogMessage("already connected");
                return;
            }
            if (_isConnecting)
            {
                //AddLogMessage("already trying to connecting");
                return;
            }

            _isConnecting = true;
            _netClient.ConnectToServer(ip, port);
        }

        public void SendMessage(string msg)
        {
            if (null == _netClient)
                return;

            _netClient.SendMessage(msg);
        }

        #region network event
        private void OnConnected_Recv(string msg)
        {
            //AddLogMessage("connection success");
            _isConnected = true;
            _isConnecting = false;
        }
        private void OnDisconnected_Recv(string msg)
        {
            //AddLogMessage("connection failed.");
            _isConnecting = false;
            _isConnected = false;
        }
        private void OnError_Recv(string msg)
        {
            var em = new ErrorMessage(msg);
            switch (em.ErrorType)
            {
                case 1:
                    {
                        //AddLogMessage("connection failed.");
                        _isConnecting = false;
                        _isConnected = false;
                    }
                    break;
                default:
                    //AddLogMessage("unknown error");
                    break;
            }

        }
        private void OnMessage_Recv(string msg)
        {
            //AddLogMessage(msg);
            
            byte[] bytes64 = Convert.FromBase64String(msg);
            string s1 = Encoding.UTF8.GetString(bytes64);
            this.OnMessageEvent?.Invoke(s1);
        }
        #endregion

        private void NetworkTimer_Tick(object sender, EventArgs e)
        {
            if (null != _netClient)
            {
                _netClient.UpdateClient();
            }
        }
    }
}
