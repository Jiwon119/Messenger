using CommonLib.Loggers;
using CommonLib.TimerTask;
using System;
using System.Threading;

namespace CommonLib.TimerTask
{
    public struct TimerConfig
    {
        public int TimerInterval { get; set; }
        public bool IsExpired { get; set; }
        public bool IsIncludeWaitTime { get; set; }
        public bool IsDisposeOnFailed { get; set; }

        public bool IsLoopingTask { get; set; }
        public bool IsInvokeOnStart { get; set; }

        public int RetryCount { get; set; }
    }


    // Timer task는 일정 주기마다 Invoke
    public class TimerTask : IDisposable
    {
        // Constructor
        internal TimerTask() { }

        internal ITaskBehaviour _behaviour;
        internal TimerConfig _config;
        internal bool _isStarted = false;

        private Timer _taskTimer = null;
        private DateTime _startTime = DateTime.Now;

        // 작업 초기화
        public bool InitTimerTask()
        {
            if (null == _behaviour)
                return false;

            if (null != _taskTimer) // 이미 초기화되어있으면 return false
                return false;

            _taskTimer = new Timer(OnTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
            _behaviour.OnInitTimerTask();

            return true;
        }

        // 작업 시작
        public bool StartTimerTask()
        {
            if (_taskTimer == null)
                return false;

            if (_config.IsInvokeOnStart)
                _taskTimer.Change(0, Timeout.Infinite);
            else
                _taskTimer.Change(_config.TimerInterval, Timeout.Infinite);

            _isStarted = true;

            return true;
        }

        // 작업 정지
        public bool StopTimerTask()
        {
            if (null == _taskTimer)
                return false;

            if (_isStarted == false)
                return false;

            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _isStarted = false;
            return true;
        }

        // 주기마다 무엇을 할지에 대한 스크립팅
        private async void OnTimerCallback(object state)
        {
            bool success = await _behaviour.DoTimerTask();

            if (_behaviour.IsTimerTaskComplete == true)
                Dispose();

            if (!success)
            {
                if (_config.IsDisposeOnFailed)
                {
                    StopTimerTask();
                    return;
                }
            }

            if (!_config.IsLoopingTask)
            {
                Dispose();
                return;
            }

            int interval = _config.TimerInterval;
            if (_config.IsIncludeWaitTime)  // 이전 태스크가 끝나기 전까지 새로운 태스크를 시작하지 않고 싶다면
            {
                TimeSpan diff = DateTime.Now - _startTime;
                _startTime = DateTime.Now;
                interval = Math.Max(0, (int)diff.TotalMilliseconds - interval);
            }

            if (_taskTimer != null)
                _taskTimer.Change(interval, Timeout.Infinite);
        }

        // Task 값
        public override int GetHashCode()
        {
            if (null == _behaviour)
                return base.GetHashCode();

            return _behaviour.TimerTaskHash;
        }

        // Dispose
        public void Dispose()
        {
            StopTimerTask();
            
            if (null != _behaviour)
            {
                _behaviour.OnDisposed();
                _behaviour = null;
            }

            if (null != _taskTimer)
            {
                _taskTimer.Dispose();
                _taskTimer = null;
            }
        }
    }

}
