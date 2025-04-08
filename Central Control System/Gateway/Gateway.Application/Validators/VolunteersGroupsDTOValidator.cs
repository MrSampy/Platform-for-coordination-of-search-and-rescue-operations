using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Volunteers;

namespace Gateway.Application.Validators
{
    public class VolunteersGroupsDTOValidator : AbstractValidator<VolunteersGroupsDTO>
    {
        public VolunteersGroupsDTOValidator()
        {
            RuleFor(x => x.VolunteerGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteersGroupsDTO.VolunteerGID)));

            RuleFor(x => x.GroupGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteersGroupsDTO.GroupGID)));
        }
    }

}
