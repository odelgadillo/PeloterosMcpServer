namespace PeloterosMcpServer.DTOs
{
    public class ReunionDto
    {
        public int ReunionId { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Acta { get; set; }
        public string? Campeonato { get; set; }
    }
}
