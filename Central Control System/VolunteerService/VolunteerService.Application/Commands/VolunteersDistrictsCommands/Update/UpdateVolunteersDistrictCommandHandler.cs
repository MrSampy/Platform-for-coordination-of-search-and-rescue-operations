using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteersDistrictsCommands.Update
{
    public class UpdateVolunteersDistrictCommandHandler : IRequestHandler<UpdateVolunteersDistrictCommand>
    {
        private readonly IRepository<VolunteersDistricts> _repository;
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersDistricts> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public UpdateVolunteersDistrictCommandHandler(IRepository<VolunteersDistricts> repository, IRepository<Volunteer> volunteerRepository,
            IUnitOfWork unitOfWork, ICacheService<VolunteersDistricts> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _repository = repository;
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task Handle(UpdateVolunteersDistrictCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByGidAsync(request.VolunteersDistrictDTO.GID, cancellationToken);
            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersDistricts), request.VolunteersDistrictDTO.GID.ToString()));
            }

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

            var mappedEntity = _mapper.Map(request.VolunteersDistrictDTO, entity);
            mappedEntity.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
        }
    }
}
