namespace Gateway.Domain.Services.Interfaces
{
    public interface IRabbitMqPublisher
    {
        void Publish<T>(string queue, T message);
    }
}
