using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteersEventsCommands.Delete
{
    public class DeleteVolunteersEventsCommandHandler : IRequestHandler<DeleteVolunteersEventsCommand>
    {
        private readonly IRepository<VolunteersEvents> _volunteerEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersEvents> _cacheService;

        public DeleteVolunteersEventsCommandHandler(IRepository<VolunteersEvents> volunteerGroupRepository, IUnitOfWork unitOfWork, ICacheService<VolunteersEvents> cacheService)
        {
            _volunteerEventRepository = volunteerGroupRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteVolunteersEventsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _volunteerEventRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersEvents), request.GID.ToString()));
            }

            await _volunteerEventRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
