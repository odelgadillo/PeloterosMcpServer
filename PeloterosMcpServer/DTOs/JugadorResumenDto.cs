namespace PeloterosMcpServer.DTOs
{
    public class JugadorResumenDto
    {
        public int JugadorId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string? Apodo { get; set; }
        public byte NroCamiseta { get; set; }
        public string? Posicion { get; set; }
        public string? Equipo { get; set; }
        public string? Estado { get; set; }
    }
}
