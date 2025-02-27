using AutoMapper;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Entities;

namespace UtilsService.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateDistrictDTO, District>().ReverseMap();
            CreateMap<CreateMeasurementUnitDTO, MeasurementUnit>().ReverseMap();
            CreateMap<CreateResourceDTO, Resource>().ReverseMap();
            CreateMap<CreateResourceUnitDTO, ResourceMeasurementUnit>().ReverseMap();
        }
    }
}
