using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Partido
{
    public int PartidoId { get; set; }

    public DateTime FechaHora { get; set; }

    public int? EquipoIdA { get; set; }

    public int? EquipoIdB { get; set; }

    public byte? GolesEquipoA { get; set; }

    public byte? GolesEquipoB { get; set; }

    public string? PartidoEstadoId { get; set; }

    public int? EquipoIdGanador { get; set; }

    public bool? Walkower { get; set; }

    public string? InformeArbitro { get; set; }

    public int? TamarillaEquipoA { get; set; }

    public int? TamarillaEquipoB { get; set; }

    public int? TrojaEquipoA { get; set; }

    public int? TrojaEquipoB { get; set; }

    public bool? BanderaEquipoA { get; set; }

    public bool? BanderaEquipoB { get; set; }

    public bool? BalonEquipoA { get; set; }

    public bool? BalonEquipoB { get; set; }

    public int? FaseId { get; set; }

    public bool? Penales { get; set; }

    public byte? PenalesEquipoA { get; set; }

    public byte? PenalesEquipoB { get; set; }

    public int? CampeonatoId { get; set; }

    public int? ArbitroId { get; set; }

    public virtual Arbitro? Arbitro { get; set; }

    public virtual Campeonato? Campeonato { get; set; }

    public virtual Equipo? EquipoIdANavigation { get; set; }

    public virtual Equipo? EquipoIdBNavigation { get; set; }

    public virtual Equipo? EquipoIdGanadorNavigation { get; set; }

    public virtual Fase? Fase { get; set; }

    public virtual PartidoEstado? PartidoEstado { get; set; }

    public virtual ICollection<PartidoJugador> PartidoJugadors { get; set; } = new List<PartidoJugador>();
}
