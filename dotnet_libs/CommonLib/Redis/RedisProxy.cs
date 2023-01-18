using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Loggers;
using StackExchange.Redis;

namespace CommonLib.Redis
{
    public class MessageBuilder
    { 
        public static string Build(string[] args)
        {
            if (null == args || 0 == args.Length)
                return "";

            StringBuilder b = new StringBuilder();
            foreach (string s in args)
            {
                b.Append(s);
                b.Append(",");
            }

            b.Remove(b.Length - 1, 1);
            return b.ToString();
        }
    }

    internal class RedisProxy
    {
        private RedisProxy() { }
        private static RedisProxy inst = null;
        public static RedisProxy Get
        {
            get
            {
                if (null == inst)
                {
                    inst = new RedisProxy();                    
                }
                return inst;
            }

        }

        private string _unionPrefix = "";
        private ConnectionMultiplexer _connMux = null;
        private IDatabaseAsync _mainCache = null;
        private ISubscriber _mainChannel = null;

        private Dictionary<string, ChannelMessageQueue> _subMessages = new Dictionary<string, ChannelMessageQueue>();

        public void SetUnionPrefix(string region)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(region);
            sb.Append(":");

            _unionPrefix = sb.ToString();
        }
        public string AppendPrefixKey(params string[] keys)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_unionPrefix);
            foreach (string key in keys)
            {
                sb.Append(key);
                sb.Append(":");
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public bool InitProxy()
        {
            if (null != _connMux)
                return true;

            string ip = EnvConfig.GetEnvValue("TARGET_REDIS_IP");
            string port = EnvConfig.GetEnvValue("TARGET_REDIS_PORT");
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port))
            {
                ip = "localhost"; // default
                port = "6379"; // default
            }

            try
            {
                //ConnMux = await ConnectionMultiplexer.ConnectAsync("localhost");
                //_connMux = await ConnectionMultiplexer.ConnectAsync("192.168.219.164:6379");
                //_connMux = await ConnectionMultiplexer.ConnectAsync($"{ip}:{port}");
                ConfigurationOptions opt = new ConfigurationOptions();
                opt.AbortOnConnectFail = true;
                opt.ConnectRetry = 10;
                opt.ConnectTimeout = 10000;
                _connMux = ConnectionMultiplexer.Connect($"{ip}:{port}");
            }
            catch (Exception e)
            {
                NLogger.Get.Error(e.Message);
                TerminateProxy();
                return false;
            }

            if (null == _connMux)
                return false;

            _connMux.InternalError += ConnMux_InternalError;
            _mainCache = _connMux.GetDatabase();
            if (null == _mainCache)
                return false;

            _mainChannel = _connMux.GetSubscriber();
            if (null == _mainChannel)
                return false;

            NLogger.Get.Info("init redis complete");
            return true;
        }
        private void ConnMux_InternalError(object sender, InternalErrorEventArgs e)
        {
            NLogger.Get.Error(e.ToString());
        }
        public void TerminateProxy()
        {   
            if (null != _mainChannel)
            {
                _mainChannel.UnsubscribeAll(CommandFlags.FireAndForget);
                _mainChannel = null;
            }

            if (null != _connMux)
            {
                _connMux.Close();
                _connMux = null;
            }
        }

        public async Task<bool> ExecRedisCommand(IRedisCommand cmd)
        {
            if (null == cmd)
                return false;

            if (null == _mainCache)
                return false;

            bool result = await cmd.DoRedisCommand(_mainCache);
            return result;
        }

        public bool SubscribeChannel(string ch)
        {
            if (null == _mainChannel)
                return false;

            if (string.IsNullOrEmpty(ch) ||
                string.IsNullOrWhiteSpace(ch))
                return false;

            if (_subMessages.ContainsKey(ch))
                return false;

            var msgQueue = _mainChannel.Subscribe(new RedisChannel(ch, RedisChannel.PatternMode.Literal));
            _subMessages.Add(ch, msgQueue);
            return true;
        }
        public void UnsubscribeAllChannels()
        {
            if (null == _mainChannel)
                return;

            _mainChannel.UnsubscribeAll(CommandFlags.FireAndForget);
        }

        public bool PublishMessage(string ch, string msg)
        {
            if (null == _mainChannel)
                return false;

            if (string.IsNullOrEmpty(ch) ||
                string.IsNullOrWhiteSpace(ch))
                return false;

            long count = _mainChannel.Publish(
                new RedisChannel(ch, RedisChannel.PatternMode.Literal),
                new RedisValue(msg));

            if (0 == count)
            {
                NLogger.Get.Error($"no one listened this channel. channel name: {ch}");
            }

            return true;
        }
        public void PollingAllMessageQueues(Action<string, string> func)
        {
            foreach (var elem in _subMessages)
            {
                if (null == elem.Value)
                    continue;

                var key = elem.Key;
                var queue = elem.Value;
                ChannelMessage msg;
                while (queue.TryRead(out msg))
                {
                    if (msg.Channel.IsNullOrEmpty ||
                        msg.Message.IsNullOrEmpty)
                        continue;

                    func?.Invoke(msg.Channel.ToString(), msg.Message.ToString());
                }
            }
        }
    }
}
