using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.DistrictQueries.GetDistrictByGid
{
    public class GetDistrictByGidQueryHandler : IRequestHandler<GetDistrictByGidQuery, District>
    {
        private readonly IDistrictRepository _districtRepository;

        public GetDistrictByGidQueryHandler(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<District> Handle(GetDistrictByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _districtRepository.GetByGidAsync(request.GID, cancellationToken);

            return result ?? throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(District), request.GID.ToString()));
            ;
        }
    }

}
