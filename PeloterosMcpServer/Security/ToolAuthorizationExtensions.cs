using ModelContextProtocol;
using System.Security.Claims;

namespace PeloterosMcpServer.Security
{
    public static class ToolAuthorizationExtensions
    {
        /// <summary>
        /// Verifica que el usuario autenticado tenga al menos uno de los roles indicados.
        /// Si no cumple, corta la ejecución con un McpException (mensaje visible al cliente).
        /// Si cumple, devuelve el ClaimsPrincipal para que la tool lo siga usando (ej. el nombre).
        /// </summary>
        public static ClaimsPrincipal RequireRole(
            this IHttpContextAccessor httpContextAccessor,
            params string[] rolesPermitidos)
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user is null || user.Identity?.IsAuthenticated != true)
                throw new McpException("No se pudo identificar al usuario.");

            if (!rolesPermitidos.Any(user.IsInRole))
            {
                throw new McpException(
                    $"El usuario '{user.Identity!.Name}' no tiene permiso para esta acción. " +
                    $"Se requiere alguno de estos roles: {string.Join(", ", rolesPermitidos)}.");
            }

            return user;
        }

    }
}
