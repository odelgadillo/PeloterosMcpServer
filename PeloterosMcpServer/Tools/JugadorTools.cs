using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.DTOs;
using System.ComponentModel;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class JugadorTools
    {
        [McpServerTool, Description(
        "Busca jugadores por nombre parcial, apodo, posición o estado de verificación. " +
        "Si se llama sin filtros, devuelve los primeros jugadores registrados. " +
        "Usar cuando el usuario pida listar o encontrar jugadores por algún criterio.")]
        public static async Task<List<JugadorResumenDto>> BuscarJugadores(
        PeloterosDbContext db,
        [Description("Texto parcial a buscar en nombre, apellido o apodo. Opcional.")] string? nombre = null,
        [Description("Nombre de la posición a filtrar, ej. 'Delantero'. Opcional.")] string? posicion = null,
        [Description("Si es true, solo devuelve jugadores verificados (excluye los pendientes de verificar).")] bool soloVerificados = false,
        [Description("Cantidad máxima de resultados a devolver.")] int limite = 20)
        {
            limite = Math.Clamp(limite, 1, 100);

            var query = db.Jugadors
                .AsNoTracking()
                .Include(j => j.Posicion)
                .Include(j => j.Equipo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(j =>
                    j.Nombre.Contains(nombre) ||
                    j.ApellidoPaterno.Contains(nombre) ||
                    j.ApellidoMaterno.Contains(nombre) ||
                    (j.Apodo != null && j.Apodo.Contains(nombre)));
            }

            if (!string.IsNullOrWhiteSpace(posicion))
            {
                query = query.Where(j => j.Posicion != null && j.Posicion.Nombre.Contains(posicion));
            }

            if (soloVerificados)
            {
                query = query.Where(j => j.JugadorEstadoId == "V");
            }

            return await query
                .OrderBy(j => j.ApellidoPaterno)
                .Take(limite)
                .Select(j => new JugadorResumenDto
                {
                    JugadorId = j.JugadorId,
                    NombreCompleto = $"{j.Nombre} {j.ApellidoPaterno} {j.ApellidoMaterno}",
                    Apodo = j.Apodo,
                    NroCamiseta = j.NroCamiseta,
                    Posicion = j.Posicion != null ? j.Posicion.Nombre : null,
                    Equipo = j.Equipo != null ? j.Equipo.Nombre : null,
                    Estado = j.JugadorEstadoId == "V" ? "Verificado" : "Registrado"
                })
                .ToListAsync();
        }


        [McpServerTool, Description(
        "Obtiene el detalle completo de un jugador específico a partir de su ID. " +
        "Usar cuando ya se conoce el JugadorId exacto, por ejemplo tras usar buscar_jugadores.")]
        public static async Task<JugadorDetalleDto?> ObtenerJugadorPorId(
        PeloterosDbContext db,
        [Description("ID del jugador a consultar.")] int jugadorId)
        {
            return await db.Jugadors
                .AsNoTracking()
                .Include(j => j.Posicion)
                .Include(j => j.Equipo)
                .Include(j => j.JugadorEstado)
                .Where(j => j.JugadorId == jugadorId)
                .Select(j => new JugadorDetalleDto
                {
                    JugadorId = j.JugadorId,
                    Nombre = j.Nombre,
                    ApellidoPaterno = j.ApellidoPaterno,
                    ApellidoMaterno = j.ApellidoMaterno,
                    Apodo = j.Apodo,
                    FechaNacimiento = j.FechaNacimiento,
                    NroCamiseta = j.NroCamiseta,
                    Posicion = j.Posicion != null ? j.Posicion.Nombre : null,
                    Equipo = j.Equipo != null ? j.Equipo.Nombre : null,
                    Estado = j.JugadorEstado != null ? j.JugadorEstado.Nombre : null
                })
                .FirstOrDefaultAsync();
        }

        [McpServerTool, Description(
        "Cuenta cuántos jugadores hay registrados, opcionalmente filtrando por verificación. " +
        "Usar para preguntas de tipo '¿cuántos jugadores hay?' sin necesidad de listar todos.")]
        public static async Task<int> ContarJugadores(
        PeloterosDbContext db,
        [Description("Si es true, cuenta solo jugadores verificados.")] bool soloVerificados = false)
        {
            var query = db.Jugadors.AsNoTracking().AsQueryable();

            if (soloVerificados)
            {
                query = query.Where(j => j.JugadorEstadoId == "V");
            }

            return await query.CountAsync();
        }

    }
}
