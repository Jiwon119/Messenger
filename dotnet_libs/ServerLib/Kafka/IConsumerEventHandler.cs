using System;
using Confluent.Kafka;

namespace ServerLib.Kafka
{

    public interface IConsumerEventHandler
    {
        void OnEventReceived(object msg);
    }
}