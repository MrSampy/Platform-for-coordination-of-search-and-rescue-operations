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
            CreateMap<Volunteer, VolunteerDTO>().ReverseMap();
            CreateMap<CreateVolunteerDTO, Volunteer>();
            CreateMap<UpdateVolunteerDTO, Volunteer>();

            // VolunteersDistricts Mapping
            CreateMap<VolunteersDistricts, VolunteersDistrictsDTO>().ReverseMap();
            CreateMap<CreateVolunteersDistrictsDTO, VolunteersDistricts>();
            CreateMap<UpdateVolunteersDistrictsDTO, VolunteersDistricts>();

            // VolunteersGroups Mapping
            CreateMap<VolunteersGroups, VolunteersGroupsDTO>().ReverseMap();
            CreateMap<CreateVolunteersGroupsDTO, VolunteersGroups>();
            CreateMap<UpdateVolunteersGroupsDTO, VolunteersGroups>();
        }
    }

}
