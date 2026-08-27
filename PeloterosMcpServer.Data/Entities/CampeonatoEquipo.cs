using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class CampeonatoEquipo
{
    public int CampeonatoEquipoId { get; set; }

    public int CampeonatoId { get; set; }

    public int EquipoId { get; set; }

    public string? GrupoId { get; set; }

    public virtual Campeonato Campeonato { get; set; } = null!;

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Grupo? Grupo { get; set; }
}
