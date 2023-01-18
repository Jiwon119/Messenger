using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommonLib.Shared
{
    public class AtomicLock
    {
        private long _lock = 0;

        public bool IsLock
        {
            get
            {
                var l = Interlocked.Read(ref _lock);
                return (0 != l);
            }
        }
        public bool Lock()
        {
            var old = Interlocked.CompareExchange(ref _lock, 0, 1);
            if (old != 0)
                return false;

            return true;
        }
        public void Unlock()
        {
            Interlocked.Exchange(ref _lock, 0);
        }
    }
}
