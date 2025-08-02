using FluentValidation;
using ClinicAppointmentManager.Core.Dtos;

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
