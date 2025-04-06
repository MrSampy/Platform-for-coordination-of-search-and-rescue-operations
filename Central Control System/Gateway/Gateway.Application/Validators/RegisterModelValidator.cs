using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;

namespace Gateway.Application.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(RegisterModel.Username)));

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(RegisterModel.Email)))
                .EmailAddress()
                .WithMessage(string.Format(SharedConstants.InvalidFieldFormatException, nameof(RegisterModel.Email)));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(RegisterModel.Password)));
        }
    }
}
