using AutoMapper;
using MediatR;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Commands.VolunteerCommands.Update
{
    public class UpdateVolunteerCommandHandler : IRequestHandler<UpdateVolunteerCommand>
    {
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Volunteer> _cacheService;
        private readonly IMapper _mapper;

        public UpdateVolunteerCommandHandler(IRepository<Volunteer> volunteerRepository, IUnitOfWork unitOfWork, ICacheService<Volunteer> cacheService, IMapper mapper)
        {
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task Handle(UpdateVolunteerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _volunteerRepository.GetByGidAsync(request.VolunteerDTO.GID, cancellationToken);

            if (entity == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.VolunteerDTO.GID.ToString()));
            }

            var mappedEntity = _mapper.Map<Volunteer>(request.VolunteerDTO);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.Now;

            await _volunteerRepository.UpdateAsync(mappedEntity);

            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }

}
