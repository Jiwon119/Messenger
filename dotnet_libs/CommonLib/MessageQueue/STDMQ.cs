using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MQTTnet;
using MQTTnet.Client;

namespace CommonLib.MessageQueue
{

    public static partial class MessageQueueFactory
    {
        public static IMessageQueue STDMQ()
        {


            return new STDMQ();
        }
    }


    public class STDMQ : IMessageQueue, IDisposable
    {
        public string GlobalPrefix { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsValid => throw new NotImplementedException();

        

        public bool ConnectMessageQueue(MQOptions opt, Action<bool> onConnected = null)
        {
            var factory = new MqttFactory();
            factory.CreateMqttClient();

            throw new NotImplementedException();
        }

        public Task<bool> ConnectMessageQueueAsync(MQOptions opt, Action<bool> onConnected = null)
        {
            throw new NotImplementedException();
        }

        public void DisconnectMessageQueue()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ForeachAllReceivedMessages(Action<string, SFM_Message> func)
        {
            throw new NotImplementedException();
        }

        public long PublishMessage(string channel, SFM_Message msg)
        {
            throw new NotImplementedException();
        }

        public Task<long> PublishMessageAsync(string channel, SFM_Message msg)
        {
            throw new NotImplementedException();
        }

        public bool SubscribeChannel(string channel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubscribeChannelAsync(string channel)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAllChannels()
        {
            throw new NotImplementedException();
        }

        public bool UnsubscribeChannel(string channel)
        {
            throw new NotImplementedException();
        }
    }
}
