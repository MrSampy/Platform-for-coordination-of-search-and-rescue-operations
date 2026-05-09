using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;

namespace Gateway.Application.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(LoginModel.Username)));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(LoginModel.Password)));
        }
    }
}
