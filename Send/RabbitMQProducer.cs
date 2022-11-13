using System;
using RabbitMQ.Client;
using System.Text;
using Common;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Producer
{
    class RabbitMQProducer : BaseProducer
    {
        internal readonly IModel _channel;

        public RabbitMQProducer(IConfig cfg) : base(cfg)
        {
            _channel = SetupProducer();
        }

        internal IModel SetupProducer()
        {
            var factory = new ConnectionFactory() { HostName = _cfg.hostname };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _cfg.queuename,
                                durable: _cfg.durable,
                                exclusive: _cfg.exclusive,
                                autoDelete: _cfg.autoDelete,
                                arguments: _cfg.arguments);
            return channel;
        }

        internal override async Task Publish()
        {
            await Task.Run(() =>
            {
                string message = DateTime.Now.ToString();
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: "",
                                     routingKey: _cfg.queuename,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            });
        }
    }
}