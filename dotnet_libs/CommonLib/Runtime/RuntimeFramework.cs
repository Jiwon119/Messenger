using CommonLib.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CommonLib.Runtime
{
    public abstract class RuntimeFramework
    {
        private bool _isRunning = false;
        private Stopwatch _loopWatch = null;
        private long _invokeTime = 1000;

        private bool _isInitComplete = false;

        public long InvokeInterval { get => _invokeTime; set => _invokeTime = value; }

        public virtual void InitRuntime()
        {
            _loopWatch = new Stopwatch();
            _isRunning = true;

            // Log로 NLogLoggingCore()을 사용하겠다.
            //NLogger.Get.SetLoggingCore(new NLogger());

            SetEnvironmentConfig();

            if (!OnInitialized())
                return;

            _isInitComplete = true;
        }

        public void Run()
        {
            if (!_isInitComplete)
                return;

            Console.CancelKeyPress += (sender, evt) =>
            {
                evt.Cancel = true;
                _isRunning = false;
            };
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                _isRunning = false;
            };

            while (_isRunning)
            {
                _loopWatch.Stop();
                long etime = _loopWatch.ElapsedMilliseconds;
                ProcessTickEvent(etime);
                _loopWatch.Reset();

                //RedisProxy.Get.PollingAllMessageQueues(ProcessReceivedMessage);

                _loopWatch.Start();
                Thread.Sleep(100);
            }

        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Terminate()
        {
            if (null != _loopWatch)
            {
                _loopWatch.Stop();
                _loopWatch = null;
            }

            OnTerminated();

            // NLogger.Get.Terminate();
        }

        private void ProcessTickEvent(long time)
        {
            OnTickEvent(time);
        }

        protected abstract void OnTickEvent(long elapsedTime);
        protected abstract bool OnInitialized();
        protected abstract void OnTerminated();
        protected virtual void SetEnvironmentConfig()
        {
            //EnvConfig.SetOrDefaultEnvValue("TARGET_REDIS_IP", "172.44.0.100");
            //EnvConfig.SetOrDefaultEnvValue("TARGET_REDIS_PORT", "8001");
        }
    }
}
