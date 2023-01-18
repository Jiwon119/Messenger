using CommonLib.Loggers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib
{
    public static class EnvConfig
    {
        public static void SetOrDefaultEnvValue(string key, string val)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(val))
            {
                NLogger.Get.Error($"Invalid key or value. key: {key}, value: {val}");
                return;
            }

            string env = GetEnvValue(key);
            if (!string.IsNullOrEmpty(env))
            {
                NLogger.Get.Error($"Already exist env value. key: {key}, cur val: {env}");
                return;
            }

            Environment.SetEnvironmentVariable(key, val);
            NLogger.Get.Info($"Set new env value. key: {key}, val: {val}");
        }

        public static string GetEnvValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return Environment.GetEnvironmentVariable(key);
        }

        public static string GetOrDefaultEnvValue(string key, string defaultValue)
        {
            string val = GetEnvValue(key);

            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val;
        }

    }
}
