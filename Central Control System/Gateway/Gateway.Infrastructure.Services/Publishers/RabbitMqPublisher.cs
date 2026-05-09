using Gateway.Domain.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Gateway.Infrastructure.Services.Publishers
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly IConnectionFactory _factory;
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqPublisher(IConnectionFactory factory)
        {
            _factory = factory;
        }

        private void EnsureConnected()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _factory.CreateConnection();
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _connection.CreateModel();
            }
        }

        public void Publish<T>(string queue, T message)
        {
            EnsureConnected();

            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(exchange: "", routingKey: queue, body: body);
        }
    }
}
