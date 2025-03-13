using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteerCommands.Delete
{
    public class DeleteVolunteerCommandHandler : IRequestHandler<DeleteVolunteerCommand>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Volunteer> _cacheService;

        public DeleteVolunteerCommandHandler(IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<Volunteer> cacheService)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(DeleteVolunteerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _volunteerRepository.GetByGidAsync(request.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.GID.ToString()));
            }

            await _volunteerRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
