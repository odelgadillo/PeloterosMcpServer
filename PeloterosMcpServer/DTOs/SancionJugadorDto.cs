namespace PeloterosMcpServer.DTOs
{
    public class SancionJugadorDto
    {
        public int PartidoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Rival { get; set; }
        public string TipoSancion { get; set; } = null!;
    }
}
