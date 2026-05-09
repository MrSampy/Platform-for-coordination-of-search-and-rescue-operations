using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.ResourcesEventCommands.Delete
{
    public class DeleteResourcesEventCommandHandler : IRequestHandler<DeleteResourcesEventCommand>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<ResourcesEvent> _cacheService;

        public DeleteResourcesEventCommandHandler(
            IRepository<ResourcesEvent> resourcesEventRepository,
            IUnitOfWork unitOfWork,
            ICacheService<ResourcesEvent> cacheService)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteResourcesEventCommand request, CancellationToken cancellationToken)
        {
            var entity = await _resourcesEventRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(ResourcesEvent), request.GID.ToString()));
            }

            await _resourcesEventRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
