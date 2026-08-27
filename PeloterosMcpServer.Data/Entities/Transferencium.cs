using System;
using System.Collections.Generic;

namespace PeloterosMcpServer.Data.Entities;

public partial class Transferencium
{
    public int TransferenciaId { get; set; }

    public int EquipoIdOrigen { get; set; }

    public int JugadorId { get; set; }

    public int EquipoIdDestino { get; set; }

    public string TransferenciaTipoId { get; set; } = null!;

    public short? Temporadas { get; set; }

    public string? DelegadoOrigen { get; set; }

    public string? DelegadoDestino { get; set; }

    public DateTime? Fecha { get; set; }

    public string TransferenciaEstadoId { get; set; } = null!;

    public virtual Equipo EquipoIdDestinoNavigation { get; set; } = null!;

    public virtual Equipo EquipoIdOrigenNavigation { get; set; } = null!;

    public virtual Jugador Jugador { get; set; } = null!;

    public virtual TransferenciaEstado TransferenciaEstado { get; set; } = null!;

    public virtual TransferenciaTipo TransferenciaTipo { get; set; } = null!;
}
