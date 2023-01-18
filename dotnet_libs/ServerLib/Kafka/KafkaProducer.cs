using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.Kafka
{
    public class KafkaProducer
    {
        public KafkaProducer(string topic, string partition, string groupId)
        {
            _topic = topic;
            _partition = partition;
            _groupId = groupId;
        }

        private Thread? _producerThread = null;
        private long _isStop = 0;

        private string _topic;
        private string _partition;
        private string _groupId;
        //private IConfiguration? _config;
        private ProducerConfig _config;

        public bool InitProducer(string iniPath)
        {
            if (null != _config)
                return false;


            //_config = new ConfigurationBuilder()
            //    .Build();

            //_topic = "dev";
            //_config["group.id"] = "kafka-test";
            //_config["auto.offset.reset"] = "earliest";
            //_config["enable.auto.commit"] = "true";
            //_config["bootstrap.servers"] = "127.0.0.1:9002";

            _config = new ProducerConfig
            {
                //BootstrapServers = "127.0.0.1:9003",
                BootstrapServers = "192.168.0.55:9093",
                ClientId = Dns.GetHostName(),
            };

            return true;
        }

        public void RunProducer()
        {
            _isStop = 0;
            _producerThread = new Thread(ProducerMain);
            _producerThread.Start();
        }

        public void StopProducer()
        {
            Interlocked.Increment(ref _isStop);

            if (null != _producerThread)
            {
                _producerThread.Join();
                _producerThread = null;
            }
        }


        private void ProducerMain()
        {            
            CancellationTokenSource src = new CancellationTokenSource();
            //using (var producer = new ProducerBuilder<string, string>(_config.AsEnumerable()).Build())
            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
                string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };
                Random rnd = new Random();

                for (int i = 0; i < 10; ++i)
                {
                    var usr = users[rnd.Next(users.Length)];
                    var item = items[rnd.Next(items.Length)];

                    producer.Produce(_topic, new Message<string, string> { Key = usr, Value = item },
                        (dr) =>
                        {
                            if (dr.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {dr.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced event to topic {_topic}: key = {usr,-10} value = {item}");
                            }
                        });
                }

                producer.Flush(TimeSpan.FromSeconds(10));
            }
            // cleanup
        }

    }
}
