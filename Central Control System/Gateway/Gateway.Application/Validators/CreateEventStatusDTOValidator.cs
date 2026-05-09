using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;

namespace Gateway.Application.Validators
{
    public class CreateEventStatusDTOValidator : AbstractValidator<CreateEventStatusDTO>
    {
        public CreateEventStatusDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventStatusDTO.Name)));
        }
    }

}
