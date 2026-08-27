using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Fase
{
    public int FaseId { get; set; }

    public string Nombre { get; set; } = null!;

    public short Orden { get; set; }

    public virtual ICollection<Partido> Partidos { get; set; } = new List<Partido>();
}
