using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class TransferenciaTipo
{
    public string TransferenciaTipoId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Transferencium> Transferencia { get; set; } = new List<Transferencium>();
}
