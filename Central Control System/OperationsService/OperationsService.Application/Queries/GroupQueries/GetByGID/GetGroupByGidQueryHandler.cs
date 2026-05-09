using AutoMapper;
using MediatR;
using OperationsService.Application.DTOs;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Exceptions;
using OperationsService.Domain.Interfaces;

namespace OperationsService.Application.Queries.GroupQueries.GetByGID
{
    public class GetGroupByGidQueryHandler : IRequestHandler<GetGroupByGidQuery, GroupDTO>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupByGidQueryHandler(IRepository<Group> groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<GroupDTO> Handle(GetGroupByGidQuery request, CancellationToken cancellationToken)
        {
            var result = await _groupRepository.GetByGidAsync(request.GID, cancellationToken);

            return result == null
                ? throw new OperationsServiceException(string.Format(Constants.NotFoundEntityException, nameof(Group), request.GID.ToString()))
                : _mapper.Map<GroupDTO>(result);
        }
    }

}
