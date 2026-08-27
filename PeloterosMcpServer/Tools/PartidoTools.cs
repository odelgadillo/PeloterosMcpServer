using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.DTOs;
using System.ComponentModel;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class PartidoTools
    {
        [McpServerTool, Description(
        "Lista partidos filtrando por campeonato, equipo, rango de fechas y/o estado " +
        "(P=Programado, C=Confirmado, D=Definido/finalizado, X=Anulado). " +
        "Usar para preguntas de agenda, calendario o qué partidos se jugaron/jugarán.")]
        public static async Task<List<PartidoResumenDto>> ListarPartidos(
        PeloterosDbContext db,
        [Description("ID del campeonato. Opcional.")] int? campeonatoId = null,
        [Description("ID de un equipo que haya jugado como local o visitante. Opcional.")] int? equipoId = null,
        [Description("Fecha desde (inclusive). Opcional.")] DateTime? fechaDesde = null,
        [Description("Fecha hasta (inclusive). Opcional.")] DateTime? fechaHasta = null,
        [Description("Código de estado: P, C, D o X. Opcional.")] string? estado = null,
        [Description("Cantidad máxima de resultados.")] int limite = 30)
        {
            limite = Math.Clamp(limite, 1, 100);

            var query = db.Partidos
                .AsNoTracking()
                .Include(p => p.EquipoIdANavigation)
                .Include(p => p.EquipoIdBNavigation)
                .Include(p => p.PartidoEstado)
                .Include(p => p.Fase)
                .AsQueryable();

            if (campeonatoId.HasValue)
                query = query.Where(p => p.CampeonatoId == campeonatoId);

            if (equipoId.HasValue)
                query = query.Where(p => p.EquipoIdA == equipoId || p.EquipoIdB == equipoId);

            if (fechaDesde.HasValue)
                query = query.Where(p => p.FechaHora >= fechaDesde);

            if (fechaHasta.HasValue)
                query = query.Where(p => p.FechaHora <= fechaHasta);

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(p => p.PartidoEstadoId == estado);

            return await query
                .OrderBy(p => p.FechaHora)
                .Take(limite)
                .Select(p => new PartidoResumenDto
                {
                    PartidoId = p.PartidoId,
                    FechaHora = p.FechaHora,
                    EquipoA = p.EquipoIdANavigation != null ? p.EquipoIdANavigation.Nombre : null,
                    EquipoB = p.EquipoIdBNavigation != null ? p.EquipoIdBNavigation.Nombre : null,
                    GolesEquipoA = p.GolesEquipoA,
                    GolesEquipoB = p.GolesEquipoB,
                    Estado = p.PartidoEstado != null ? p.PartidoEstado.Nombre : null,
                    Fase = p.Fase != null ? p.Fase.Nombre : null
                })
                .ToListAsync();
        }

        [McpServerTool, Description(
            "Obtiene el detalle completo de un partido específico: resultado, penales, walkover, " +
            "árbitro e informe. Usar cuando ya se conoce el PartidoId, por ejemplo tras listar_partidos.")]
        public static async Task<PartidoDetalleDto?> ObtenerDetallePartido(
            PeloterosDbContext db,
            [Description("ID del partido a consultar.")] int partidoId)
        {
            return await db.Partidos
                .AsNoTracking()
                .Include(p => p.EquipoIdANavigation)
                .Include(p => p.EquipoIdBNavigation)
                .Include(p => p.EquipoIdGanadorNavigation)
                .Include(p => p.PartidoEstado)
                .Include(p => p.Fase)
                .Include(p => p.Campeonato)
                .Include(p => p.Arbitro)
                .Where(p => p.PartidoId == partidoId)
                .Select(p => new PartidoDetalleDto
                {
                    PartidoId = p.PartidoId,
                    FechaHora = p.FechaHora,
                    Campeonato = p.Campeonato != null ? p.Campeonato.Nombre : null,
                    Fase = p.Fase != null ? p.Fase.Nombre : null,
                    Arbitro = p.Arbitro != null ? p.Arbitro.Nombre : null,
                    EquipoA = p.EquipoIdANavigation != null ? p.EquipoIdANavigation.Nombre : null,
                    EquipoB = p.EquipoIdBNavigation != null ? p.EquipoIdBNavigation.Nombre : null,
                    GolesEquipoA = p.GolesEquipoA,
                    GolesEquipoB = p.GolesEquipoB,
                    EquipoGanador = p.EquipoIdGanadorNavigation != null ? p.EquipoIdGanadorNavigation.Nombre : null,
                    Walkover = p.Walkower,
                    HuboPenales = p.Penales,
                    PenalesEquipoA = p.PenalesEquipoA,
                    PenalesEquipoB = p.PenalesEquipoB,
                    Estado = p.PartidoEstado != null ? p.PartidoEstado.Nombre : null,
                    InformeArbitro = p.InformeArbitro
                })
                .FirstOrDefaultAsync();
        }

        [McpServerTool, Description(
            "Devuelve el ranking de goleadores de un campeonato, ordenado de mayor a menor cantidad " +
            "de goles. Usar para preguntas sobre quién es el máximo goleador o tabla de goleo.")]
        public static async Task<List<GoleadorDto>> ListarGoleadores(
            PeloterosDbContext db,
            [Description("ID del campeonato a consultar.")] int campeonatoId,
            [Description("Cantidad máxima de jugadores a devolver.")] int limite = 10)
        {
            limite = Math.Clamp(limite, 1, 50);

            return await db.PartidoJugadors
                .AsNoTracking()
                .Where(pj => pj.Partido.CampeonatoId == campeonatoId && pj.Goles > 0)
                .GroupBy(pj => new { pj.JugadorId, pj.Jugador.Nombre, pj.Jugador.ApellidoPaterno, pj.Jugador.ApellidoMaterno, EquipoNombre = pj.Equipo != null ? pj.Equipo.Nombre : null })
                .Select(g => new GoleadorDto
                {
                    JugadorId = g.Key.JugadorId,
                    NombreCompleto = $"{g.Key.Nombre} {g.Key.ApellidoPaterno} {g.Key.ApellidoMaterno}",
                    Equipo = g.Key.EquipoNombre,
                    TotalGoles = g.Sum(pj => (int)(pj.Goles ?? 0))
                })
                .OrderByDescending(g => g.TotalGoles)
                .Take(limite)
                .ToListAsync();
        }

        [McpServerTool, Description(
            "Lista las sanciones (tarjetas amarilla, doble amarilla o roja) recibidas por un jugador " +
            "específico, opcionalmente filtrando por campeonato. Usar para consultar historial " +
            "disciplinario de un jugador.")]
        public static async Task<List<SancionJugadorDto>> ListarSancionesJugador(
            PeloterosDbContext db,
            [Description("ID del jugador a consultar.")] int jugadorId,
            [Description("ID de campeonato para acotar la búsqueda. Opcional.")] int? campeonatoId = null)
        {
            var query = db.PartidoJugadors
                .AsNoTracking()
                .Include(pj => pj.Partido)
                .Include(pj => pj.JugadorSancion)
                .Where(pj => pj.JugadorId == jugadorId && pj.JugadorSancionId != null)
                .AsQueryable();

            if (campeonatoId.HasValue)
                query = query.Where(pj => pj.Partido.CampeonatoId == campeonatoId);

            // Rival: el otro equipo del partido, distinto del equipo con el que jugó el jugador
            return await query
                .OrderByDescending(pj => pj.Partido.FechaHora)
                .Select(pj => new SancionJugadorDto
                {
                    PartidoId = pj.PartidoId,
                    FechaHora = pj.Partido.FechaHora,
                    Rival = pj.EquipoId == pj.Partido.EquipoIdA
                        ? (pj.Partido.EquipoIdBNavigation != null ? pj.Partido.EquipoIdBNavigation.Nombre : null)
                        : (pj.Partido.EquipoIdANavigation != null ? pj.Partido.EquipoIdANavigation.Nombre : null),
                    TipoSancion = pj.JugadorSancion!.Nombre
                })
                .ToListAsync();
        }
    }
}
