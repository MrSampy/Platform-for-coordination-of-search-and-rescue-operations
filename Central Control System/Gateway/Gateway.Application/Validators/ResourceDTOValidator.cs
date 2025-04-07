using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Utils;

namespace Gateway.Application.Validators
{
    public class ResourceDTOValidator : AbstractValidator<ResourceDTO>
    {
        public ResourceDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(ResourceDTO.Name)));
        }
    }
