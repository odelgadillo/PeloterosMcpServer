namespace PeloterosMcpServer.DTOs
{
    public class GoleadorDto
    {
        public int JugadorId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string? Equipo { get; set; }
        public int TotalGoles { get; set; }
    }
}
