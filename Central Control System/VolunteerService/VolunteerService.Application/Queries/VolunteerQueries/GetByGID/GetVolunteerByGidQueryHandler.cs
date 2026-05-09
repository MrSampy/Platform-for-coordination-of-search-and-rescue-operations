using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerQueries.GetByGID
{
    public class GetVolunteerByGidQueryHandler : IRequestHandler<GetVolunteerByGidQuery, VolunteerDTO>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IMapper _mapper;

        public GetVolunteerByGidQueryHandler(IRepository<Volunteer> volunteerRepository, IMapper mapper)
        {
            _volunteerRepository = volunteerRepository;
            _mapper = mapper;
        }

        public async Task<VolunteerDTO> Handle(GetVolunteerByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _volunteerRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.GID.ToString()))
                : _mapper.Map<VolunteerDTO>(result);
        }
    }

}
