using System.Text.Json.Serialization;

namespace ClinicAppointmentManager.Core.Dtos.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }

    }
}
