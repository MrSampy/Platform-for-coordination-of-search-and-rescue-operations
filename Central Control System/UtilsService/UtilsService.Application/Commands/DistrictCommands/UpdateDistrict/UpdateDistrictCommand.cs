using MediatR;
using UtilsService.Domain.Entities;
namespace UtilsService.Application.Commands.DistrictCommands.UpdateDistrict
{
    public class UpdateDistrictCommand : IRequest
    {
        public District District { get; set; }
    }
}
