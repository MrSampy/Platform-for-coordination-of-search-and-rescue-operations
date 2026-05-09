using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationTaskQueries.GetByGID
{
    public class GetOperationTaskByGidQueryHandler : IRequestHandler<GetOperationTaskByGidQuery, OperationTaskDTO>
    {
        private readonly IRepository<OperationTask> _operationTaskRepository;
        private readonly IMapper _mapper;

        public GetOperationTaskByGidQueryHandler(IRepository<OperationTask> operationTaskRepository, IMapper mapper)
        {
            _operationTaskRepository = operationTaskRepository;
            _mapper = mapper;
        }

        public async Task<OperationTaskDTO> Handle(GetOperationTaskByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _operationTaskRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTask), request.GID.ToString()))
                : _mapper.Map<OperationTaskDTO>(result);
        }
    }
}
