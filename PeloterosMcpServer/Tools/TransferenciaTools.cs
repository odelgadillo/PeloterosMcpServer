using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.DTOs;
using System.ComponentModel;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class TransferenciaTools
    {
        [McpServerTool, Description(
        "Lista transferencias de jugadores entre equipos, filtrando opcionalmente por jugador, " +
        "equipo (origen o destino) o estado (usar el nombre del estado, ej. 'En curso', 'Aprobada', 'Anulada'). " +
        "Usar para preguntas sobre movimientos de jugadores entre equipos.")]
        public static async Task<List<TransferenciaDto>> ListarTransferencias(
        PeloterosDbContext db,
        [Description("ID del jugador involucrado. Opcional.")] int? jugadorId = null,
        [Description("ID de un equipo, como origen o destino. Opcional.")] int? equipoId = null,
        [Description("Nombre del estado a filtrar (ej. 'En curso'). Opcional.")] string? estado = null,
        [Description("Cantidad máxima de resultados.")] int limite = 30)
        {
            limite = Math.Clamp(limite, 1, 100);

            var query = db.Transferencia
                .AsNoTracking()
                .Include(t => t.Jugador)
                .Include(t => t.EquipoIdOrigenNavigation)
                .Include(t => t.EquipoIdDestinoNavigation)
                .Include(t => t.TransferenciaTipo)
                .Include(t => t.TransferenciaEstado)
                .AsQueryable();

            if (jugadorId.HasValue)
                query = query.Where(t => t.JugadorId == jugadorId);

            if (equipoId.HasValue)
                query = query.Where(t => t.EquipoIdOrigen == equipoId || t.EquipoIdDestino == equipoId);

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(t => t.TransferenciaEstado.Nombre == estado);

            return await query
                .OrderByDescending(t => t.Fecha)
                .Take(limite)
                .Select(t => new TransferenciaDto
                {
                    TransferenciaId = t.TransferenciaId,
                    Jugador = $"{t.Jugador.Nombre} {t.Jugador.ApellidoPaterno} {t.Jugador.ApellidoMaterno}",
                    EquipoOrigen = t.EquipoIdOrigenNavigation.Nombre,
                    EquipoDestino = t.EquipoIdDestinoNavigation.Nombre,
                    Tipo = t.TransferenciaTipo.Nombre,
                    Estado = t.TransferenciaEstado.Nombre,
                    Fecha = t.Fecha,
                    Temporadas = t.Temporadas
                })
                .ToListAsync();
        }

        [McpServerTool, Description(
            "Devuelve el historial completo de transferencias de un jugador específico, ordenado " +
            "cronológicamente. Usar para preguntas del tipo 'por qué equipos pasó este jugador'.")]
        public static async Task<List<TransferenciaDto>> ObtenerHistorialTransferenciasJugador(
            PeloterosDbContext db,
            [Description("ID del jugador a consultar.")] int jugadorId)
        {
            return await db.Transferencia
                .AsNoTracking()
                .Include(t => t.Jugador)
                .Include(t => t.EquipoIdOrigenNavigation)
                .Include(t => t.EquipoIdDestinoNavigation)
                .Include(t => t.TransferenciaTipo)
                .Include(t => t.TransferenciaEstado)
                .Where(t => t.JugadorId == jugadorId)
                .OrderBy(t => t.Fecha)
                .Select(t => new TransferenciaDto
                {
                    TransferenciaId = t.TransferenciaId,
                    Jugador = $"{t.Jugador.Nombre} {t.Jugador.ApellidoPaterno} {t.Jugador.ApellidoMaterno}",
                    EquipoOrigen = t.EquipoIdOrigenNavigation.Nombre,
                    EquipoDestino = t.EquipoIdDestinoNavigation.Nombre,
                    Tipo = t.TransferenciaTipo.Nombre,
                    Estado = t.TransferenciaEstado.Nombre,
                    Fecha = t.Fecha,
                    Temporadas = t.Temporadas
                })
                .ToListAsync();
        }
    }
}
