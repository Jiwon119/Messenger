using System;
using CommonLib.Loggers;
using CommonLib.Network;
using CommonLib.Runtime;

namespace TestClient
{
    class ClientRuntime : RuntimeFramework
    {
        private SimpleNetClient? _client = null;

        protected override bool OnInitialized()
        {
            if (null != _client)
                return false;

            _client = new SimpleNetClient();
            if (null == _client)
                return false;

            _client.InitClient();
            
            _client.OnConnected += (msg) => {
                NLogger.Get.Log(NLog.LogLevel.Info, msg);
                _client.SendMessage("hello server");
            };
            _client.OnDisconnected += (msg) => {
                NLogger.Get.Log(NLog.LogLevel.Info, msg);
            };
            _client.OnMessage += (msg) => {
                NLogger.Get.Log(NLog.LogLevel.Info, msg);
            };

            _client.ConnectToServer("127.0.0.1", 6767);
            return true;
        }

        protected override void OnTerminated()
        {
            if (null != _client)
            {
                _client.TerminateClient();
                _client = null;
            }
        }

        protected override void OnTickEvent(long elapsedTime)
        {
            if (null != _client)
            {
                _client.UpdateClient();
            }
        }
    }
}