using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.DTOs;
using System.ComponentModel;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class CampeonatoTools
    {
        [McpServerTool, Description(
        "Lista los campeonatos registrados, opcionalmente filtrando solo los activos. " +
        "Usar para ubicar el CampeonatoId antes de consultar equipos, partidos u otra " +
        "información específica de un campeonato.")]
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

        [McpServerTool, Description(
            "Lista los equipos que participan en un campeonato específico, incluyendo el grupo " +
            "asignado a cada uno en ese campeonato. Requiere el CampeonatoId " +
            "(obtenerlo antes con listar_campeonatos si no se conoce).")]
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
    }
}
