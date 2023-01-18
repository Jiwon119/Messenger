using CommonLib;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.Kafka
{
    public class KafkaConsumer
    {
        // public KafkaConsumer(string topic, string partition, string groupId)
        // {
        //     _topic = topic;
        //     _partition = partition;
        //     _groupId = groupId; 
        // }
        public KafkaConsumer(int id, string topic, IConsumerEventHandler handler)
        {
            ConsumerId = id;
            Topic = topic;
            _eventHandler = handler;
        }

        public KafkaConsumer(int id, string topic, Action<ConsumeResult<string, byte[]>> handler)
        {
            ConsumerId = id;
            Topic = topic;
            _actionHandler = handler;
        }

        private Thread? _consumerThread = null;
        private long _isStop = 0;
        private Action<ConsumeResult<string, byte[]>> _actionHandler;
        private IConsumerEventHandler _eventHandler;
        public int ConsumerId { get; private set; }
        public string Topic { get; private set; }

        private string? _partition;
        private string? _groupId;
        //private IConfiguration? _config;
        private ConsumerConfig? _config;

        // public bool InitConsumer(string iniPath)
        // {
        //     if (null != _config)
        //         return false;

        //     var bootstrap_servers = EnvConfig.GetOrDefaultEnvValue("BOOTSTRAP_SERVERS", "");

        //     //_config = new ConfigurationBuilder()
        //     //    .Build();

        //     //_topic = "dev";
        //     //_config["group.id"] = "kafka-test";
        //     //_config["auto.offset.reset"] = "earliest";
        //     //_config["enable.auto.commit"] = "true";
        //     //_config["bootstrap.servers"] = "127.0.0.1:9002";

        //     _config = new ConsumerConfig()
        //     {
        //         //BootstrapServers = "127.0.0.1:9003",
        //         BootstrapServers = "192.168.0.55:9093",
        //         GroupId = "kafka-test",
        //         GroupInstanceId = "kafka-inst-test",
        //         AutoOffsetReset = AutoOffsetReset.Earliest,
        //         EnableAutoCommit = true,
        //         SessionTimeoutMs = 60 * 1000,
        //     };

        //     return true;
        // }

        public bool InitConsumer(string uri, string groupId, string? groupInstId, string? iniPath)
        {
            if (string.IsNullOrWhiteSpace(uri) || string.IsNullOrWhiteSpace(groupId))
            {
                return false;
            }

            _config = new ConsumerConfig()
            {
                BootstrapServers = uri,
                GroupId = groupId,
                GroupInstanceId = groupInstId ?? null,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                SessionTimeoutMs = 60 * 1000,
            };

            return true;
        }

        public void RunConsumer()
        {
            _isStop = 0;
            _consumerThread = new Thread(ConsumerMain);
            _consumerThread.Start();
        }

        public void StopConsumer()
        {
            Interlocked.Increment(ref _isStop);
        }
        
        public void JoinConsumer()
        {
            if (null != _consumerThread)
            {
                _consumerThread.Join();
                _consumerThread = null;
            }
        }


        private void ConsumerMain()
        {
            CancellationTokenSource src = new CancellationTokenSource();
            //using (var consumer = new ConsumerBuilder<string, string>(_config.AsEnumerable()).Build())
            using (var consumer = new ConsumerBuilder<string, byte[]>(_config).Build())
            {
                consumer.Subscribe(Topic);
                while (0 == _isStop)
                {
                    try
                    {
                        var cr = consumer.Consume(src.Token);
                        if (null == cr.Message)
                            continue;

                        //_eventHandler.OnEventReceived(cr);
                        _actionHandler?.Invoke(cr);
                        //Console.WriteLine($"Consume message. key: {cr.Message.Key}, value: {cr.Message.Value}");
                    }
                    catch (OperationCanceledException e)
                    {
                        // TODO
                        // 연결이 끊겼을 시 처리를 해야함.
                        continue;
                    }
                    catch (ConsumeException e)
                    {
                        // TODO
                        // 연결이 끊겼을 시 처리를 해야함.
                        break;
                    }
                }
            }

            // cleanup
        }
    }
}
