using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteersGroupsQueries.Create
{
    public class CreateVolunteerGroupQueryHandler : IRequestHandler<CreateVolunteerGroupQuery, VolunteersGroupsDTO>
    {
        private readonly IRepository<VolunteersGroups> _volunteerGroupRepository;
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersGroups> _cacheService;
        private readonly IMapper _mapper;

        public CreateVolunteerGroupQueryHandler(IRepository<VolunteersGroups> volunteerGroupRepository, IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<VolunteersGroups> cacheService, IMapper mapper)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<VolunteersGroupsDTO> Handle(CreateVolunteerGroupQuery request, CancellationToken cancellationToken)
        {
            var volunteer = await _volunteerRepository.GetByGidAsync(request.VolunteerGroupDTO.VolunteerGID, cancellationToken);
            if (volunteer == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.VolunteerGroupDTO.VolunteerGID.ToString()));
            }

            var volunteerGroup = _mapper.Map<VolunteersGroups>(request.VolunteerGroupDTO);
            volunteerGroup.GID = Guid.NewGuid();
            volunteerGroup.CreatedAt = volunteerGroup.UpdatedAt = DateTime.UtcNow;

            await _volunteerGroupRepository.AddAsync(volunteerGroup);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<VolunteersGroupsDTO>(volunteerGroup);
        }
    }
}
