using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;

namespace Gateway.Application.Validators
{
    public class CreateOperationTaskStatusDTOValidator : AbstractValidator<CreateOperationTaskStatusDTO>
    {
        public CreateOperationTaskStatusDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationTaskStatusDTO.Name)));
        }
    }

}
