using FluentValidation;
using ClinicAppointmentManager.Core.Dtos.Specialty;

namespace ClinicAppointmentManager.Services.Validators
{
    public class SpecialtyPutDtoValidator : AbstractValidator<SpecialtyPutDto>
    {
        public SpecialtyPutDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
