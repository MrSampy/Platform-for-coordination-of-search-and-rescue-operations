using MediatR;
using UtilsService.Application.DTOs;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.ResourceUnitQueries.IsResourceInUnit
{
    public class IsResourceInUnitQueryHandler : IRequestHandler<IsResourceInUnitQuery, IsExistModel>
    {
        private readonly IResourceMeasurementUnitRepository _repository;

        public IsResourceInUnitQueryHandler(IResourceMeasurementUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<IsExistModel> Handle(IsResourceInUnitQuery request, CancellationToken cancellationToken)
        {
            return new IsExistModel() { IsExist = await _repository.IsResourceInMeasurementUnit(request.ResourceUnit) };
        }
    }
}
