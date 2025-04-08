using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Volunteers.Create;

namespace Gateway.Application.Validators
{
    public class CreateVolunteersGroupsDTOValidator : AbstractValidator<CreateVolunteersGroupsDTO>
    {
        public CreateVolunteersGroupsDTOValidator()
        {
            RuleFor(x => x.VolunteerGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateVolunteersGroupsDTO.VolunteerGID)));

            RuleFor(x => x.GroupGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateVolunteersGroupsDTO.GroupGID)));
        }
    }

}
