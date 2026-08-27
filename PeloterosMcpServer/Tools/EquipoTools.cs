using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.DTOs;
using System.ComponentModel;

namespace PeloterosMcpServer.Tools
{
    [McpServerToolType]
    public class EquipoTools
    {
        [McpServerTool, Description(
        "Lista los delegados habilitados de un equipo, con su correo electrónico de contacto. " +
        "Usar cuando se pregunte quién es el delegado o cómo contactar a un equipo.")]
        public static async Task<List<DelegadoDto>> ListarDelegadosDeEquipo(
        PeloterosDbContext db,
        [Description("ID del equipo a consultar.")] int equipoId)
        {
            return await db.EquipoDelegados
                .AsNoTracking()
                .Where(d => d.EquipoId == equipoId)
                .Select(d => new DelegadoDto
                {
                    Nombre = d.DelegadoNombre,
                    CorreoElectronico = d.DelegadoCorreoElectronico
                })
                .ToListAsync();
        }
    }
}
