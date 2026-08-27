using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class JugadorSancion
{
    public string JugadorSancionId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<PartidoJugador> PartidoJugadors { get; set; } = new List<PartidoJugador>();
}
