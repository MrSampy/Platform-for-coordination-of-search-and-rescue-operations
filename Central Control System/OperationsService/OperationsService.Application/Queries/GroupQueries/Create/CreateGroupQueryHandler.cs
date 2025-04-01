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
        private readonly IApiBuilder _apiBuilder;

        public CreateGroupQueryHandler(IRepository<Group> groupRepository, IRepository<Event> eventRepository, IUnitOfWork unitOfWork,
            ICacheService<Group> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _groupRepository = groupRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task<GroupDTO> Handle(CreateGroupQuery request, CancellationToken cancellationToken)
        {
            var eventExists = await _eventRepository.GetByGidAsync(request.Group.EventGID, cancellationToken);
            if (eventExists == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Event), request.Group.EventGID.ToString()));
            }
            var volunteer = await _apiBuilder.GetRequest<VolunteerDTO>($"volunteers/api/volunteer/{request.Group.LeaderGID}", Constants.VolunteerService, cancellationToken, request.Token);
            if (volunteer == null)
            {
                throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, "Volunteer", request.Group.LeaderGID.ToString()));
            }
            var group = _mapper.Map<Group>(request.Group);
            group.GID = Guid.NewGuid();
            group.CreatedAt = group.UpdatedAt = DateTime.UtcNow;
            await _groupRepository.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<GroupDTO>(group);
        }
    }
}
