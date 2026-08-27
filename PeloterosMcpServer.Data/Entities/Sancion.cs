using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Sancion
{
    public int SancionId { get; set; }

    public int EquipoId { get; set; }

    public DateTime Fecha { get; set; }

    public string Motivo { get; set; } = null!;

    public int Puntos { get; set; }

    public int? CampeonatoId { get; set; }

    public virtual Campeonato? Campeonato { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;
}
