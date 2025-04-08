using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Operations.Update;

namespace Gateway.Application.Validators
{
    public class UpdateResourcesEventDTOValidator : AbstractValidator<UpdateResourcesEventDTO>
    {
        public UpdateResourcesEventDTOValidator()
        {
            RuleFor(x => x.GID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(UpdateResourcesEventDTO.GID)));


            RuleFor(x => x.ResourceGID)
               .NotEmpty()
               .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateResourcesEventDTO.ResourceGID)));

            RuleFor(x => x.EventGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateResourcesEventDTO.EventGID)));

            RuleFor(x => x.RequiredQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage(string.Format(SharedConstants.InvalidFieldFormatException, nameof(CreateResourcesEventDTO.RequiredQuantity)));

            RuleFor(x => x.AvailableQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage(string.Format(SharedConstants.InvalidFieldFormatException, nameof(CreateResourcesEventDTO.AvailableQuantity)));

        }
    }

}
