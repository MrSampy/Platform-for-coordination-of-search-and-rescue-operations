using MediatR;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.GroupCommands.Delete
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Group> _cacheService;

        public DeleteGroupCommandHandler(IRepository<Group> groupRepository, IUnitOfWork unitOfWork, ICacheService<Group> cacheService)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _groupRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Group), request.GID.ToString()));
            }

            await _groupRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
