using AutoMapper;
using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.IsVolunteerinGroup
{
    public class IsVolunteerinGroupQueryHandler : IRequestHandler<IsVolunteerinGroupQuery, bool>
    {
        private readonly IRepository<VolunteersGroups> _repository;
        private readonly IMapper _mapper;

        public IsVolunteerinGroupQueryHandler(IRepository<VolunteersGroups> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(IsVolunteerinGroupQuery request, CancellationToken cancellationToken)
        {
            return (await _repository.GetAllAsync(cancellationToken)).Any(vg => vg.VolunteerGID == request.VolunteersGroupsDTO.VolunteerGID
                && vg.GroupGID == request.VolunteersGroupsDTO.GroupGID);
        }
    }
}
