using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.DistrictCommands.UpdateDistrict
{
    public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<District> _cacheService;

        public UpdateDistrictCommandHandler(IDistrictRepository districtRepository, IUnitOfWork unitOfWork, ICacheService<District> cacheService)
        {
            _districtRepository = districtRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
        {
            var entity = await _districtRepository.GetByGidAsync(request.District.GID, cancellationToken);

            if (entity == null)
            {
                throw new UtilsServiceException(string.Format(Constants.NotFoundEntityException, nameof(District), request.District.GID.ToString()));
            }

            await _districtRepository.UpdateAsync(request.District);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
