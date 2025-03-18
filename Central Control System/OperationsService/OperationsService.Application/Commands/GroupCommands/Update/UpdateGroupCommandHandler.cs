using AutoMapper;
using MediatR;
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

        public UpdateGroupCommandHandler(IRepository<Group> groupRepository, IRepository<Event> eventRepository, IUnitOfWork unitOfWork, ICacheService<Group> cacheService, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
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

            var mappedEntity = _mapper.Map<Group>(request.Group);

            mappedEntity.CreatedAt = entity.CreatedAt;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

            await _groupRepository.UpdateAsync(mappedEntity);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();
        }
    }
}
