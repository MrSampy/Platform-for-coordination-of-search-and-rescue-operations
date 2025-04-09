using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerQueries.Create
{
    public class CreateVolunteerQueryHandler : IRequestHandler<CreateVolunteerQuery, VolunteerDTO>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Volunteer> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public CreateVolunteerQueryHandler(IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<Volunteer> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task<VolunteerDTO> Handle(CreateVolunteerQuery request, CancellationToken cancellationToken)
        {
            var user = await _apiBuilder.GetRequest<UserDTO>($"api/user/bygid/{request.VolunteerDTO.UserGID}", Constants.AuthService, cancellationToken, request.Token);
            if (user == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, "User", request.VolunteerDTO.UserGID.ToString()));
            }

            var entityWithSameEmail = (await _volunteerRepository.GetAllAsync(cancellationToken)).FirstOrDefault(v => v.Email == request.VolunteerDTO.Email);

            if (entityWithSameEmail != null)
            {
                throw new VolunteerServiceException(Constants.VolunteerWithSuchEmailException);
            }

            var volunteer = _mapper.Map<Volunteer>(request.VolunteerDTO);
            volunteer.GID = Guid.NewGuid();

            volunteer.CreatedAt = volunteer.UpdatedAt = DateTime.UtcNow;

            await _volunteerRepository.AddAsync(volunteer);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<VolunteerDTO>(volunteer);
        }
    }
}
