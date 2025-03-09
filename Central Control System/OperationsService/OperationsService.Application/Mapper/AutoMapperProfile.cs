using AutoMapper;
using OperationsService.Application.DTOs;
using OperationsService.Application.DTOs.Create;
using OperationsService.Application.DTOs.Update;
using OperationsService.Domain.Entities;

namespace OperationsService.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Event Mapping
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<CreateEventDTO, Event>();
            CreateMap<UpdateEventDTO, Event>();

            // EventStatus Mapping
            CreateMap<EventStatus, EventStatusDTO>().ReverseMap();
            CreateMap<CreateEventStatusDTO, EventStatus>();
            CreateMap<UpdateEventStatusDTO, EventStatus>();

            // EventType Mapping
            CreateMap<EventType, EventTypeDTO>();
            CreateMap<CreateEventTypeDTO, EventType>();
            CreateMap<UpdateEventTypeDTO, EventType>();

            // Group Mapping
            CreateMap<Group, GroupDTO>().ReverseMap();
            CreateMap<CreateGroupDTO, Group>();
            CreateMap<UpdateGroupDTO, Group>();

            // OperationTask Mapping
            CreateMap<OperationTask, OperationTaskDTO>().ReverseMap();
            CreateMap<CreateOperationTaskDTO, OperationTask>();
            CreateMap<UpdateOperationTaskDTO, OperationTask>();

            // OperationTaskStatus Mapping
            CreateMap<OperationTaskStatus, OperationTaskStatusDTO>().ReverseMap();
            CreateMap<CreateOperationTaskStatusDTO, OperationTaskStatus>();
            CreateMap<UpdateOperationTaskStatusDTO, OperationTaskStatus>();

            // OperationWorker Mapping
            CreateMap<OperationWorker, OperationWorkerDTO>().ReverseMap();
            CreateMap<CreateOperationWorkerDTO, OperationWorker>();
            CreateMap<UpdateOperationWorkerDTO, OperationWorker>();

            // ResourcesEvent Mapping
            CreateMap<ResourcesEvent, ResourcesEventDTO>().ReverseMap();
            CreateMap<CreateResourcesEventDTO, ResourcesEvent>();
            CreateMap<UpdateResourcesEventDTO, ResourcesEvent>();
        }
    }
}
