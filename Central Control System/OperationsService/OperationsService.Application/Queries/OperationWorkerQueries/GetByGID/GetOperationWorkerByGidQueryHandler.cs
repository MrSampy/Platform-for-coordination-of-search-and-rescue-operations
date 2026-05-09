using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationWorkerQueries.GetByGID
{
    public class GetOperationWorkerByGidQueryHandler : IRequestHandler<GetOperationWorkerByGidQuery, OperationWorkerDTO>
    {
        private readonly IRepository<OperationWorker> _operationWorkerRepository;
        private readonly IMapper _mapper;

        public GetOperationWorkerByGidQueryHandler(IRepository<OperationWorker> operationWorkerRepository, IMapper mapper)
        {
            _operationWorkerRepository = operationWorkerRepository;
            _mapper = mapper;
        }

        public async Task<OperationWorkerDTO> Handle(GetOperationWorkerByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _operationWorkerRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationWorker), request.GID.ToString()))
                : _mapper.Map<OperationWorkerDTO>(result);
        }
    }
}
