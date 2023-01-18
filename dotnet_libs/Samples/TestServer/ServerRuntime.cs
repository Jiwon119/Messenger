using System;
using CommonLib.Loggers;
using CommonLib.Network;
using CommonLib.Runtime;

namespace TestServer
{
    class ServerRuntime : RuntimeFramework
    {
        private SimpleNetServer? _server = null;

        protected override bool OnInitialized()
        {
            if (null != _server)
                return false;

            _server = NetServer.MakeSimpleServer("127.0.0.1", 6767) as SimpleNetServer;
            if (null == _server)
                return false;
            
            _server.OnOpen += (id, msg) => {
                NLogger.Get.Log(NLog.LogLevel.Info, msg);
            };
            _server.OnClose += (id, msg) => {
                NLogger.Get.Log(NLog.LogLevel.Info, msg);
            };
            _server.OnMessage += (id, msg) => {
                NLogger.Get.Log(NLog.LogLevel.Info, msg);

                _server.SendMessage(id, "hi client");
            };

            _server.StartServer();
            return true;
        }

        protected override void OnTerminated()
        {
            if (null != _server)
            {
                _server.TerminateServer();
                _server = null;
            }
        }

        protected override void OnTickEvent(long elapsedTime)
        {
            if (null != _server)
            {
                _server.UpdateServer();
            }
        }
    }
}