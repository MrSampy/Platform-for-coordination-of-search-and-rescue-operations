using MediatR;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Commands.DistrictCommands.DeleteDistrict
{
    public class DeleteDistrictCommandHandler : IRequestHandler<DeleteDistrictCommand>
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<District> _cacheService;

        public DeleteDistrictCommandHandler(IDistrictRepository districtRepository, IUnitOfWork unitOfWork, ICacheService<District> cacheService)
        {
            _districtRepository = districtRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteDistrictCommand request, CancellationToken cancellationToken)
        {
            var entity = await _districtRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new CustomException(string.Format(Constants.NotFoundEntityException, nameof(District), request.GID.ToString()));
            }

            await _districtRepository.DeleteAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
