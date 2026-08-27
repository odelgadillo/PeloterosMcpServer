namespace PeloterosMcpServer.DTOs
{
    public class PartidoDetalleDto
    {
        public int PartidoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Campeonato { get; set; }
        public string? Fase { get; set; }
        public string? Arbitro { get; set; }
        public string? EquipoA { get; set; }
        public string? EquipoB { get; set; }
        public byte? GolesEquipoA { get; set; }
        public byte? GolesEquipoB { get; set; }
        public string? EquipoGanador { get; set; }
        public bool? Walkover { get; set; }
        public bool? HuboPenales { get; set; }
        public byte? PenalesEquipoA { get; set; }
        public byte? PenalesEquipoB { get; set; }
        public string? Estado { get; set; }
        public string? InformeArbitro { get; set; }
    }
}
