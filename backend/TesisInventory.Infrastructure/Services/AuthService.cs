using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Usuario> ValidateGoogleTokenAsync(string googleToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    // Audience = new List<string>() { _configuration["Google:ClientId"] } // Configurar ClientId luego
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);

                return new Usuario
                {
                    GoogleId = payload.Subject,
                    Email = payload.Email,
                    NombreUsuario = payload.Name,
                    // Otros campos se llenarán al consultar/crear en BD
                };
            }
            catch (InvalidJwtException)
            {
                return null;
            }
        }

        public bool ValidateDomain(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return email.EndsWith("@ucc.edu.ar", StringComparison.OrdinalIgnoreCase);
        }

        public Task<string> GenerateJwtTokenAsync(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? "User"),
                    new Claim("sede_id", usuario.IdSede.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1), // Expiración de 1 día
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
