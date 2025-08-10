using Microsoft.AspNetCore.Identity;
using ClinicAppointmentManager.Core.Enums;

namespace ClinicAppointmentManager.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole UserRole { get; set; }

        // Navigation
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
