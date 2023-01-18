using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.TimerTask
{

    public class TimerTaskScheduler : TaskScheduler
    {
        public TimerTaskScheduler(int maxCount)
        {
            _taskQueue = new ConcurrentQueue<Task>();
            _maxThreadCount = maxCount;
        }

        private ConcurrentQueue<Task> _taskQueue = null;

        private readonly long _maxThreadCount;
        private long _curRunningThreadCount = 0;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _taskQueue.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            if (null == task)
                return;

            _taskQueue.Enqueue(task);

            long count = Interlocked.Read(ref _curRunningThreadCount);
            if (count < _maxThreadCount)
            {
                Interlocked.Increment(ref _curRunningThreadCount);
                InvokeTaskProcessing();
            }
        }

        private void InvokeTaskProcessing()
        {
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                try
                {
                    Task task = null;
                    while (_taskQueue.TryDequeue(out task))
                    {
                        base.TryExecuteTask(task);
                    }
                }
                finally
                {
                    Interlocked.Decrement(ref _curRunningThreadCount);
                }
            },
            null);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected sealed override bool TryDequeue(Task task)
        {
            return true;
        }
    }

}
