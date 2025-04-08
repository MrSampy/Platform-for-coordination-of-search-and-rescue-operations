using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Volunteers.Create;

namespace Gateway.Application.Validators
{
    public class CreateVolunteersDistrictsDTOValidator : AbstractValidator<CreateVolunteersDistrictsDTO>
    {
        public CreateVolunteersDistrictsDTOValidator()
        {
            RuleFor(x => x.VolunteerGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateVolunteersDistrictsDTO.VolunteerGID)));

            RuleFor(x => x.DistrictGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(CreateVolunteersDistrictsDTO.DistrictGID)));
        }
    }

}
