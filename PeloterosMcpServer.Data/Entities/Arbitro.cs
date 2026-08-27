using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Arbitro
{
    public int ArbitroId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
