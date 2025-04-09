using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationWorkerQueries.Create
{
    public class CreateOperationWorkerQueryHandler : IRequestHandler<CreateOperationWorkerQuery, OperationWorkerDTO>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<OperationWorker> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;
        public CreateOperationWorkerQueryHandler(
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

        public async Task<OperationWorkerDTO> Handle(CreateOperationWorkerQuery request, CancellationToken cancellationToken)
        {
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

            var operationWorker = _mapper.Map<OperationWorker>(request.OperationWorker);
            operationWorker.GID = Guid.NewGuid();
            operationWorker.CreatedAt = operationWorker.UpdatedAt = DateTime.UtcNow;
            await _operationWorkerRepository.AddAsync(operationWorker);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<OperationWorkerDTO>(operationWorker);
        }
    }

}
