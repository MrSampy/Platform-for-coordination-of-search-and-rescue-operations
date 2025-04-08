using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.DTOs.Volunteers.Create;

namespace Gateway.Application.Validators
{
    public class CreateVolunteerDTOValidator : AbstractValidator<CreateVolunteerDTO>
    {
        public CreateVolunteerDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteerDTO.Name)));

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteerDTO.Surname)));

            RuleFor(x => x.SecondName)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteerDTO.SecondName)));

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(string.Format(SharedConstants.InvalidFieldFormatException, nameof(VolunteerDTO.Email)));

            RuleFor(x => x.MobilePhone)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteerDTO.MobilePhone)))
                .Matches(SharedConstants.MobilePhomeRegexp)
                .WithMessage(string.Format(SharedConstants.InvalidFieldFormatException, nameof(VolunteerDTO.MobilePhone)));

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Today)
                .WithMessage(SharedConstants.InvalidBirthDateException);

            RuleFor(x => x.UserGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteerDTO.UserGID)));
        }
    }
}
