namespace PeloterosMcpServer.DTOs
{
    public class CampeonatoDto
    {
        public int CampeonatoId { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public string Estado { get; set; } = null!;
        public string? Presidente { get; set; }
    }
}
