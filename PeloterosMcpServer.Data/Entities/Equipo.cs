using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Equipo
{
    public int EquipoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? NombreCorto { get; set; }

    public string? Abreviatura { get; set; }

    public string Delegado { get; set; } = null!;

    public string? DelegadoCi { get; set; }

    public string? Telefono { get; set; }

    public byte[]? EscudoImagen { get; set; }

    public string? EscudoImagenNombre { get; set; }

    public string? GrupoId { get; set; }

    public virtual ICollection<CampeonatoEquipo> CampeonatoEquipos { get; set; } = new List<CampeonatoEquipo>();

    public virtual ICollection<EquipoDelegado> EquipoDelegados { get; set; } = new List<EquipoDelegado>();

    public virtual ICollection<EquipoJugador> EquipoJugadors { get; set; } = new List<EquipoJugador>();

    public virtual Grupo? Grupo { get; set; }

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();

    public virtual ICollection<Partido> PartidoEquipoIdANavigations { get; set; } = new List<Partido>();

    public virtual ICollection<Partido> PartidoEquipoIdBNavigations { get; set; } = new List<Partido>();

    public virtual ICollection<Partido> PartidoEquipoIdGanadorNavigations { get; set; } = new List<Partido>();

    public virtual ICollection<PartidoJugador> PartidoJugadors { get; set; } = new List<PartidoJugador>();

    public virtual ICollection<ReunionAsistencium> ReunionAsistencia { get; set; } = new List<ReunionAsistencium>();

    public virtual ICollection<Sancion> Sancions { get; set; } = new List<Sancion>();

    public virtual ICollection<Transferencium> TransferenciumEquipoIdDestinoNavigations { get; set; } = new List<Transferencium>();

    public virtual ICollection<Transferencium> TransferenciumEquipoIdOrigenNavigations { get; set; } = new List<Transferencium>();
}
