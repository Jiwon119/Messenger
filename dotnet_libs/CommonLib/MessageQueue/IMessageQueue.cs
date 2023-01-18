using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.MessageQueue
{
    public struct MQOptions
    {
        public string IpAddr;
        public string Port;
        public int TimeoutMs;
        public int RetryCount;
        public string ConfigStr;
    }

    // threading 고려 안함.
    public interface IMessageQueue
    {
        string GlobalPrefix { get; set; }
        bool IsValid { get; }
        bool ConnectMessageQueue(MQOptions opt, Action<bool> onConnected = null);
        void DisconnectMessageQueue();
        bool SubscribeChannel(string channel);
        bool UnsubscribeChannel(string channel);
        void UnsubscribeAllChannels();
        long PublishMessage(string channel, SFM_Message msg);
        void ForeachAllReceivedMessages(Action<string, SFM_Message> func);

        Task<bool> ConnectMessageQueueAsync(MQOptions opt, Action<bool> onConnected = null);
        Task<bool> SubscribeChannelAsync(string channel);
        Task<long> PublishMessageAsync(string channel, SFM_Message msg);
        
    }
}
