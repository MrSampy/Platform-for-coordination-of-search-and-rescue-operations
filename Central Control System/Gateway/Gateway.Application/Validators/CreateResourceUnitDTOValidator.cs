using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils.Create;

namespace Gateway.Application.Validators
{
    public class CreateResourceUnitDTOValidator : AbstractValidator<CreateResourceUnitDTO>
    {
        public CreateResourceUnitDTOValidator()
        {
            RuleFor(x => x.UnitGID)
                .NotEqual(Guid.Empty)
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateResourceUnitDTO.UnitGID)));

            RuleFor(x => x.ResourceGID)
                .NotEqual(Guid.Empty)
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateResourceUnitDTO.ResourceGID)));
        }
    }
}
