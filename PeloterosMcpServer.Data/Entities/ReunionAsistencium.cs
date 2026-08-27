using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class ReunionAsistencium
{
    public int ReunionAsistenciaId { get; set; }

    public int ReunionId { get; set; }

    public int EquipoId { get; set; }

    public bool Asistencia { get; set; }

    public bool? Licencia { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Reunion Reunion { get; set; } = null!;
}
