using Gateway.Application.Events;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Utils;
using Gateway.DTO.DTOs.Volunteers;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Gateway.Application.Consumers
{
    public class ResetCacheConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceProvider _services;

        #region Cache
        private readonly ICacheService<MessageDTO> _messageCache;
        private readonly ICacheService<EventDTO> _eventCache;
        private readonly ICacheService<EventStatusDTO> _eventStatusCache;
        private readonly ICacheService<EventTypeDTO> _eventTypeCache;
        private readonly ICacheService<GroupDTO> _groupCache;
        private readonly ICacheService<OperationTaskDTO> _operationTaskCache;
        private readonly ICacheService<OperationTaskStatusDTO> _operationTaskStatusCache;
        private readonly ICacheService<OperationWorkerDTO> _operationWorkerCache;
        private readonly ICacheService<ResourcesEventDTO> _resourcesEventCache;
        private readonly ICacheService<DistrictDTO> _districtCache;
        private readonly ICacheService<ResourceDTO> _resourceCache;
        private readonly ICacheService<MeasurementUnitDTO> _measurementUnitCache;
        private readonly ICacheService<VolunteerDTO> _volunteerCache;
        private readonly ICacheService<VolunteersDistrictsDTO> _volunteersDistrictsCache;
        private readonly ICacheService<VolunteersGroupsDTO> _volunteersGroupsCache;
        private readonly ICacheService<VolunteersEventsDTO> _volunteersEventsCache;
        #endregion

        public ResetCacheConsumer(IModel channel, IServiceProvider services,
            ICacheService<MessageDTO> messageCache, ICacheService<EventDTO> eventCache, ICacheService<EventStatusDTO> eventStatusCache,
            ICacheService<EventTypeDTO> eventTypeCache, ICacheService<GroupDTO> groupCache, ICacheService<OperationTaskDTO> operationTaskCache,
            ICacheService<OperationTaskStatusDTO> operationTaskStatusCache, ICacheService<OperationWorkerDTO> operationWorkerCache,
            ICacheService<ResourcesEventDTO> resourcesEventCache, ICacheService<DistrictDTO> districtCache, ICacheService<ResourceDTO> resourceCache,
            ICacheService<MeasurementUnitDTO> measurementUnitCache, ICacheService<VolunteerDTO> volunteerCache, ICacheService<VolunteersDistrictsDTO> volunteersDistrictsCache,
            ICacheService<VolunteersGroupsDTO> volunteersGroupsCache, ICacheService<VolunteersEventsDTO> volunteersEventsCache)
        {
            _channel = channel;
            _services = services;
            _messageCache = messageCache;
            _eventCache = eventCache;
            _eventStatusCache = eventStatusCache;
            _eventTypeCache = eventTypeCache;
            _groupCache = groupCache;
            _operationTaskCache = operationTaskCache;
            _operationTaskStatusCache = operationTaskStatusCache;
            _operationWorkerCache = operationWorkerCache;
            _resourcesEventCache = resourcesEventCache;
            _districtCache = districtCache;
            _resourceCache = resourceCache;
            _measurementUnitCache = measurementUnitCache;
            _volunteerCache = volunteerCache;
            _volunteersDistrictsCache = volunteersDistrictsCache;
            _volunteersGroupsCache = volunteersGroupsCache;
            _volunteersEventsCache = volunteersEventsCache;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare("reset.cache", durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var evt = JsonSerializer.Deserialize<ResetCacheEvent>(json);
                evt ??= new ResetCacheEvent();

                switch (evt.EntityName)
                {
                    case nameof(MessageDTO):
                        _messageCache.Reset();
                        break;
                    case nameof(EventDTO):
                        _eventCache.Reset();
                        break;
                    case nameof(EventStatusDTO):
                        _eventStatusCache.Reset();
                        break;
                    case nameof(EventTypeDTO):
                        _eventTypeCache.Reset();
                        break;
                    case nameof(GroupDTO):
                        _groupCache.Reset();
                        break;
                    case nameof(OperationTaskDTO):
                        _operationTaskCache.Reset();
                        break;
                    case nameof(OperationTaskStatusDTO):
                        _operationTaskStatusCache.Reset();
                        break;
                    case nameof(OperationWorkerDTO):
                        _operationWorkerCache.Reset();
                        break;
                    case nameof(ResourcesEventDTO):
                        _resourcesEventCache.Reset();
                        break;
                    case nameof(DistrictDTO):
                        _districtCache.Reset();
                        break;
                    case nameof(ResourceDTO):
                        _resourceCache.Reset();
                        break;
                    case nameof(MeasurementUnitDTO):
                        _measurementUnitCache.Reset();
                        break;
                    case nameof(VolunteerDTO):
                        _volunteerCache.Reset();
                        break;
                    case nameof(VolunteersDistrictsDTO):
                        _volunteersDistrictsCache.Reset();
                        break;
                    case nameof(VolunteersGroupsDTO):
                        _volunteersGroupsCache.Reset();
                        break;
                    case nameof(VolunteersEventsDTO):
                        _volunteersEventsCache.Reset();
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown entity name: {evt.EntityName}");
                }

                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue: "reset.cache", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
