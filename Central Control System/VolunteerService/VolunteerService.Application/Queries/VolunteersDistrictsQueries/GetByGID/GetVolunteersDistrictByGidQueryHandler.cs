using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.GetByGID
{
    public class GetVolunteersDistrictByGidQueryHandler : IRequestHandler<GetVolunteersDistrictByGidQuery, VolunteersDistrictsDTO>
    {
        private readonly IRepository<VolunteersDistricts> _repository;
        private readonly IMapper _mapper;

        public GetVolunteersDistrictByGidQueryHandler(IRepository<VolunteersDistricts> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VolunteersDistrictsDTO> Handle(GetVolunteersDistrictByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByGidAsync(request.GID, cancellationToken);
            return result == null
                ? throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersDistricts), request.GID.ToString()))
                : _mapper.Map<VolunteersDistrictsDTO>(result);
        }
    }

}
