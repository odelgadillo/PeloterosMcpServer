using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeloterosMcpServer.Data.Context;

namespace PeloterosMcpServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController (PeloterosDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                // Consultamos la cantidad de registros en la base de datos
                var totalEquipos = await db.Equipos.CountAsync();

                return Ok(new
                {
                    Status = "OK",
                    Message = "Conexión a Datos exitosa desde el servidor",
                    EquiposRegistrados = totalEquipos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = "Fallo al conectar con la base de datos",
                    Detail = ex.Message
                });
            }
        }
    }
}
