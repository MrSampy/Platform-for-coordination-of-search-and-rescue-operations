using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerQueries.Create
{
    public class CreateVolunteerQueryHandler : IRequestHandler<CreateVolunteerQuery, VolunteerDTO>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Volunteer> _cacheService;
        private readonly IMapper _mapper;

        public CreateVolunteerQueryHandler(IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<Volunteer> cacheService, IMapper mapper)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<VolunteerDTO> Handle(CreateVolunteerQuery request, CancellationToken cancellationToken)
        {
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
