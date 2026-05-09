using AutoMapper;
using MediatR;
using VolunteerService.Application.DTOs;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Queries.VolunteerEventsQuery.Create
{
    public class CreateVolunteerEventsQueryHandler : IRequestHandler<CreateVolunteerEventsQuery, VolunteersEventsDTO>
    {
        private readonly IRepository<VolunteersEvents> _volunteerEventRepository;
        private readonly IRepository<Volunteer> _volunteerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService<VolunteersEvents> _cacheService;
        private readonly IMapper _mapper;
        private readonly IApiBuilder _apiBuilder;

        public CreateVolunteerEventsQueryHandler(IRepository<VolunteersEvents> volunteerEventRepository, IRepository<Volunteer> volunteerRepository,
            IUnitOfWork unitOfWork, ICacheService<VolunteersEvents> cacheService, IMapper mapper, IApiBuilder apiBuilder)
        {
            _volunteerEventRepository = volunteerEventRepository;
            _volunteerRepository = volunteerRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
            _apiBuilder = apiBuilder;
        }

        public async Task<VolunteersEventsDTO> Handle(CreateVolunteerEventsQuery request, CancellationToken cancellationToken)
        {
            var volunteer = await _volunteerRepository.GetByGidAsync(request.VolunteerEventsDTO.VolunteerGID, cancellationToken);
            if (volunteer == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, nameof(Volunteer), request.VolunteerEventsDTO.VolunteerGID.ToString()));
            }

            var eventDTO = await _apiBuilder.GetRequest<EventDTO>($"operations/api/Event/{request.VolunteerEventsDTO.EventGID}", Constants.OperatrionsService, cancellationToken, request.Token);
            if (eventDTO == null)
            {
                throw new VolunteerServiceException(string.Format(Constants.NotFoundEntityException, "Event", request.VolunteerEventsDTO.EventGID.ToString()));
            }

            var volunteerEvent = _mapper.Map<VolunteersEvents>(request.VolunteerEventsDTO);
            volunteerEvent.GID = Guid.NewGuid();
            volunteerEvent.CreatedAt = volunteerEvent.UpdatedAt = DateTime.UtcNow;

            await _volunteerEventRepository.AddAsync(volunteerEvent);
            await _unitOfWork.SaveChangesAsync();

            _cacheService.Reset();

            return _mapper.Map<VolunteersEventsDTO>(volunteerEvent);
        }
    }
}
