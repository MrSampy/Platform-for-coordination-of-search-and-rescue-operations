using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourceUnitQueries.GetResourceUnitByGid
{
    public class GetResourceUnitByGidQueryHandler : IRequestHandler<GetResourceUnitByGidQuery, ResourceMeasurementUnit>
    {
        private readonly IResourceMeasurementUnitRepository _repository;

        public GetResourceUnitByGidQueryHandler(IResourceMeasurementUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResourceMeasurementUnit> Handle(GetResourceUnitByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByGidAsync(request.GID, cancellationToken);

            return result ?? throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(ResourceMeasurementUnit), request.GID.ToString()));
        }
    }
}
