using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Validators
{
    public class UpdateGroupDTOValidator : AbstractValidator<UpdateGroupDTO>
    {
        public UpdateGroupDTOValidator()
        {
            RuleFor(x => x.GID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UpdateGroupDTO.GID)));

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateGroupDTO.Name)));

            RuleFor(x => x.EventGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateGroupDTO.EventGID)));
        }
    }

}
