using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Consumer
{
    internal class RabbitMQConsumer : BaseConsumer
    {
        internal readonly IModel _channel;

        public RabbitMQConsumer(IConfig cfg, IDbConnectionFactory dbConnectionFactory) : base(cfg, dbConnectionFactory)
        {
            _channel = SetupClient();
        }

        public IModel SetupClient()
        {
            var factory = new ConnectionFactory() { HostName = _cfg.hostname };
            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _cfg.queuename,
                                     durable: _cfg.durable,
                                     exclusive: _cfg.exclusive,
                                     autoDelete: _cfg.autoDelete,
                                     arguments: _cfg.arguments);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            return channel;
        }

        internal override void ListenEvents()
        {
            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool Publish = await ProcessMessage(message);

                if (Publish)
                {
                    string newMessage = DateTime.Now.AddSeconds(-90).ToString();
                    var newBody = Encoding.UTF8.GetBytes(newMessage);

                    _channel.BasicPublish(exchange: "",
                                         routingKey: _cfg.queuename,
                                         basicProperties: null,
                                         body: body);
                }
                // Note: it is possible to access the channel via
                //       ((EventingBasicConsumer)sender).Model here
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: _cfg.queuename,
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
