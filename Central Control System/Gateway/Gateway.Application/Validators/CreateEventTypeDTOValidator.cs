using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;

namespace Gateway.Application.Validators
{
    public class CreateEventTypeDTOValidator : AbstractValidator<CreateEventTypeDTO>
    {
        public CreateEventTypeDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventTypeDTO.Name)));
        }
    }

}
