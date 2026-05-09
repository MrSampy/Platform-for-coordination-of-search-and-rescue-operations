using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Application.Validators
{
    public class CreateMeasurementUnitDTOValidator : AbstractValidator<CreateMeasurementUnitDTO>
    {
        public CreateMeasurementUnitDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateMeasurementUnitDTO.Name)));
        }
    }
}
