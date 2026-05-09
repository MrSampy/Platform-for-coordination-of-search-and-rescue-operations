using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.GetByGID
{
    public class GetVolunteersEventsByGidQueryHandler : IRequestHandler<GetVolunteersEventsByGidQuery, VolunteersEventsDTO>
    {
        private readonly IRepository<VolunteersEvents> _repository;
        private readonly IMapper _mapper;

        public GetVolunteersEventsByGidQueryHandler(IRepository<VolunteersEvents> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VolunteersEventsDTO> Handle(GetVolunteersEventsByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByGidAsync(request.GID, cancellationToken);
            return result == null
                ? throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersEvents), request.GID.ToString()))
                : _mapper.Map<VolunteersEventsDTO>(result);
        }
    }
}
