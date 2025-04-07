using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils;

namespace Gateway.Application.Validators
{
    public class ResourceMeasurementUnitDTOValidator : AbstractValidator<ResourceMeasurementUnitDTO>
    {
        public ResourceMeasurementUnitDTOValidator()
        {
            RuleFor(x => x.UnitGID)
                .NotEqual(Guid.Empty)
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(ResourceMeasurementUnitDTO.UnitGID)));

            RuleFor(x => x.ResourceGID)
                .NotEqual(Guid.Empty)
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(ResourceMeasurementUnitDTO.ResourceGID)));
        }
    }
}
