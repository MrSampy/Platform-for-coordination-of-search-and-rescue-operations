using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Validators
{
    public class UpdateEventTypeDTOValidator : AbstractValidator<UpdateEventTypeDTO>
    {
        public UpdateEventTypeDTOValidator()
        {
            RuleFor(x => x.GID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UpdateEventTypeDTO.GID)));

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventTypeDTO.Name)));
        }
    }

}
