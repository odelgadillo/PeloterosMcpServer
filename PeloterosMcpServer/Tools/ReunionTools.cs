using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.DTOs;
using System.ComponentModel;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class ReunionTools
    {
        [McpServerTool, Description(
        "Lista reuniones con su acta (qué se trató), filtrando opcionalmente por campeonato o " +
        "por una fecha exacta. Sin filtro de fecha y con límite 1, devuelve la última reunión. " +
        "Usar para preguntas sobre qué se trató en una reunión, sea la última o una fecha puntual.")]
        public static async Task<List<ReunionDto>> ListarReuniones(
        PeloterosDbContext db,
        [Description("ID del campeonato a filtrar. Opcional.")] int? campeonatoId = null,
        [Description("Fecha exacta de la reunión a buscar (solo la fecha, sin hora). Opcional.")] DateTime? fecha = null,
        [Description("Cantidad máxima de resultados, ordenados de más reciente a más antigua.")] int limite = 10)
        {
            limite = Math.Clamp(limite, 1, 50);

            var query = db.Reunions
                .AsNoTracking()
                .Include(r => r.Campeonato)
                .AsQueryable();

            if (campeonatoId.HasValue)
                query = query.Where(r => r.CampeonatoId == campeonatoId);

            if (fecha.HasValue)
                query = query.Where(r => r.FechaHora.Date == fecha.Value.Date);

            return await query
                .OrderByDescending(r => r.FechaHora)
                .Take(limite)
                .Select(r => new ReunionDto
                {
                    ReunionId = r.ReunionId,
                    FechaHora = r.FechaHora,
                    Acta = r.Acta,
                    Campeonato = r.Campeonato != null ? r.Campeonato.Nombre : null
                })
                .ToListAsync();
        }

        [McpServerTool, Description(
            "Cuenta cuántas reuniones se realizaron en un campeonato.")]
        public static async Task<int> ContarReuniones(
            PeloterosDbContext db,
            [Description("ID del campeonato a consultar.")] int campeonatoId)
        {
            return await db.Reunions
                .AsNoTracking()
                .Where(r => r.CampeonatoId == campeonatoId)
                .CountAsync();
        }

        [McpServerTool, Description(
            "Devuelve estadísticas de asistencia a reuniones por equipo dentro de un campeonato: " +
            "total de reuniones, asistencias, faltas con licencia (justificadas, no pagan multa) y " +
            "faltas sin licencia (las que generan multa). Usar para calcular multas, ver ranking de " +
            "cumplimiento, o detectar equipos con licencias repetidas (mirando el campo FaltasConLicencia).")]
        public static async Task<List<AsistenciaEquipoDto>> ObtenerEstadisticasAsistencia(
            PeloterosDbContext db,
            [Description("ID del campeonato a consultar.")] int campeonatoId)
        {
            return await db.ReunionAsistencia
                .AsNoTracking()
                .Where(ra => ra.Reunion.CampeonatoId == campeonatoId)
                .GroupBy(ra => new { ra.EquipoId, ra.Equipo.Nombre })
                .Select(g => new AsistenciaEquipoDto
                {
                    EquipoId = g.Key.EquipoId,
                    Equipo = g.Key.Nombre,
                    TotalReuniones = g.Count(),
                    Asistencias = g.Count(ra => ra.Asistencia),
                    FaltasConLicencia = g.Count(ra => !ra.Asistencia && ra.Licencia == true),
                    FaltasSinLicencia = g.Count(ra => !ra.Asistencia && ra.Licencia != true)
                })
                .OrderByDescending(a => a.Asistencias)
                .ToListAsync();
        }

        [McpServerTool, Description(
            "Lista las fechas de reunión en las que un equipo específico pidió licencia (faltó " +
            "justificadamente). Usar para el detalle de licencias de un equipo puntual.")]
        public static async Task<List<LicenciaDto>> ListarLicenciasDeEquipo(
            PeloterosDbContext db,
            [Description("ID del equipo a consultar.")] int equipoId,
            [Description("ID de campeonato para acotar la búsqueda. Opcional.")] int? campeonatoId = null)
        {
            var query = db.ReunionAsistencia
                .AsNoTracking()
                .Include(ra => ra.Reunion)
                .Where(ra => ra.EquipoId == equipoId && ra.Licencia == true)
                .AsQueryable();

            if (campeonatoId.HasValue)
                query = query.Where(ra => ra.Reunion.CampeonatoId == campeonatoId);

            return await query
                .OrderByDescending(ra => ra.Reunion.FechaHora)
                .Select(ra => new LicenciaDto
                {
                    ReunionId = ra.ReunionId,
                    FechaHora = ra.Reunion.FechaHora
                })
                .ToListAsync();
        }
    }
}
