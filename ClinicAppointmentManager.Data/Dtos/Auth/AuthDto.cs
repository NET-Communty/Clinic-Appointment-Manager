namespace ClinicAppointmentManager.Core.Dtos.Auth
{
    public class AuthDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }


    public class RefreshRequestDto 
    {
        public string RefreshToken { get; set; } = null!;
    }
}
