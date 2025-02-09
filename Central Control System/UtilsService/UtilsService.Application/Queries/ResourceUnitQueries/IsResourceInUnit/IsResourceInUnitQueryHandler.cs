using MediatR;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourceUnitQueries.IsResourceInUnit
{
    public class IsResourceInUnitQueryHandler : IRequestHandler<IsResourceInUnitQuery, bool>
    {
        private readonly IResourceMeasurementUnitRepository _repository;

        public IsResourceInUnitQueryHandler(IResourceMeasurementUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsResourceInUnitQuery request, CancellationToken cancellationToken)
        {
            return await _repository.IsResourceInMeasurementUnit(request.ResourceUnit);
        }
    }
}
