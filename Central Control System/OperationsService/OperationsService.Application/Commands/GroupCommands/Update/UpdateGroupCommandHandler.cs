using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Commands.GroupCommands.Update
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Group> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public UpdateGroupCommandHandler(IRepository<Group> groupRepository, IRepository<Event> eventRepository, IUnitOfWork unitOfWork,
            ICacheService<Group> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _groupRepository = groupRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _groupRepository.GetByGidAsync(request.Group.GID, cancellationToken);

            if (entity == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Group), request.Group.GID.ToString()));
            }

            var eventExists = await _eventRepository.GetByGidAsync(request.Group.EventGID, cancellationToken);
            if (eventExists == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.Group.EventGID.ToString()));
            }

            if (request.Group.LeaderGID != null)
            {
                var volunteer = await _apiBuilder.GetRequest<VolunteerDTO>($"volunteers/api/volunteer/{request.Group.LeaderGID}", Constants.VolunteerService, cancellationToken, request.Token);
                if (volunteer == null)
                {
                    throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "Volunteer", request.Group.LeaderGID.ToString()));
                }
            }

            var mappedEntity = _mapper.Map<Group>(request.Group);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _groupRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
