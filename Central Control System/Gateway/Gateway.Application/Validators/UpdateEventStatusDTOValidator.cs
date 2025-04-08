using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Validators
{
    public class UpdateEventStatusDTOValidator : AbstractValidator<UpdateEventStatusDTO>
    {
        public UpdateEventStatusDTOValidator()
        {
            RuleFor(x => x.GID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UpdateEventStatusDTO.GID)));

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventStatusDTO.Name)));
        }
    }

}
