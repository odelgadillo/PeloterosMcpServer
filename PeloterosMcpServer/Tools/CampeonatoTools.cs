using Microsoft.EntityFrameworkCore;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.Data.Entities;
using PeloterosMcpServer.DTOs;
using PeloterosMcpServer.Security;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class CampeonatoTools
    {
        [McpServerTool, Description("""Lista los campeonatos registrados, opcionalmente filtrando solo los activos. Usar para ubicar el CampeonatoId antes de consultar equipos, partidos u otra información específica de un campeonato.""")]
        public static async Task<List<CampeonatoDto>> ListarCampeonatos(
        PeloterosDbContext db,
        [Description("Si es true, devuelve solo el/los campeonato(s) activos.")] bool soloActivos = false,
        [Description("Cantidad máxima de resultados.")] int limite = 20)
        {
            limite = Math.Clamp(limite, 1, 100);

            var query = db.Campeonatos
                .AsNoTracking()
                .Include(c => c.CampeonatoEstado)
                .AsQueryable();

            if (soloActivos)
            {
                query = query.Where(c => c.CampeonatoEstadoId == "A");
            }

            return await query
                .OrderByDescending(c => c.FechaInicio)
                .Take(limite)
                .Select(c => new CampeonatoDto
                {
                    CampeonatoId = c.CampeonatoId,
                    Nombre = c.Nombre,
                    FechaInicio = c.FechaInicio,
                    Estado = c.CampeonatoEstado.Nombre,
                    Presidente = c.Presidente
                })
                .ToListAsync();
        }


        [McpServerTool, Description("""Lista los equipos que participan en un campeonato específico, incluyendo el grupo asignado a cada uno en ese campeonato. Requiere el CampeonatoId (obtenerlo antes con listar_campeonatos si no se conoce).""")]
        public static async Task<List<EquipoEnCampeonatoDto>> ListarEquiposDeCampeonato(
        PeloterosDbContext db,
        [Description("ID del campeonato a consultar.")] int campeonatoId,
        [Description("Filtrar solo por este grupo (ej. 'A', 'B'). Opcional.")] string? grupoId = null)
        {
            var query = db.CampeonatoEquipos
                .AsNoTracking()
                .Include(ce => ce.Equipo)
                .Include(ce => ce.Grupo)
                .Where(ce => ce.CampeonatoId == campeonatoId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(grupoId))
            {
                query = query.Where(ce => ce.GrupoId == grupoId);
            }

            return await query
                .OrderBy(ce => ce.Equipo.Nombre)
                .Select(ce => new EquipoEnCampeonatoDto
                {
                    EquipoId = ce.Equipo.EquipoId,
                    Nombre = ce.Equipo.Nombre,
                    NombreCorto = ce.Equipo.NombreCorto,
                    Grupo = ce.Grupo != null ? ce.Grupo.Nombre : null,
                    Delegado = ce.Equipo.Delegado
                })
                .ToListAsync();
        }


        [McpServerTool, Description("""Inicia un nuevo campeonato. Peloteros permite como máximo un campeonato activo a la vez, así que si hay uno activo, se inactiva automáticamente al crear el nuevo. IMPORTANTE: llamar primero con confirmar=false (el valor por default) para obtener una vista previa de lo que va a pasar, mostrarle esa información al usuario, y recién llamar de nuevo con confirmar=true si el usuario confirma explícitamente. Si el usuario en cambio quiere activar un campeonato que ya existe (por ejemplo uno viejo), usar ActivarCampeonatoExistente. Si quiere cerrar el activo sin crear ni activar otro, usar CerrarCampeonatoActivo. Requiere rol Administrator.""")]
        public static async Task<IniciarCampeonatoResultado> IniciarNuevoCampeonato(
        PeloterosDbContext db,
        IHttpContextAccessor httpContextAccessor,
        [Description("Nombre del nuevo campeonato.")] string nombre,
        [Description("Fecha de inicio del nuevo campeonato.")] DateTime fechaInicio,
        [Description("Color del campeonato en formato hexadecimal, ej. #ff5900. Opcional.")] string? color = null,
        [Description("Nombre del presidente del campeonato. Opcional.")] string? presidente = null,
        [Description("Nombre del vicepresidente del campeonato. Opcional.")] string? vicepresidente = null,
        [Description("Poner en true solo después de que el usuario haya visto la vista previa y confirmado.")] bool confirmar = false)
        {
            httpContextAccessor.RequireRole("Administrator");

            if (color != null && !Regex.IsMatch(color, "^#[0-9A-Fa-f]{6}$"))
                throw new McpException($"'{color}' no es un color hexadecimal válido. Formato esperado: #rrggbb, ej. #ff5900.");

            var activo = await db.Campeonatos
                .Include(c => c.Partidos).ThenInclude(p => p.Fase)
                .FirstOrDefaultAsync(c => c.CampeonatoEstadoId == "A");

            bool? tieneGanador = activo == null ? null : activo.Partidos.Any(p =>
                p.Fase.Nombre == "Final" && p.PartidoEstadoId == "D" && p.EquipoIdGanador != null);

            if (!confirmar)
            {
                return new IniciarCampeonatoResultado
                {
                    RequiereConfirmacion = true,
                    Mensaje = activo == null
                        ? "No hay ningún campeonato activo actualmente. Se creará el nuevo y quedará activo."
                        : $"Hay un campeonato activo: '{activo.Nombre}', que {(tieneGanador == true ? "ya tiene" : "todavía NO tiene")} un ganador definido. Al confirmar, se inactivará y se creará el nuevo como activo.",
                    CampeonatoActivoActual = activo?.Nombre,
                    CampeonatoActivoTieneGanadorDefinido = tieneGanador,
                    NuevoCampeonatoNombre = nombre,
                    NuevoCampeonatoFechaInicio = fechaInicio
                };
            }

            if (activo != null)
                activo.CampeonatoEstadoId = "I";

            db.Campeonatos.Add(new Campeonato
            {
                Nombre = nombre,
                FechaInicio = fechaInicio,
                CampeonatoEstadoId = "A",
                ColorCampeonato = color,
                Presidente = presidente,
                Vicepresidente = vicepresidente
            });

            await db.SaveChangesAsync();

            return new IniciarCampeonatoResultado
            {
                RequiereConfirmacion = false,
                Mensaje = activo == null
                    ? $"Campeonato '{nombre}' creado y activado. No había ninguno activo previamente."
                    : $"Campeonato '{nombre}' creado y activado. Se inactivó '{activo.Nombre}'.",
                CampeonatoActivoActual = activo?.Nombre,
                CampeonatoActivoTieneGanadorDefinido = tieneGanador,
                NuevoCampeonatoNombre = nombre,
                NuevoCampeonatoFechaInicio = fechaInicio
            };
        }


        [McpServerTool, Description("""Cierra (inactiva) el campeonato actualmente activo, SIN crear uno nuevo. Usar esto solo cuando el usuario pide explícitamente cerrar, finalizar o inactivar el torneo actual sin arrancar ni activar otro a continuación. Si el usuario pide iniciar un campeonato NUEVO, usar IniciarNuevoCampeonato. Si pide activar uno que ya existe, usar ActivarCampeonatoExistente. Requiere rol Administrator.""")]
        public static async Task<CerrarCampeonatoResultado> CerrarCampeonatoActivo(
            PeloterosDbContext db,
            IHttpContextAccessor httpContextAccessor,
            [Description("Poner en true solo después de que el usuario haya confirmado explícitamente.")] bool confirmar = false)
        {
            httpContextAccessor.RequireRole("Administrator");

            var activo = await db.Campeonatos
                .Include(c => c.Partidos).ThenInclude(p => p.Fase)
                .FirstOrDefaultAsync(c => c.CampeonatoEstadoId == "A");

            if (activo == null)
                return new CerrarCampeonatoResultado
                {
                    RequiereConfirmacion = false,
                    Mensaje = "No hay ningún campeonato activo para cerrar."
                };

            bool tieneGanador = activo.Partidos.Any(p =>
                p.Fase.Nombre == "Final" && p.PartidoEstadoId == "D" && p.EquipoIdGanador != null);

            if (!confirmar)
            {
                return new CerrarCampeonatoResultado
                {
                    RequiereConfirmacion = true,
                    Mensaje = $"Se cerraría '{activo.Nombre}', que {(tieneGanador ? "ya tiene" : "todavía NO tiene")} un ganador definido. Ningún campeonato quedaría activo después de esto.",
                    CampeonatoNombre = activo.Nombre
                };
            }

            activo.CampeonatoEstadoId = "I";
            await db.SaveChangesAsync();

            return new CerrarCampeonatoResultado
            {
                RequiereConfirmacion = false,
                Mensaje = $"Campeonato '{activo.Nombre}' cerrado. No queda ningún campeonato activo.",
                CampeonatoNombre = activo.Nombre
            };
        }


        [McpServerTool, Description("""Activa un campeonato EXISTENTE (por ejemplo, uno viejo que quedó incompleto), inactivando automáticamente el que esté activo en ese momento. NO crea ningún campeonato nuevo. Usar esto cuando el usuario pide reactivar, volver a activar, o cambiar cuál campeonato está activo entre campeonatos que ya existen. Si el usuario pide iniciar un campeonato NUEVO, usar IniciarNuevoCampeonato. Si pide cerrar el activo sin activar otro, usar CerrarCampeonatoActivo. Requiere rol Administrator.""")]
        public static async Task<ActivarCampeonatoResultado> ActivarCampeonatoExistente(
            PeloterosDbContext db,
            IHttpContextAccessor httpContextAccessor,
            [Description("ID del campeonato existente a activar.")] int campeonatoId,
            [Description("Poner en true solo después de que el usuario haya confirmado explícitamente.")] bool confirmar = false)
        {
            httpContextAccessor.RequireRole("Administrator");

            var destino = await db.Campeonatos.FindAsync(campeonatoId);
            if (destino == null)
                throw new McpException($"No existe un campeonato con ID {campeonatoId}.");

            if (destino.CampeonatoEstadoId == "A")
                return new ActivarCampeonatoResultado
                {
                    RequiereConfirmacion = false,
                    Mensaje = $"'{destino.Nombre}' ya está activo. No se hizo ningún cambio."
                };

            var actual = await db.Campeonatos.FirstOrDefaultAsync(c => c.CampeonatoEstadoId == "A");

            if (!confirmar)
            {
                return new ActivarCampeonatoResultado
                {
                    RequiereConfirmacion = true,
                    Mensaje = actual == null
                        ? $"Se activará '{destino.Nombre}'. No hay ningún campeonato activo actualmente."
                        : $"Se activará '{destino.Nombre}' y se inactivará el actual, '{actual.Nombre}'.",
                    CampeonatoActivadoNombre = destino.Nombre,
                    CampeonatoInactivadoNombre = actual?.Nombre
                };
            }

            if (actual != null)
                actual.CampeonatoEstadoId = "I";
            destino.CampeonatoEstadoId = "A";

            await db.SaveChangesAsync();

            return new ActivarCampeonatoResultado
            {
                RequiereConfirmacion = false,
                Mensaje = actual == null
                    ? $"'{destino.Nombre}' activado."
                    : $"'{destino.Nombre}' activado. '{actual.Nombre}' quedó inactivo.",
                CampeonatoActivadoNombre = destino.Nombre,
                CampeonatoInactivadoNombre = actual?.Nombre
            };
        }
    }
}
