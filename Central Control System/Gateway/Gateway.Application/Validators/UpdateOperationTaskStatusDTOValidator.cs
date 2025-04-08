using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Validators
{
    public class UpdateOperationTaskStatusDTOValidator : AbstractValidator<UpdateOperationTaskStatusDTO>
    {
        public UpdateOperationTaskStatusDTOValidator()
        {
            RuleFor(x => x.GID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UpdateOperationTaskStatusDTO.GID)));
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationTaskStatusDTO.Name)));
        }
    }

}
