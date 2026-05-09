using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;

namespace Gateway.Application.Validators
{
    public class CreateEventDTOValidator : AbstractValidator<CreateEventDTO>
    {
        public CreateEventDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventDTO.Name)));

            RuleFor(x => x.EventTypeGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventDTO.EventTypeGID)));

            RuleFor(x => x.DistrictGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventDTO.DistrictGID)));

            RuleFor(x => x.CoordinatorGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventDTO.CoordinatorGID)));

            RuleFor(x => x.DispatcherGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventDTO.DispatcherGID)));

            RuleFor(x => x.EventStatusGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateEventDTO.EventStatusGID)));
        }
    }

}
