using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Validators
{
    public class UpdateOperationWorkerDTOValidator : AbstractValidator<UpdateOperationWorkerDTO>
    {
        public UpdateOperationWorkerDTOValidator()
        {
            RuleFor(x => x.GID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UpdateOperationWorkerDTO.GID)));

            RuleFor(x => x.Name)
                      .NotEmpty()
                      .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationWorkerDTO.Name)));

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationWorkerDTO.Surname)));

            RuleFor(x => x.SecondName)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationWorkerDTO.SecondName)));

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(string.Format(SharedConstants.InvalidFieldFormatException, nameof(CreateOperationWorkerDTO.Email)));

            RuleFor(x => x.IdentificationCode)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationWorkerDTO.IdentificationCode)));

            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Today)
                .WithMessage(SharedConstants.InvalidBirthDateException);

            RuleFor(x => x.UserGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationWorkerDTO.UserGID)));
        }
    }

}
