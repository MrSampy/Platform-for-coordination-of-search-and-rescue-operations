using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.ResourcesEventQueries.GetByGID
{
    public class GetResourcesEventByGidQueryHandler : IRequestHandler<GetResourcesEventByGidQuery, ResourcesEventDTO>
    {
        private readonly IRepository<ResourcesEvent> _resourcesEventRepository;
        private readonly IMapper _mapper;

        public GetResourcesEventByGidQueryHandler(IRepository<ResourcesEvent> resourcesEventRepository, IMapper mapper)
        {
            _resourcesEventRepository = resourcesEventRepository;
            _mapper = mapper;
        }

        public async Task<ResourcesEventDTO> Handle(GetResourcesEventByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _resourcesEventRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(ResourcesEvent), request.GID.ToString()))
                : _mapper.Map<ResourcesEventDTO>(result);
        }
    }

}
