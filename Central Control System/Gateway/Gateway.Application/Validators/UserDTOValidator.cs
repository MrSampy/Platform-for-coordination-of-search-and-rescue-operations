using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;

namespace Gateway.Application.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UserDTO.Name)));

            RuleFor(x => x.Roles)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UserDTO.Roles)));

            RuleForEach(x => x.Roles)
                .SetValidator(new RoleDTOValidator());
        }
    }
}
