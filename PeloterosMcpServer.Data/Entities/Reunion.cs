using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Reunion
{
    public int ReunionId { get; set; }

    public DateTime FechaHora { get; set; }

    public string? Acta { get; set; }

    public int? CampeonatoId { get; set; }

    public virtual Campeonato? Campeonato { get; set; }

    public virtual ICollection<ReunionAsistencium> ReunionAsistencia { get; set; } = new List<ReunionAsistencium>();
}
