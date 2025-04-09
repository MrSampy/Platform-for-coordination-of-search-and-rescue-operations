using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.OperationWorkerCommands.Update
{
    public class UpdateOperationWorkerCommandHandler : IRequestHandler<UpdateOperationWorkerCommand>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationWorker> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public UpdateOperationWorkerCommandHandler(
            IRepository<OperationWorker> operationWorkerRepository,
            IUnitOfWork unitOfWork,
            ICacheService<OperationWorker> cacheService,
            IMapper mapper,
            IApiBuilder apiBuilder)
        {
            _operationWorkerRepository = operationWorkerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task Handle(UpdateOperationWorkerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationWorkerRepository.GetByGidAsync(request.OperationWorker.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), request.OperationWorker.GID.ToString()));
            }

            var user = await _apiBuilder.GetRequest<UserDTO>($"api/user/bygid/{request.OperationWorker.UserGID}", Constants.AuthService, cancellationToken, request.Token);
            if (user == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "User", request.OperationWorker.UserGID.ToString()));
            }

            var entityWithSameEmail = (await _operationWorkerRepository.GetAllAsync(cancellationToken)).FirstOrDefault(v => v.Email == request.OperationWorker.Email);

            if (entityWithSameEmail != null)
            {
                throw new OperationsServiceException(Constants.OperationWorkerWithSuchEmailException);
            }

            var mappedEntity = _mapper.Map<OperationWorker>(request.OperationWorker);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _operationWorkerRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
