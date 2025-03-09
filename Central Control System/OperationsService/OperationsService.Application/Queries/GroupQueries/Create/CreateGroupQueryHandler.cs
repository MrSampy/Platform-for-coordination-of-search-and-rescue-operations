using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.GroupQueries.Create
{
    public class CreateGroupQueryHandler : IRequestHandler<CreateGroupQuery, GroupDTO>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<Group> _cacheService;
        private readonly IMapper _mapper;

        public CreateGroupQueryHandler(IRepository<Group> groupRepository, IRepository<Event> eventRepository, IUnitOfWork unitOfWork, ICacheService<Group> cacheService, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<GroupDTO> Handle(CreateGroupQuery request, CancellationToken cancellationToken)
        {
            var eventExists = await _eventRepository.GetByGidAsync(request.Group.EventGID, cancellationToken);
            if (eventExists == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.Group.EventGID.ToString()));
            }

            var group = _mapper.Map<Group>(request.Group);
            group.GID = Guid.NewGuid();
            group.CreatedAt = group.UpdatedAt = DateTime.Now;
            await _groupRepository.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<GroupDTO>(group);
        }
    }
}
