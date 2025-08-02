
using ClinicAppointmentManager.Core.Dtos;
using FluentValidation;

namespace ClinicAppointmentManager.Services.Validators
{
    public class DoctorPutDtoValidator : AbstractValidator<DoctorPutDto>
    {
        public DoctorPutDtoValidator()
        {
            RuleFor(d => d.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(d => d.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(d => d.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.")
                .MaximumLength(20).WithMessage("License number cannot exceed 20 characters.");

            RuleFor(d => d.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(d => d.ClinicId)
                .GreaterThan(0).WithMessage("Clinic ID must be greater than 0.");

            RuleFor(d => d.SpecialtyId)
                .GreaterThan(0).WithMessage("Specialty ID must be greater than 0.");
        }
    }
}
