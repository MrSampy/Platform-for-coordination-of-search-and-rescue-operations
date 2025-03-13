using AutoMapper;
using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteersGroupsCommands.Update
{
    public class UpdateVolunteerGroupCommandHandler : IRequestHandler<UpdateVolunteerGroupCommand>
    {
        private readonly IRepository<VolunteersGroups> _volunteerGroupRepository;
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersGroups> _cacheService;
        private readonly IMapper _mapper;

        public UpdateVolunteerGroupCommandHandler(IRepository<VolunteersGroups> volunteerGroupRepository, IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<VolunteersGroups> cacheService, IMapper mapper)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task Handle(UpdateVolunteerGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _volunteerGroupRepository.GetByGidAsync(request.VolunteerGroupDTO.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersGroups), request.VolunteerGroupDTO.GID.ToString()));
            }

            var volunteer = await _volunteerRepository.GetByGidAsync(request.VolunteerGroupDTO.VolunteerGID, cancellationToken);
            if (volunteer == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.VolunteerGroupDTO.VolunteerGID.ToString()));
            }

            var mappedEntity = _mapper.Map<VolunteersGroups>(request.VolunteerGroupDTO);
            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.Now;

            await _volunteerGroupRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
