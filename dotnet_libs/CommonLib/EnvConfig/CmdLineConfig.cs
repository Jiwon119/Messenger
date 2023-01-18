using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib
{
    public class CmdLineConfig
    {
        private static CmdLineConfig inst = null;
        public static CmdLineConfig Get
        {
            get
            {
                if (null == inst)
                {
                    inst = new CmdLineConfig();
                    inst.CollectCommandLineArgs();
                }

                return inst;
            }
        }

        private Dictionary<string, string> ArgsDict = new Dictionary<string, string>();
        private CmdLineConfig() { }

        private void CollectCommandLineArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; ++i)
            {
                if (!args[i].StartsWith("--"))
                    continue;

                if (args.Length <= i + 1)
                    return;

                //string.IsNullOrEmpty()
                string key = args[i++].Remove(0, 2);
                string val = args[i];

                if (string.IsNullOrEmpty(key))
                    continue;

                ArgsDict.Add(key, val);
            }
        }

        public string GetArgs(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";

            if (!ArgsDict.ContainsKey(key))
                return "";

            return ArgsDict[key];
        }
        public bool IsFlag(string key)
        {
            if (string.IsNullOrEmpty(key) ||
                !ArgsDict.ContainsKey(key))
                return false;

            return true;
        }
    }
}
