namespace PeloterosMcpServer.DTOs
{
    public class TransferenciaDto
    {
        public int TransferenciaId { get; set; }
        public string Jugador { get; set; } = null!;
        public string EquipoOrigen { get; set; } = null!;
        public string EquipoDestino { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime? Fecha { get; set; }
        public short? Temporadas { get; set; }
    }
}
