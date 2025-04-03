using AutoMapper;
using VolunteerService.Application.DTOs;
using VolunteerService.Application.DTOs.Create;
using VolunteerService.Application.DTOs.Update;
using VolunteerService.Domain.Entities;

namespace VolunteerService.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Volunteer Mapping
            CreateMap<Volunteer, VolunteerDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc)))
                .ReverseMap();
            CreateMap<CreateVolunteerDTO, Volunteer>();
            CreateMap<UpdateVolunteerDTO, Volunteer>();

            // VolunteersDistricts Mapping
            CreateMap<VolunteersDistricts, VolunteersDistrictsDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateVolunteersDistrictsDTO, VolunteersDistricts>();

            // VolunteersGroups Mapping
            CreateMap<VolunteersGroups, VolunteersGroupsDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt != null ? DateTime.SpecifyKind(src.CreatedAt.Value, DateTimeKind.Utc) : src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt != null ? DateTime.SpecifyKind(src.UpdatedAt.Value, DateTimeKind.Utc) : src.UpdatedAt))
                .ReverseMap();
            CreateMap<CreateVolunteersGroupsDTO, VolunteersGroups>();
        }
    }

}
