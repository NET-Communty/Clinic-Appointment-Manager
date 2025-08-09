using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentManager.Core.Entities
{
    [Owned] //
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }

        public bool IsActive => RevokedOn == null && DateTime.UtcNow < ExpiresOn;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    }
}
