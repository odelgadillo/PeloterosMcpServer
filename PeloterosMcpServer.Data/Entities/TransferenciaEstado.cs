using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class TransferenciaEstado
{
    public string TransferenciaEstadoId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Transferencium> Transferencia { get; set; } = new List<Transferencium>();
}
