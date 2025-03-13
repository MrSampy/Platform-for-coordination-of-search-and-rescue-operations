using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.GetByGID
{
    public class GetVolunteersGroupsByGidQueryHandler : IRequestHandler<GetVolunteersGroupsByGidQuery, VolunteersGroupsDTO>
    {
        private readonly IRepository<VolunteersGroups> _repository;
        private readonly IMapper _mapper;

        public GetVolunteersGroupsByGidQueryHandler(IRepository<VolunteersGroups> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VolunteersGroupsDTO> Handle(GetVolunteersGroupsByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByGidAsync(request.GID, cancellationToken);
            return result == null
                ? throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersDistricts), request.GID.ToString()))
                : _mapper.Map<VolunteersGroupsDTO>(result);
        }
    }
}
