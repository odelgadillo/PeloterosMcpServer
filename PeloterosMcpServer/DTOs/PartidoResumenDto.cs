namespace PeloterosMcpServer.DTOs
{
    public class PartidoResumenDto
    {
        public int PartidoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string? EquipoA { get; set; }
        public string? EquipoB { get; set; }
        public byte? GolesEquipoA { get; set; }
        public byte? GolesEquipoB { get; set; }
        public string? Estado { get; set; }
        public string? Fase { get; set; }
    }
}
