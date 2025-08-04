using FluentValidation;
using ClinicAppointmentManager.Core.Dtos.Clinic;

namespace ClinicAppointmentManager.Services.Validators
{
    public class ClinicPutDtoValidator : AbstractValidator<ClinicPutDto>
    {
        public ClinicPutDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(200)
                .Matches(@"^[a-zA-Z0-9\s,.-]+$")
                .WithMessage("Address contains invalid characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20)
                .Matches(@"^\+?[0-9\s\-()]+$")
                .WithMessage("Phone number format is invalid.");
        }
    }
}
