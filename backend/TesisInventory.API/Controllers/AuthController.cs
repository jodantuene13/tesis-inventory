using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Interfaces;
using TesisInventory.API.DTOs;

namespace TesisInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                // 1. Validar Token con Google
                var googleUser = await _authService.ValidateGoogleTokenAsync(request.Token);
                if (googleUser == null)
                    return Unauthorized(new { message = "Token de Google inválido." });

                // 2. Validar Dominio
                if (!_authService.ValidateDomain(googleUser.Email))
                    return Unauthorized(new { message = "El dominio del correo no está autorizado (@ucc.edu.ar)." });

                // 3. Buscar Usuario en BD
                var dbUser = await _userRepository.GetByEmailAsync(googleUser.Email);
                if (dbUser == null)
                    return Unauthorized(new { message = "Usuario no registrado en el sistema." });

                // 4. Validar Estado
                if (!dbUser.Estado)
                    return Unauthorized(new { message = "Usuario inactivo. Contacte al administrador." });

                // 5. Vincular GoogleId si es necesario
                if (string.IsNullOrEmpty(dbUser.GoogleId))
                {
                    dbUser.GoogleId = googleUser.GoogleId;
                    await _userRepository.UpdateAsync(dbUser);
                }

                // 6. Generar JWT
                var jwt = await _authService.GenerateJwtTokenAsync(dbUser);

                return Ok(new { token = jwt, user = new { dbUser.NombreUsuario, dbUser.Email, dbUser.IdRol, nombreRol = dbUser.Rol?.NombreRol } });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno: " + ex.Message, details = ex.ToString() });
            }
        }
    }
}
