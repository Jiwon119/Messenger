using CommonLib.Loggers;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CommonLib.TimerTask
{
    // TimerTask를 DIctionary로 관리하는 클래스
    public class TimerTaskManager
    {
        // Task Dictionary
        private ConcurrentDictionary<int, TimerTask> _taskDict = new ConcurrentDictionary<int, TimerTask>();

        private TimerTaskManager() { }
        private static TimerTaskManager _inst = null;
        public static TimerTaskManager Get
        {
            get 
            {
                if (null == _inst)
                    _inst = new TimerTaskManager();

                return _inst;
            }
        }

        public bool IsExistTimerTask(int id) => _taskDict.ContainsKey(id);
        public ICollection<int> Keys => _taskDict.Keys;

        // Task 생성. 초기화. 실행.
        public bool CreateTimerTask(ITaskBehaviour bhv, TimerConfig config)
        {
            if (null == bhv)
                return false;

            // TimerTask 생성(behavior, config 할당)
            TimerTask t = new TimerTask
            {
                _behaviour = bhv,
                _config = config
            };

            // ConcrrentDictionary로 관리
            int key = t.GetHashCode();
            if (!_taskDict.TryAdd(key, t))
                return false;

            return (t._config.IsInvokeOnStart) ? InitTimerTask(key) && StartTimerTask(key) : InitTimerTask(key);
        }

        public bool StartTimerTask(int key)
        {
            if (!IsExistTimerTask(key))
                return false;

            _taskDict.TryGetValue(key, out TimerTask t);

            if(t._isStarted == true)
                return false;

            NLogger.Get.Info($"Task ID: {key}");

            return t.StartTimerTask();
        }

        private bool InitTimerTask(int key)
        {
            if (!IsExistTimerTask(key))
                return false;

            _taskDict.TryGetValue(key, out TimerTask t);
            
            return t.InitTimerTask();
        }

        public bool StopTimerTask(int key)
        {
            if (!IsExistTimerTask(key))
                return false;

            _taskDict.TryGetValue(key, out TimerTask t);
            if (!t._config.IsLoopingTask)
                return RemoveTimerTask(key);

            NLogger.Get.Info($"Task ID: {key}");

            return t.StopTimerTask();
           }

        public bool RemoveTimerTask(int key)
        {
            if (!IsExistTimerTask(key))
                return false;

            _taskDict.TryRemove(key, out TimerTask t);

            t.Dispose();
            NLogger.Get.Info($"Task ID: {key}");

            return true;
        }
    }
}
