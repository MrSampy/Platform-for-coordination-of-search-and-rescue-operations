using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.OperationTaskStatusQueries.GetByGID
{

    public class GetOperationTaskStatusByGidQueryHandler : IRequestHandler<GetOperationTaskStatusByGidQuery, OperationTaskStatusDTO>
    {
        private readonly IRepository<OperationTaskStatus> _operationTaskStatusRepository;
        private readonly IMapper _mapper;

        public GetOperationTaskStatusByGidQueryHandler(IRepository<OperationTaskStatus> operationTaskStatusRepository, IMapper mapper)
        {
            _operationTaskStatusRepository = operationTaskStatusRepository;
            _mapper = mapper;
        }

        public async Task<OperationTaskStatusDTO> Handle(GetOperationTaskStatusByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _operationTaskStatusRepository.GetByGidAsync(request.GID, cancellationToken);
            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(OperationTaskStatus), request.GID.ToString()))
                : _mapper.Map<OperationTaskStatusDTO>(result);
        }
    }

}
