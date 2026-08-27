using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Jugador
{
    public int JugadorId { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string? Apodo { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Ci { get; set; } = null!;

    public string? Telefono { get; set; }

    public byte NroCamiseta { get; set; }

    public byte? PosicionId { get; set; }

    public byte[]? JugadorImagen { get; set; }

    public string? JugadorImagenNombre { get; set; }

    public int? EquipoId { get; set; }

    public string? JugadorEstadoId { get; set; }

    public virtual Equipo? Equipo { get; set; }

    public virtual ICollection<EquipoJugador> EquipoJugadors { get; set; } = new List<EquipoJugador>();

    public virtual JugadorEstado? JugadorEstado { get; set; }

    public virtual ICollection<PartidoJugador> PartidoJugadors { get; set; } = new List<PartidoJugador>();

    public virtual Posicion? Posicion { get; set; }

    public virtual ICollection<Transferencium> Transferencia { get; set; } = new List<Transferencium>();
}
