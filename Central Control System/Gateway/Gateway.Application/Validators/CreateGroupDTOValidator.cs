using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;

namespace Gateway.Application.Validators
{
    public class CreateGroupDTOValidator : AbstractValidator<CreateGroupDTO>
    {
        public CreateGroupDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateGroupDTO.Name)));

            RuleFor(x => x.EventGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateGroupDTO.EventGID)));
        }
    }

}
