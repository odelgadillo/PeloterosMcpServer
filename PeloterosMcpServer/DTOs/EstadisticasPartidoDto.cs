namespace PeloterosMcpServer.DTOs
{
    public class RankingTarjetasDto
    {
        public int EquipoId { get; set; }
        public string Equipo { get; set; } = null!;
        public int TotalAmarillas { get; set; }
        public int TotalRojas { get; set; }
    }
}
