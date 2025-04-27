using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteerCommands.Update
{
    public class UpdateVolunteerCommandHandler : IRequestHandler<UpdateVolunteerCommand>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Volunteer> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public UpdateVolunteerCommandHandler(IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<Volunteer> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task Handle(UpdateVolunteerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _volunteerRepository.GetByGidAsync(request.VolunteerDTO.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.VolunteerDTO.GID.ToString()));
            }

            var user = await _apiBuilder.GetRequest<UserDTO>($"api/user/bygid/{request.VolunteerDTO.UserGID}", Constants.AuthService, cancellationToken, request.Token);
            if (user == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, "User", request.VolunteerDTO.UserGID.ToString()));
            }

            if (entity.Email != request.VolunteerDTO.Email)
            {
                var entityWithSameEmail = (await _volunteerRepository.GetAllAsync(cancellationToken)).FirstOrDefault(v => v.Email == request.VolunteerDTO.Email);

                if (entityWithSameEmail != null)
                {
                    throw new VolunteerServiceException(Constants.VolunteerWithSuchEmailException);
                }
            }

            var mappedEntity = _mapper.Map<Volunteer>(request.VolunteerDTO);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _volunteerRepository.UpdateAsync(mappedEntity);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
