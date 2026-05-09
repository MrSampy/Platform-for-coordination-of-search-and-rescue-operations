using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersDistrictsQueries.Create
{
    public class CreateVolunteersDistrictQueryHandler : IRequestHandler<CreateVolunteersDistrictQuery, VolunteersDistrictsDTO>
    {
        private readonly IRepository<VolunteersDistricts> _repository;
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersDistricts> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public CreateVolunteersDistrictQueryHandler(IRepository<VolunteersDistricts> repository, IRepository<Volunteer> volunteerRepository,
            IUnitOfWork unitOfWork, ICacheService<VolunteersDistricts> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _repository = repository;
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task<VolunteersDistrictsDTO> Handle(CreateVolunteersDistrictQuery request, CancellationToken cancellationToken)
        {
            var volunteer = await _volunteerRepository.GetByGidAsync(request.VolunteersDistrictDTO.VolunteerGID, cancellationToken);
            if (volunteer == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.VolunteersDistrictDTO.VolunteerGID.ToString()));
            }

            var district = await _apiBuilder.GetRequest<DistrictDTO>($"utils/api/district/{request.VolunteersDistrictDTO.DistrictGID}", Constants.UtilsService, cancellationToken, request.Token);
            if (district == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, "District", request.VolunteersDistrictDTO.DistrictGID.ToString()));
            }

            var entity = _mapper.Map<VolunteersDistricts>(request.VolunteersDistrictDTO);
            entity.GID = Guid.NewGuid();
            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
            return _mapper.Map<VolunteersDistrictsDTO>(entity);
        }
    }
}
