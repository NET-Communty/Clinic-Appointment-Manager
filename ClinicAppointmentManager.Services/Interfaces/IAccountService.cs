using ClinicAppointmentManager.Core.Dtos.Auth;

namespace ClinicAppointmentManager.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AuthResult> RegisterAsync(AuthDto dto);
        Task<AuthResult> LoginAsync(AuthDto dto);
        Task<AuthResult> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}