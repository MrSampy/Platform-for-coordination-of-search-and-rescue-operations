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
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateEventDTO, Event>();
            CreateMap<UpdateEventDTO, Event>();

            // Event Message
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateMessageDTO, Message>();

            // EventStatus Mapping
            CreateMap<EventStatus, EventStatusDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateEventStatusDTO, EventStatus>();
            CreateMap<UpdateEventStatusDTO, EventStatus>();

            // EventType Mapping
            CreateMap<EventType, EventTypeDTO>();
            CreateMap<CreateEventTypeDTO, EventType>();
            CreateMap<UpdateEventTypeDTO, EventType>();

            // Group Mapping
            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateGroupDTO, Group>();
            CreateMap<UpdateGroupDTO, Group>();

            // OperationTask Mapping
            CreateMap<OperationTask, OperationTaskDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateOperationTaskDTO, OperationTask>();
            CreateMap<UpdateOperationTaskDTO, OperationTask>();

            // OperationTaskStatus Mapping
            CreateMap<OperationTaskStatus, OperationTaskStatusDTO>().ReverseMap();
            CreateMap<CreateOperationTaskStatusDTO, OperationTaskStatus>();
            CreateMap<UpdateOperationTaskStatusDTO, OperationTaskStatus>();

            // OperationWorker Mapping
            CreateMap<OperationWorker, OperationWorkerDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc)))
                .ReverseMap();
            CreateMap<CreateOperationWorkerDTO, OperationWorker>();
            CreateMap<UpdateOperationWorkerDTO, OperationWorker>();

            // ResourcesEvent Mapping
            CreateMap<ResourcesEvent, ResourcesEventDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateResourcesEventDTO, ResourcesEvent>();
            CreateMap<UpdateResourcesEventDTO, ResourcesEvent>();
        }
    }
}
