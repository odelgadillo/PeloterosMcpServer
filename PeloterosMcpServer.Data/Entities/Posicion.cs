using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Posicion
{
    public byte PosicionId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();
}
