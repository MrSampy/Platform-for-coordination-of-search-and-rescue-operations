using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;

namespace Gateway.Application.Validators
{
    public class CreateOperationTaskDTOValidator : AbstractValidator<CreateOperationTaskDTO>
    {
        public CreateOperationTaskDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationTaskDTO.Name)));

            RuleFor(x => x.TaskDescription)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationTaskDTO.TaskDescription)));

            RuleFor(x => x.GroupGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationTaskDTO.GroupGID)));

            RuleFor(x => x.TaskStatusGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateOperationTaskDTO.TaskStatusGID)));
        }
    }

}
