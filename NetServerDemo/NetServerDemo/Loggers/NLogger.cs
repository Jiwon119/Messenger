using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetServerDemo.Loggers
{
    public class NLogger
    {
        private static Logger _logger;

        public static Logger Get
        {
            get
            {
                if (_logger == null)
                    _logger = Init();

                return _logger;
            }
        }

        public static Logger Init()
        {
            Logger result = LogManager.GetCurrentClassLogger();

            NLog.LogManager.ThrowExceptions = true;

            var config = new NLog.Config.LoggingConfiguration();

            var logconsole = new ColoredConsoleTarget("logconsole")
            {
                Layout = "${environment-user:userName=true:DefaultUser=true}) ${longdate} <${level:uppercase=true}> [${callsite:includeNamespace=false:cleanNamesOfAsyncContinuations=True}] ${message}"
            };
            logconsole.UseDefaultRowHighlightingRules = true;

            // Rules for mapping loggers to targets
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            //config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            return result;
        }

        public static void Terminate()
        {
            LogManager.Flush();
            LogManager.Shutdown();
        }
    }
}
