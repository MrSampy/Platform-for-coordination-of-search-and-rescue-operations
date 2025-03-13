using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteersDistrictsCommands.Delete
{
    public class DeleteVolunteersDistrictCommandHandler : IRequestHandler<DeleteVolunteersDistrictCommand>
    {
        private readonly IRepository<VolunteersDistricts> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersDistricts> _cacheService;

        public DeleteVolunteersDistrictCommandHandler(IRepository<VolunteersDistricts> repository, IUnitOfWork unitOfWork, ICacheService<VolunteersDistricts> cacheService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteVolunteersDistrictCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(VolunteersDistricts), request.GID.ToString()));
            }

            await _repository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            _cacheService.Reset();
        }
    }
}
