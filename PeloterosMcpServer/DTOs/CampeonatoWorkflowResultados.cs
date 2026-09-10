namespace PeloterosMcpServer.DTOs
{
    public class IniciarCampeonatoResultado
    {
        public bool RequiereConfirmacion { get; set; }
        public string Mensaje { get; set; }
        public string? CampeonatoActivoActual { get; set; }
        public bool? CampeonatoActivoTieneGanadorDefinido { get; set; }
        public string NuevoCampeonatoNombre { get; set; }
        public DateTime NuevoCampeonatoFechaInicio { get; set; }
    }

    public class CerrarCampeonatoResultado
    {
        public bool RequiereConfirmacion { get; set; }
        public string Mensaje { get; set; }
        public string? CampeonatoNombre { get; set; }
    }

    public class ActivarCampeonatoResultado
    {
        public bool RequiereConfirmacion { get; set; }
        public string Mensaje { get; set; }
        public string? CampeonatoActivadoNombre { get; set; }
        public string? CampeonatoInactivadoNombre { get; set; }
    }
}