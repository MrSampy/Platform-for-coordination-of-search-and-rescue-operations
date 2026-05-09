using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteersGroupsCommands.Delete
{
    public class DeleteVolunteerGroupCommandHandler : IRequestHandler<DeleteVolunteerGroupCommand>
    {
        private readonly IRepository<VolunteersGroups> _volunteerGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersGroups> _cacheService;

        public DeleteVolunteerGroupCommandHandler(IRepository<VolunteersGroups> volunteerGroupRepository, IUnitOfWork unitOfWork, ICacheService<VolunteersGroups> cacheService)
        {
            _volunteerGroupRepository = volunteerGroupRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteVolunteerGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _volunteerGroupRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersGroups), request.GID.ToString()));
            }

            await _volunteerGroupRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
