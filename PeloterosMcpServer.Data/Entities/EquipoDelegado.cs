using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class EquipoDelegado
{
    public int EquipoDelegadoId { get; set; }

    public int EquipoId { get; set; }

    public string DelegadoNombre { get; set; } = null!;

    public string DelegadoCorreoElectronico { get; set; } = null!;

    public virtual Equipo Equipo { get; set; } = null!;
}
