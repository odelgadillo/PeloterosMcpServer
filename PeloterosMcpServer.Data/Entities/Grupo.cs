using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Grupo
{
    public string GrupoId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<CampeonatoEquipo> CampeonatoEquipos { get; set; } = new List<CampeonatoEquipo>();

    public virtual ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}
