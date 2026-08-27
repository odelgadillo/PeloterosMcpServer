using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class RegistroOperacion
{
    public long RegistroOperacionId { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public string Entidad { get; set; } = null!;

    public string Operacion { get; set; } = null!;

    public string? IdRegistro { get; set; }
}
