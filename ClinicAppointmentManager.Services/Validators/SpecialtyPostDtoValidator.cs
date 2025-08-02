using FluentValidation;
using ClinicAppointmentManager.Core.Dtos;

namespace ClinicAppointmentManager.Services.Validators
{
    public class SpecialtyPostDtoValidator : AbstractValidator<SpecialtyPostDto>
    {
        public SpecialtyPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
