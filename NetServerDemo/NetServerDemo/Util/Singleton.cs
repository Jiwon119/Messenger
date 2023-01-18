using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServerDemo.Util
{
    internal class Singleton<T> where T : Singleton<T>, new()
    {
        private static T? _instance = null;
        public static T Get
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new T();
                    _instance.Init();
                }

                return _instance;
            }
        }

        protected virtual bool Init()
        {
            return true;
        }
    }
}
