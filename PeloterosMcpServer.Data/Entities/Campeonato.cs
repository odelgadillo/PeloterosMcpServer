using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Campeonato
{
    public int CampeonatoId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public string? Urlreglamento { get; set; }

    public string? Urlconvocatoria { get; set; }

    public string CampeonatoEstadoId { get; set; } = null!;

    public string? ColorCampeonato { get; set; }

    public string? Presidente { get; set; }

    public string? Vicepresidente { get; set; }

    public virtual ICollection<CampeonatoEquipo> CampeonatoEquipos { get; set; } = new List<CampeonatoEquipo>();

    public virtual CampeonatoEstado CampeonatoEstado { get; set; } = null!;

    public virtual ICollection<EquipoJugador> EquipoJugadors { get; set; } = new List<EquipoJugador>();

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();

    public virtual ICollection<Reunion> Reunions { get; set; } = new List<Reunion>();

    public virtual ICollection<Sancion> Sancions { get; set; } = new List<Sancion>();
}
