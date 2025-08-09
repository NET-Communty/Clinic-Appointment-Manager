using ClinicAppointmentManager.Core.Dtos.Auth;
using ClinicAppointmentManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicAppointmentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;

        public AccountController(IAccountService account)
        {
            _account = account;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDto dto)
        {
            var result = await _account.RegisterAsync(dto);
            if (!result.Success) return BadRequest(result);

            SetRfreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto dto)
        {
            var result = await _account.LoginAsync(dto);
            if (!result.Success) 
                return BadRequest(result);


            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRfreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Ok(result);
        }

        private void SetRfreshTokenInCookie(string refreshToken, DateTime? refreshTokenExpiresAt)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiresAt?.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _account.RefreshTokenAsync(refreshToken);
            if (!result.Success) return BadRequest(result);

            // Set new refresh token in cookie
            SetRfreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
            return Ok(result);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeTokenDto? dto)
        {
            var refreshToken = dto?.RefreshToken ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { message = "Refresh token is required" });

            var success = await _account.RevokeTokenAsync(refreshToken);
            if (!success) return NotFound(new { message = "Token not found or already revoked" });
            return Ok(new { message = "Token revoked" });
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Me()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
            var username = User.FindFirstValue("username") ?? User.Identity?.Name;
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            return Ok(new { id, email, username, roles });
        }
    }
}
