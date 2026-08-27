using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class JugadorEstado
{
    public string JugadorEstadoId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();
}
