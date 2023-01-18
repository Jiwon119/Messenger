using CommonLib.Loggers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.MessageQueue
{
    public static partial class MessageQueueFactory
    {
        public static IMessageQueue RedisMQ()
        {
            return new RedisMQ();
        }
    }

    class RedisMQ : IDisposable, IMessageQueue
    {
        internal RedisMQ()
        {

        }

        private string _globalPrefix = "";
        private ConnectionMultiplexer _connMux = null;
        private ISubscriber _mainChannel = null;
        private bool _isInit = false;
        private bool _isConnected = false;

        private Dictionary<string, ChannelMessageQueue> _subMessages = 
            new Dictionary<string, ChannelMessageQueue>(); // channel name, channel's message queue

        public string GlobalPrefix { get => _globalPrefix; set => _globalPrefix = value; }

        public bool IsValid => _isInit && _isConnected;

        public bool ConnectMessageQueue(MQOptions opt, Action<bool> onConnected = null)
        {
            if (_isInit)
            {
                NLogger.Get.Error("already init complete");
                return false;
            }   

            string ip = string.IsNullOrEmpty(opt.IpAddr) ? EnvConfig.GetEnvValue("TARGET_REDIS_IP") : opt.IpAddr;
            string port = string.IsNullOrEmpty(opt.Port) ? EnvConfig.GetEnvValue("TARGET_REDIS_PORT") : opt.Port.ToString();
            if (string.IsNullOrEmpty(ip)) ip = "localhost"; // default
            if (string.IsNullOrEmpty(port)) port = "6379"; // default

            NLogger.Get.Info($"접속정보 ip: {ip}, port: {port}");

            try
            {   
                ConfigurationOptions option = new ConfigurationOptions();
                option.AbortOnConnectFail = true;
                option.ConnectRetry = 10;
                option.ConnectTimeout = 10000;
                _connMux = ConnectionMultiplexer.Connect($"{ip}:{port}");
                _isConnected = true;
            }
            catch (Exception e)
            {
                NLogger.Get.Error(e.Message);
                return false;
            }

            if (null == _connMux)
                return false;

            _connMux.InternalError += ConnMux_InternalError;

            _mainChannel = _connMux.GetSubscriber();
            if (null == _mainChannel)
                return false;

            NLogger.Get.Info("init MQ complete");
            _isInit = true;
            return true;
        }
        public async Task<bool> ConnectMessageQueueAsync(MQOptions opt, Action<bool> onConnected = null)
        {
            if (_isInit)
            {
                NLogger.Get.Error("already init complete");
                return false;
            }

            string ip = string.IsNullOrEmpty(opt.IpAddr) ? EnvConfig.GetEnvValue("TARGET_REDIS_IP") : opt.IpAddr;
            string port = string.IsNullOrEmpty(opt.Port) ? EnvConfig.GetEnvValue("TARGET_REDIS_PORT") : opt.Port.ToString();
            if (string.IsNullOrEmpty(ip)) ip = "localhost"; // default
            if (string.IsNullOrEmpty(port)) port = "6379"; // default

            NLogger.Get.Info($"접속정보 ip: {ip}, port: {port}");

            try
            {
                ConfigurationOptions option = new ConfigurationOptions();
                option.AbortOnConnectFail = true;
                option.ConnectRetry = 10;
                option.ConnectTimeout = 10000;
                _connMux = await ConnectionMultiplexer.ConnectAsync($"{ip}:{port}");
                _isConnected = true;
            }
            catch (Exception e)
            {
                NLogger.Get.Error(e.Message);
                return false;
            }

            if (null == _connMux)
                return false;

            _connMux.InternalError += ConnMux_InternalError;

            _mainChannel = _connMux.GetSubscriber();
            if (null == _mainChannel)
                return false;

            NLogger.Get.Info("init MQ complete");
            _isInit = true;
            return true;
        }


        private void ConnMux_InternalError(object sender, InternalErrorEventArgs e)
        {
            NLogger.Get.Error(e.ToString());
        }

        public void DisconnectMessageQueue()
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

        public void Dispose()
        {
            DisconnectMessageQueue();
        }

        public void ForeachAllReceivedMessages(Action<string, SFM_Message> func)
        {
            //foreach 
            foreach (var elem in _subMessages)
            {
                if (null == elem.Value)
                    continue;

                var chName = elem.Key;
                var msgQueue = elem.Value;
                ChannelMessage msg;
                while (msgQueue.TryRead(out msg))
                {
                    if (msg.Channel.IsNullOrEmpty ||
                        msg.Message.IsNullOrEmpty)
                    {
                        NLogger.Get.Error($"invalid msg received. ch: {msg.Channel}, msg: {msg.Message}");
                        continue;
                    }

                    SFM_Message sfm = new SFM_Message();
                    try { sfm.SerializeMessage = msg.Message; }
                    catch (Exception e)
                    {
                        NLogger.Get.Error($"serialize failed. e: {e.Message}");
                        continue;
                    }
                    
                    func?.Invoke(chName, sfm);
                }
            }
        }

        public long PublishMessage(string channel, SFM_Message msg)
        {
            do
            {
                if (null == msg || null == _mainChannel)
                {
                    NLogger.Get.Error("invalid mq state");
                    break;
                }
                    

                if (string.IsNullOrEmpty(channel) || !msg.IsValid)
                    break;

                long count = _mainChannel.Publish(
                    new RedisChannel(channel, RedisChannel.PatternMode.Literal),
                    new RedisValue(msg.SerializeMessage));

                return count;

            } while (false);

            // something wrong;
            return -1;
        }
        public async Task<long> PublishMessageAsync(string channel, SFM_Message msg)
        {
            do
            {
                if (null == msg || null == _mainChannel)
                {
                    NLogger.Get.Error("invalid mq state");
                    break;
                }


                if (string.IsNullOrEmpty(channel) || !msg.IsValid)
                    break;

                long count = await _mainChannel.PublishAsync(
                    new RedisChannel(channel, RedisChannel.PatternMode.Literal),
                    new RedisValue(msg.SerializeMessage));

                return count;

            } while (false);

            // something wrong;
            return -1;
        }

        public bool SubscribeChannel(string channel)
        {
            if (null == _mainChannel)
            {
                NLogger.Get.Error("invalid mq state");
                return false;
            }                

            if (string.IsNullOrEmpty(channel) ||
                string.IsNullOrWhiteSpace(channel))
            {
                NLogger.Get.Error($"invalid channel name. ch: {channel}");
                return false;
            }
                

            if (_subMessages.ContainsKey(channel))
            {
                NLogger.Get.Error($"already subscribe channel. ch: {channel}");
                return false;
            }   

            var msgQueue = _mainChannel.Subscribe(new RedisChannel(channel, RedisChannel.PatternMode.Literal));
            if (null == msgQueue)
            {
                NLogger.Get.Error($"subscribe failed. ch: {channel}");
                return false;
            }

            _subMessages.Add(channel, msgQueue);
            return true;
        }
        public async Task<bool> SubscribeChannelAsync(string channel)
        {
            if (null == _mainChannel)
            {
                NLogger.Get.Error("invalid mq state");
                return false;
            }

            if (string.IsNullOrEmpty(channel) ||
                string.IsNullOrWhiteSpace(channel))
            {
                NLogger.Get.Error($"invalid channel name. ch: {channel}");
                return false;
            }


            if (_subMessages.ContainsKey(channel))
            {
                NLogger.Get.Error($"already subscribe channel. ch: {channel}");
                return false;
            }

            var msgQueue = await _mainChannel.SubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Literal));
            if (null == msgQueue)
            {
                NLogger.Get.Error($"subscribe failed. ch: {channel}");
                return false;
            }

            _subMessages.Add(channel, msgQueue);
            return true;
        }


        public void UnsubscribeAllChannels()
        {
            if (null == _mainChannel)
                return;

            _mainChannel.UnsubscribeAll(CommandFlags.FireAndForget);
            _subMessages.Clear();
        }

        public bool UnsubscribeChannel(string channel)
        {
            if (null == _mainChannel)
            {
                NLogger.Get.Error("invalid mq state");
                return false;
            }

            if (string.IsNullOrEmpty(channel) ||
                string.IsNullOrWhiteSpace(channel))
            {
                NLogger.Get.Error($"invalid channel name. ch: {channel}");
                return false;
            }

            if (!_subMessages.ContainsKey(channel))
            {
                NLogger.Get.Error($"cannot found channel name. ch: {channel}");
                return false;
            }

            _mainChannel.Unsubscribe(new RedisChannel(channel, RedisChannel.PatternMode.Literal));
            _subMessages.Remove(channel);
            return true;
        }

    }
}
