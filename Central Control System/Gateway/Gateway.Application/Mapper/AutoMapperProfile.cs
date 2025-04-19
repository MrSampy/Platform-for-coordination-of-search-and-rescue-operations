using AutoMapper;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Event Mapping
            CreateMap<CreateEventDTO, EventDTO>().ReverseMap();
            CreateMap<UpdateEventDTO, EventDTO>().ReverseMap();

            // EventStatus Mapping
            CreateMap<CreateEventStatusDTO, EventStatusDTO>().ReverseMap();
            CreateMap<UpdateEventStatusDTO, EventStatusDTO>().ReverseMap();

            // EventType Mapping
            CreateMap<CreateEventTypeDTO, EventTypeDTO>().ReverseMap();
            CreateMap<UpdateEventTypeDTO, EventTypeDTO>().ReverseMap();

            // Group Mapping
            CreateMap<CreateGroupDTO, GroupDTO>().ReverseMap();
            CreateMap<UpdateGroupDTO, GroupDTO>().ReverseMap();

            // OperationTask Mapping
            CreateMap<CreateOperationTaskDTO, OperationTaskDTO>().ReverseMap();
            CreateMap<UpdateOperationTaskDTO, OperationTaskDTO>().ReverseMap();

            // OperationTaskStatus Mapping
            CreateMap<CreateOperationTaskStatusDTO, OperationTaskStatusDTO>().ReverseMap();
            CreateMap<UpdateOperationTaskStatusDTO, OperationTaskStatusDTO>().ReverseMap();

            // OperationWorker Mapping
            CreateMap<CreateOperationWorkerDTO, OperationWorkerDTO>().ReverseMap();
            CreateMap<UpdateOperationWorkerDTO, OperationWorkerDTO>().ReverseMap();

            // ResourcesEvent Mapping
            CreateMap<CreateResourcesEventDTO, ResourcesEventDTO>().ReverseMap();
            CreateMap<UpdateResourcesEventDTO, ResourcesEventDTO>().ReverseMap();
        }
    }
}
