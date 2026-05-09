using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils;

namespace Gateway.Application.Validators
{
    public class DistrictDTOValidator : AbstractValidator<DistrictDTO>
    {
        public DistrictDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(DistrictDTO.Name)));
        }
    }
}
