namespace PeloterosMcpServer.DTOs
{
    public class AsistenciaEquipoDto
    {
        public int EquipoId { get; set; }
        public string Equipo { get; set; } = null!;
        public int TotalReuniones { get; set; }
        public int Asistencias { get; set; }
        public int FaltasConLicencia { get; set; }
        public int FaltasSinLicencia { get; set; }
    }
}
