using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TesisInventory.API.Filters
{
    /// <summary>
    /// Verifica que el JWT del usuario contenga el claim "permiso" con el valor requerido.
    /// Aplicar sobre métodos o controladores que requieran un permiso específico.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermisoAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _permiso;

        public RequirePermisoAttribute(string permiso)
        {
            _permiso = permiso;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "No autenticado." });
                return;
            }

            var tienePermiso = user.Claims
                .Where(c => c.Type == "permiso")
                .Any(c => c.Value == _permiso);

            if (!tienePermiso)
            {
                context.Result = new ObjectResult(new
                {
                    message = $"No tenés el permiso '{_permiso}' requerido para esta operación."
                })
                { StatusCode = 403 };
                return;
            }

            await next();
        }
    }
}
