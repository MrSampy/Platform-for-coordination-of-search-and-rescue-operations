using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Application.Validators
{
    public class CreateDistrictDTOValidator : AbstractValidator<CreateDistrictDTO>
    {
        public CreateDistrictDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateDistrictDTO.Name)));
        }
    }
}
