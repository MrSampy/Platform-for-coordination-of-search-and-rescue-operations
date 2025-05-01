using Gateway.Application.Events;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Gateway.Application.Consumers
{
    public class SendMessageConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceProvider _services;

        public SendMessageConsumer(IModel channel, IServiceProvider services)
        {
            _channel = channel;
            _services = services;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare("token.send", durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var evt = JsonSerializer.Deserialize<SendMessageEvent>(json);
                evt ??= new SendMessageEvent();
                // Example usage of scoped services
                Console.WriteLine($"{evt.Message}");

                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue: "token.send", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
