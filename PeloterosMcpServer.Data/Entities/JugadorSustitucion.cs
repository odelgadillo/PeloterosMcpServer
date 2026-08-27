using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class JugadorSustitucion
{
    public string JugadorSustitucionId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<PartidoJugador> PartidoJugadors { get; set; } = new List<PartidoJugador>();
}
