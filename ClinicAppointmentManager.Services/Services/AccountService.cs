using ClinicAppointmentManager.Core.Dtos.Auth;
using ClinicAppointmentManager.Core.Entities;
using ClinicAppointmentManager.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ClinicAppointmentManager.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<AuthResult> RegisterAsync(AuthDto dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return new AuthResult { Success = false, Errors = new[] { "Email in use" } };

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                EmailConfirmed = true, // email confirmation flow in prod
                RefreshTokens = []
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                return new AuthResult { Success = false, Errors = createResult.Errors.Select(e => e.Description) };

            // Issue tokens
            var jwt = await GenerateJwtTokenAsync(user);
            var refresh = GenerateRefreshToken(_configuration);

            user.RefreshTokens.Add(refresh); // => null pointer exception
            await _userManager.UpdateAsync(user);

            return new AuthResult
            {
                Success = true,
                Token = jwt.Token,
                ExpiresAt = jwt.Expires,
                RefreshToken = refresh.Token,
                RefreshTokenExpiresAt = refresh.ExpiresOn
            };
        }

        public async Task<AuthResult> LoginAsync(AuthDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return new AuthResult { Success = false, Errors = new[] { "Invalid credentials" } };

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return new AuthResult { Success = false, Errors = new[] { "Invalid credentials" } };

            var jwt = await GenerateJwtTokenAsync(user);


            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                // If user already has an active refresh token, return it
                var activeRefresh = user.RefreshTokens.First(t => t.IsActive);
                return new AuthResult
                {
                    Success = true,
                    Token = jwt.Token,
                    ExpiresAt = jwt.Expires,
                    RefreshToken = activeRefresh.Token,
                    RefreshTokenExpiresAt = activeRefresh.ExpiresOn
                };
            }


            var refresh = GenerateRefreshToken(_configuration);
            user.RefreshTokens.Add(refresh);
            await _userManager.UpdateAsync(user);
            return new AuthResult
            {
                Success = true,
                Token = jwt.Token,
                ExpiresAt = jwt.Expires,
                RefreshToken = refresh.Token,
                RefreshTokenExpiresAt = refresh.ExpiresOn
            };
        }

        public async Task<AuthResult> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
            {
                return new AuthResult { Success = false, Errors = new[] { "Invalid refresh token" } };
            }

            var refresh = user.RefreshTokens.Single(t => t.Token == token);

            if (!refresh.IsActive)
                return new AuthResult { Success = false, Errors = new[] { "Refresh token is not active" } };

            var newRefresh = GenerateRefreshToken(_configuration);

            refresh.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            var jwt = await GenerateJwtTokenAsync(user);

            return new AuthResult
            {
                Success = true,
                Token = jwt.Token,
                ExpiresAt = jwt.Expires,
                RefreshToken = newRefresh.Token,
                RefreshTokenExpiresAt = newRefresh.ExpiresOn
            };
        }


        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return false;
            var refresh = user.RefreshTokens.Single(t => t.Token == token);

            if (refresh == null || !refresh.IsActive)
                return false; // nothing to revoke

            refresh.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return true;
        }


        // Helper method to generate JWT token
        private (string Token, DateTime Expires) GenerateJwtTokenFromClaims(IEnumerable<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"] ?? "15");
            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expires);
        }
        private async Task<(string Token, DateTime Expires)> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.UserName ?? string.Empty)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            return GenerateJwtTokenFromClaims(claims);
        }
        private static RefreshToken GenerateRefreshToken(IConfiguration configuration)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);

            var ttlDays = int.Parse(configuration.GetSection("JwtSettings")["RefreshTokenTTLDays"] ?? "30");

            return new RefreshToken
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(ttlDays),
                CreatedOn = DateTime.UtcNow,
            };
        }

    }
}
