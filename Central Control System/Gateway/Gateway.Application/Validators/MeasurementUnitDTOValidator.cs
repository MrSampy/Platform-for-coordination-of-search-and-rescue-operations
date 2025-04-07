using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils;

namespace Gateway.Application.Validators
{
    public class MeasurementUnitDTOValidator : AbstractValidator<MeasurementUnitDTO>
    {
        public MeasurementUnitDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(MeasurementUnitDTO.Name)));
        }
    }
}
