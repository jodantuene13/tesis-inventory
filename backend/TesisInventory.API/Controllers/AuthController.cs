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
        private readonly IRolesService _rolesService;

        public AuthController(IAuthService authService, IUserRepository userRepository, IRolesService rolesService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _rolesService = rolesService;
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

                return await GenerateUserResponse(dbUser);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno: " + ex.Message, details = ex.ToString() });
            }
        }

        [HttpPost("test-login")]
        public async Task<IActionResult> TestLogin([FromBody] EmailLoginRequest request)
        {
            try
            {
                // 1. Buscar Usuario en BD
                var dbUser = await _userRepository.GetByEmailAsync(request.Email);
                if (dbUser == null)
                    return Unauthorized(new { message = "Usuario no registrado en el sistema." });

                // 2. Validar Estado
                if (!dbUser.Estado)
                    return Unauthorized(new { message = "Usuario inactivo. Contacte al administrador." });

                return await GenerateUserResponse(dbUser);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error interno: " + ex.Message, details = ex.ToString() });
            }
        }

        private async Task<IActionResult> GenerateUserResponse(TesisInventory.Domain.Entities.Usuario dbUser)
        {
            // 1. Obtener permisos del rol para incluirlos en el JWT
            var roleConfig = await _rolesService.GetRoleByIdAsync(dbUser.IdRol);
            var todosLosPermisos = await _rolesService.GetAllPermisosAsync();

            var permisosIds = roleConfig?.PermisosIds ?? new System.Collections.Generic.List<int>();
            var permisosNombres = todosLosPermisos
                .Where(p => permisosIds.Contains(p.IdPermiso))
                .Select(p => p.Nombre)
                .ToList();

            // 2. Configuración de sedes viene del usuario, no del rol
            var sedesPermitidas = dbUser.UsuariosSedes?.Select(us => us.IdSede).ToList()
                                  ?? new System.Collections.Generic.List<int>();

            // 3. Generar JWT
            var jwt = await _authService.GenerateJwtTokenAsync(dbUser, permisosNombres);

            return Ok(new {
                token = jwt,
                user = new {
                    dbUser.IdUsuario,
                    dbUser.NombreUsuario,
                    dbUser.Email,
                    dbUser.IdRol,
                    dbUser.IdSede,
                    nombreRol = dbUser.Rol?.NombreRol,
                    nombreSede = dbUser.Sede?.NombreSede,
                    todasLasSedes = dbUser.TodasLasSedes,
                    limitarOperacionSedePrimaria = dbUser.LimitarOperacionSedePrimaria,
                    permisos = permisosNombres,
                    sedesPermitidas = sedesPermitidas
                }
            });
        }
    }
}
