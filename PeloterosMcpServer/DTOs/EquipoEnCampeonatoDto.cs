namespace PeloterosMcpServer.DTOs
{
    public class EquipoEnCampeonatoDto
    {
        public int EquipoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? NombreCorto { get; set; }
        public string? Grupo { get; set; }
        public string Delegado { get; set; } = null!;
    }
}
