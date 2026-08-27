using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PeloterosMcpServer.Data.Entities;

namespace PeloterosMcpServer.Data.Context;

public partial class PeloterosDbContext : DbContext
{
    public PeloterosDbContext(DbContextOptions<PeloterosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Arbitro> Arbitros { get; set; }

    public virtual DbSet<Campeonato> Campeonatos { get; set; }

    public virtual DbSet<CampeonatoEquipo> CampeonatoEquipos { get; set; }

    public virtual DbSet<CampeonatoEstado> CampeonatoEstados { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<EquipoDelegado> EquipoDelegados { get; set; }

    public virtual DbSet<EquipoJugador> EquipoJugadors { get; set; }

    public virtual DbSet<Fase> Fases { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    public virtual DbSet<JugadorEstado> JugadorEstados { get; set; }

    public virtual DbSet<JugadorSancion> JugadorSancions { get; set; }

    public virtual DbSet<JugadorSustitucion> JugadorSustitucions { get; set; }

    public virtual DbSet<Partido> Partidos { get; set; }

    public virtual DbSet<PartidoEstado> PartidoEstados { get; set; }

    public virtual DbSet<PartidoJugador> PartidoJugadors { get; set; }

    public virtual DbSet<Posicion> Posicions { get; set; }

    public virtual DbSet<RegistroOperacion> RegistroOperacions { get; set; }

    public virtual DbSet<Reunion> Reunions { get; set; }

    public virtual DbSet<ReunionAsistencium> ReunionAsistencia { get; set; }

    public virtual DbSet<Sancion> Sancions { get; set; }

    public virtual DbSet<TransferenciaEstado> TransferenciaEstados { get; set; }

    public virtual DbSet<TransferenciaTipo> TransferenciaTipos { get; set; }

    public virtual DbSet<Transferencium> Transferencia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Arbitro>(entity =>
        {
            entity.HasKey(e => e.ArbitroId).HasName("PK__Arbitro__67970FE0184FD5F8");

            entity.ToTable("Arbitro");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Campeonato>(entity =>
        {
            entity.HasKey(e => e.CampeonatoId).HasName("PK__Campeona__020946814258CF0E");

            entity.ToTable("Campeonato");

            entity.Property(e => e.CampeonatoEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ColorCampeonato)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Presidente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Urlconvocatoria)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("URLConvocatoria");
            entity.Property(e => e.Urlreglamento)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("URLReglamento");
            entity.Property(e => e.Vicepresidente)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CampeonatoEstado).WithMany(p => p.Campeonatos)
                .HasForeignKey(d => d.CampeonatoEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Campeonato_CampeonatoEstado");
        });

        modelBuilder.Entity<CampeonatoEquipo>(entity =>
        {
            entity.HasKey(e => e.CampeonatoEquipoId).HasName("PK__Campeona__B2B5EA162221949D");

            entity.ToTable("CampeonatoEquipo");

            entity.Property(e => e.GrupoId)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.Campeonato).WithMany(p => p.CampeonatoEquipos)
                .HasForeignKey(d => d.CampeonatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CampeonatoEquipo_Campeonato");

            entity.HasOne(d => d.Equipo).WithMany(p => p.CampeonatoEquipos)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CampeonatoEquipo_Equipo");

            entity.HasOne(d => d.Grupo).WithMany(p => p.CampeonatoEquipos)
                .HasForeignKey(d => d.GrupoId)
                .HasConstraintName("FK_CampeonatoEquipo_Grupo");
        });

        modelBuilder.Entity<CampeonatoEstado>(entity =>
        {
            entity.HasKey(e => e.CampeonatoEstadoId).HasName("PK__Campeona__320DF2759C0FF448");

            entity.ToTable("CampeonatoEstado");

            entity.Property(e => e.CampeonatoEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.EquipoId).HasName("PK__tmp_ms_x__DE8A0BDFACCED14A");

            entity.ToTable("Equipo");

            entity.Property(e => e.Abreviatura)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Delegado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DelegadoCi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DelegadoCI");
            entity.Property(e => e.EscudoImagenNombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.GrupoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCorto)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Grupo).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.GrupoId)
                .HasConstraintName("FK_Equipo_Grupo");
        });

        modelBuilder.Entity<EquipoDelegado>(entity =>
        {
            entity.HasKey(e => e.EquipoDelegadoId).HasName("PK__EquipoDe__090FCA0043E89921");

            entity.Property(e => e.DelegadoCorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DelegadoNombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Equipo).WithMany(p => p.EquipoDelegados)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipoDelegados_Equipos");
        });

        modelBuilder.Entity<EquipoJugador>(entity =>
        {
            entity.HasKey(e => e.EquipoJugadorId).HasName("PK__EquipoJu__1217DA4347769EA5");

            entity.ToTable("EquipoJugador");

            entity.HasOne(d => d.Campeonato).WithMany(p => p.EquipoJugadors)
                .HasForeignKey(d => d.CampeonatoId)
                .HasConstraintName("FK_EquipoJugador_Campeonato");

            entity.HasOne(d => d.Equipo).WithMany(p => p.EquipoJugadors)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipoJugador_Equipo");

            entity.HasOne(d => d.Jugador).WithMany(p => p.EquipoJugadors)
                .HasForeignKey(d => d.JugadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquipoJugador_Jugador");
        });

        modelBuilder.Entity<Fase>(entity =>
        {
            entity.HasKey(e => e.FaseId).HasName("PK__Fase__D043487535DC162E");

            entity.ToTable("Fase");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.GrupoId).HasName("PK__Grupo__556BF040741D50EB");

            entity.ToTable("Grupo");

            entity.Property(e => e.GrupoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.JugadorId).HasName("PK__Jugador__4B5753A2C7F7973F");

            entity.ToTable("Jugador");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Apodo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ci)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CI");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.JugadorEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.JugadorImagenNombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Equipo).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.EquipoId)
                .HasConstraintName("FK_Jugador_Equipo");

            entity.HasOne(d => d.JugadorEstado).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.JugadorEstadoId)
                .HasConstraintName("FK_Jugador_JugadorEstado");

            entity.HasOne(d => d.Posicion).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.PosicionId)
                .HasConstraintName("FK_Jugador_Posicion");
        });

        modelBuilder.Entity<JugadorEstado>(entity =>
        {
            entity.HasKey(e => e.JugadorEstadoId).HasName("PK__JugadorE__AB24D7E625B80600");

            entity.ToTable("JugadorEstado");

            entity.Property(e => e.JugadorEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JugadorSancion>(entity =>
        {
            entity.HasKey(e => e.JugadorSancionId).HasName("PK__JugadorS__E5F28767C5D69C39");

            entity.ToTable("JugadorSancion");

            entity.Property(e => e.JugadorSancionId)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JugadorSustitucion>(entity =>
        {
            entity.HasKey(e => e.JugadorSustitucionId).HasName("PK__JugadorS__CE2ECC9957F295D9");

            entity.ToTable("JugadorSustitucion");

            entity.Property(e => e.JugadorSustitucionId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Partido>(entity =>
        {
            entity.HasKey(e => e.PartidoId).HasName("PK__Partido__DBC2E8B6CD4ABB3D");

            entity.ToTable("Partido");

            entity.Property(e => e.BalonEquipoA).HasDefaultValue(false);
            entity.Property(e => e.BalonEquipoB).HasDefaultValue(false);
            entity.Property(e => e.BanderaEquipoA).HasDefaultValue(false);
            entity.Property(e => e.BanderaEquipoB).HasDefaultValue(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.InformeArbitro)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.PartidoEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Penales).HasDefaultValue(false);
            entity.Property(e => e.PenalesEquipoA).HasDefaultValue((byte)0);
            entity.Property(e => e.PenalesEquipoB).HasDefaultValue((byte)0);
            entity.Property(e => e.TamarillaEquipoA)
                .HasDefaultValue(0)
                .HasColumnName("TAmarillaEquipoA");
            entity.Property(e => e.TamarillaEquipoB)
                .HasDefaultValue(0)
                .HasColumnName("TAmarillaEquipoB");
            entity.Property(e => e.TrojaEquipoA)
                .HasDefaultValue(0)
                .HasColumnName("TRojaEquipoA");
            entity.Property(e => e.TrojaEquipoB)
                .HasDefaultValue(0)
                .HasColumnName("TRojaEquipoB");
            entity.Property(e => e.Walkower).HasDefaultValue(false);

            entity.HasOne(d => d.Arbitro).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.ArbitroId)
                .HasConstraintName("FK_Partido_Arbitro");

            entity.HasOne(d => d.Campeonato).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.CampeonatoId)
                .HasConstraintName("FK_Partido_Campeonato");

            entity.HasOne(d => d.EquipoIdANavigation).WithMany(p => p.PartidoEquipoIdANavigations)
                .HasForeignKey(d => d.EquipoIdA)
                .HasConstraintName("FK_Partido_EquipoA");

            entity.HasOne(d => d.EquipoIdBNavigation).WithMany(p => p.PartidoEquipoIdBNavigations)
                .HasForeignKey(d => d.EquipoIdB)
                .HasConstraintName("FK_Partido_EquipoB");

            entity.HasOne(d => d.EquipoIdGanadorNavigation).WithMany(p => p.PartidoEquipoIdGanadorNavigations)
                .HasForeignKey(d => d.EquipoIdGanador)
                .HasConstraintName("FK_Partido_EquipoGanador");

            entity.HasOne(d => d.Fase).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.FaseId)
                .HasConstraintName("FK_Partido_Fase");

            entity.HasOne(d => d.PartidoEstado).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.PartidoEstadoId)
                .HasConstraintName("FK_Partido_PartidoEstado");
        });

        modelBuilder.Entity<PartidoEstado>(entity =>
        {
            entity.HasKey(e => e.PartidoEstadoId).HasName("PK__PartidoE__E3466E96340DA917");

            entity.ToTable("PartidoEstado");

            entity.Property(e => e.PartidoEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PartidoJugador>(entity =>
        {
            entity.HasKey(e => e.PartidoJugadorId).HasName("PK__PartidoJ__53277DAB5EC1F1AC");

            entity.ToTable("PartidoJugador");

            entity.Property(e => e.JugadorSancionId)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.JugadorSustitucionId)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.Equipo).WithMany(p => p.PartidoJugadors)
                .HasForeignKey(d => d.EquipoId)
                .HasConstraintName("FK_PartidoJugador_Equipo");

            entity.HasOne(d => d.Jugador).WithMany(p => p.PartidoJugadors)
                .HasForeignKey(d => d.JugadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartidoJugador_Jugador");

            entity.HasOne(d => d.JugadorSancion).WithMany(p => p.PartidoJugadors)
                .HasForeignKey(d => d.JugadorSancionId)
                .HasConstraintName("FK_PartidoJugador_JugadorSancion");

            entity.HasOne(d => d.JugadorSustitucion).WithMany(p => p.PartidoJugadors)
                .HasForeignKey(d => d.JugadorSustitucionId)
                .HasConstraintName("FK_PartidoJugador_JugadorSustitucion");

            entity.HasOne(d => d.Partido).WithMany(p => p.PartidoJugadors)
                .HasForeignKey(d => d.PartidoId)
                .HasConstraintName("FK_PartidoJugador_Partido");
        });

        modelBuilder.Entity<Posicion>(entity =>
        {
            entity.HasKey(e => e.PosicionId).HasName("PK__Posicion__FCC102D6E27012AE");

            entity.ToTable("Posicion");

            entity.Property(e => e.PosicionId).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RegistroOperacion>(entity =>
        {
            entity.HasKey(e => e.RegistroOperacionId).HasName("PK__Registro__B04704E2D3335BCD");

            entity.ToTable("RegistroOperacion");

            entity.Property(e => e.Entidad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.IdRegistro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Operacion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<Reunion>(entity =>
        {
            entity.HasKey(e => e.ReunionId).HasName("PK__Reunion__C0AD9DA3803580F9");

            entity.ToTable("Reunion");

            entity.Property(e => e.Acta).IsUnicode(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");

            entity.HasOne(d => d.Campeonato).WithMany(p => p.Reunions)
                .HasForeignKey(d => d.CampeonatoId)
                .HasConstraintName("FK_Reunion_Campeonato");
        });

        modelBuilder.Entity<ReunionAsistencium>(entity =>
        {
            entity.HasKey(e => e.ReunionAsistenciaId).HasName("PK__ReunionA__4A92619238367B12");

            entity.HasOne(d => d.Equipo).WithMany(p => p.ReunionAsistencia)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReunionAsistencia_Equipo");

            entity.HasOne(d => d.Reunion).WithMany(p => p.ReunionAsistencia)
                .HasForeignKey(d => d.ReunionId)
                .HasConstraintName("FK_ReunionAsistencia_Reunion");
        });

        modelBuilder.Entity<Sancion>(entity =>
        {
            entity.HasKey(e => e.SancionId).HasName("PK__Sancion__473A3633B5EF2164");

            entity.ToTable("Sancion");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Motivo).IsUnicode(false);

            entity.HasOne(d => d.Campeonato).WithMany(p => p.Sancions)
                .HasForeignKey(d => d.CampeonatoId)
                .HasConstraintName("FK_Sancion_Campeonato");

            entity.HasOne(d => d.Equipo).WithMany(p => p.Sancions)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sancion_Equipo");
        });

        modelBuilder.Entity<TransferenciaEstado>(entity =>
        {
            entity.HasKey(e => e.TransferenciaEstadoId).HasName("PK__Transfer__11959B8FBDED9BE7");

            entity.ToTable("TransferenciaEstado");

            entity.Property(e => e.TransferenciaEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransferenciaTipo>(entity =>
        {
            entity.HasKey(e => e.TransferenciaTipoId).HasName("PK__Transfer__B1E8914F883DF0F3");

            entity.ToTable("TransferenciaTipo");

            entity.Property(e => e.TransferenciaTipoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transferencium>(entity =>
        {
            entity.HasKey(e => e.TransferenciaId).HasName("PK__Transfer__E5B4F5D248262B03");

            entity.Property(e => e.DelegadoDestino)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DelegadoOrigen)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.TransferenciaEstadoId)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TransferenciaTipoId)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.EquipoIdDestinoNavigation).WithMany(p => p.TransferenciumEquipoIdDestinoNavigations)
                .HasForeignKey(d => d.EquipoIdDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transferencia_EquipoDestino");

            entity.HasOne(d => d.EquipoIdOrigenNavigation).WithMany(p => p.TransferenciumEquipoIdOrigenNavigations)
                .HasForeignKey(d => d.EquipoIdOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transferencia_EquipoOrigen");

            entity.HasOne(d => d.Jugador).WithMany(p => p.Transferencia)
                .HasForeignKey(d => d.JugadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transferencia_Jugador");

            entity.HasOne(d => d.TransferenciaEstado).WithMany(p => p.Transferencia)
                .HasForeignKey(d => d.TransferenciaEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transferencia_TransferenciaEstado");

            entity.HasOne(d => d.TransferenciaTipo).WithMany(p => p.Transferencia)
                .HasForeignKey(d => d.TransferenciaTipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transferencia_TransferenciaTipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
