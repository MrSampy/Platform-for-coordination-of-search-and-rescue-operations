using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Queries.DistrictQueries.CreateDistrict
{
    public class CreateDistrictQueryHandler : IRequestHandler<CreateDistrictQuery, District>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly ICacheService<District> _cacheService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDistrictQueryHandler(IDistrictRepository districtRepository, IUnitOfWork unitOfWork, ICacheService<District> cacheService)
        {
            _districtRepository = districtRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<District> Handle(CreateDistrictQuery request, CancellationToken cancellationToken)
        {
            var district = request.District;

            district.GID = Guid.NewGuid();

            await _districtRepository.AddAsync(district);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return district;
        }
    }

}
