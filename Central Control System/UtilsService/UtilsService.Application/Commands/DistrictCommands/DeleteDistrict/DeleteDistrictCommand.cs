using MediatR;

namespace UtilsService.Application.Commands.DistrictCommands.DeleteDistrict
{
    public class DeleteDistrictCommand : IRequest
    {
        public required Guid GID { get; set; }
    }
}
