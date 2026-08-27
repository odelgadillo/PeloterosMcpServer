using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class PartidoJugador
{
    public int PartidoJugadorId { get; set; }

    public int PartidoId { get; set; }

    public int JugadorId { get; set; }

    public byte? Goles { get; set; }

    public byte? NroCamiseta { get; set; }

    public string? JugadorSancionId { get; set; }

    public string? JugadorSustitucionId { get; set; }

    public int? EquipoId { get; set; }

    public virtual Equipo? Equipo { get; set; }

    public virtual Jugador Jugador { get; set; } = null!;

    public virtual JugadorSancion? JugadorSancion { get; set; }

    public virtual JugadorSustitucion? JugadorSustitucion { get; set; }

    public virtual Partido Partido { get; set; } = null!;
}
