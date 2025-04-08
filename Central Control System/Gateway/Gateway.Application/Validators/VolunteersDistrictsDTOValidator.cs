using FluentValidation;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Volunteers;

namespace Gateway.Application.Validators
{
    public class VolunteersDistrictsDTOValidator : AbstractValidator<VolunteersDistrictsDTO>
    {
        public VolunteersDistrictsDTOValidator()
        {
            RuleFor(x => x.VolunteerGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteersDistrictsDTO.VolunteerGID)));

            RuleFor(x => x.DistrictGID)
                .NotEmpty()
                .WithMessage(string.Format(SharedConstants.FieldIsRequierdException, nameof(VolunteersDistrictsDTO.DistrictGID)));
        }
    }

}
