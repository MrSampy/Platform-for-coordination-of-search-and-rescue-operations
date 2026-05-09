using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;

namespace Gateway.Application.Validators
{
    public class RoleDTOValidator : AbstractValidator<RoleDTO>
    {
        public RoleDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(RoleDTO.Name)));
        }
    }
}
