using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MicroserviceB.Service.Services;
using MicroserviceB.Messaging.Receive.Options;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Text.Json;
using MicroserviceB.Service.Models;

namespace MicroserviceB.Messaging.Receive.Receiver
{
    public class DisplayNameConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IDisplayNameService _displayNameService;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _routingKey;

        public DisplayNameConsumer(IDisplayNameService displayNameService, IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _exchangeName = rabbitMqOptions.Value.ExchangeName;
            _routingKey = rabbitMqOptions.Value.RoutingKey;
            _displayNameService = displayNameService;
            InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(_queueName, _exchangeName, _routingKey, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                HandleMessage(ExtractNameFromMessage(message));

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private string ExtractNameFromMessage(string message)
        {
            return message.Substring(message.LastIndexOf(',') + 1).Replace("\"", string.Empty).Replace(" ", string.Empty);
        }

        private void HandleMessage(string name)
        {
            _displayNameService.DisplayName(name);
        }
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
