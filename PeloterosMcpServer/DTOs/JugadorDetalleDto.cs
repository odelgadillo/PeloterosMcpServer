namespace PeloterosMcpServer.DTOs
{
    public class JugadorDetalleDto
    {
        public int JugadorId { get; set; }
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string? Apodo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public byte NroCamiseta { get; set; }
        public string? Posicion { get; set; }
        public string? Equipo { get; set; }
        public string? Estado { get; set; }
    }
}
