using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class PartidoEstado
{
    public string PartidoEstadoId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
