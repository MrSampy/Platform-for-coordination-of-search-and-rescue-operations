using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Application.Validators
{
    public class CreateResourceDTOValidator : AbstractValidator<CreateResourceDTO>
    {
        public CreateResourceDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateResourceDTO.Name)));
        }
    }
}
