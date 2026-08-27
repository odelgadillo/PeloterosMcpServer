using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class CampeonatoEstado
{
    public string CampeonatoEstadoId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Campeonato> Campeonatos { get; set; } = new List<Campeonato>();
}
