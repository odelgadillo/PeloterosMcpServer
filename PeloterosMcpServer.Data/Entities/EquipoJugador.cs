using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class EquipoJugador
{
    public int EquipoJugadorId { get; set; }

    public int EquipoId { get; set; }

    public int JugadorId { get; set; }

    public int? CampeonatoId { get; set; }

    public virtual Campeonato? Campeonato { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Jugador Jugador { get; set; } = null!;
}
