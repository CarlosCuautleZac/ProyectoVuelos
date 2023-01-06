using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AerolineaApi.Models
{
    public partial class sistem21_aerolineaContext : DbContext
    {
        public sistem21_aerolineaContext()
        {
        }

        public sistem21_aerolineaContext(DbContextOptions<sistem21_aerolineaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Observacion> Observacion { get; set; } = null!;
        public virtual DbSet<Vuelo> Vuelo { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_aerolinea;user=sistem21_aerolinea;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Observacion>(entity =>
            {
                entity.ToTable("observacion");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Observacion1)
                    .HasMaxLength(45)
                    .HasColumnName("observacion");
            });

            modelBuilder.Entity<Vuelo>(entity =>
            {
                entity.ToTable("vuelo");

                entity.HasIndex(e => e.Idobservacion, "fkVueloObservacion_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Aerolinea)
                    .HasMaxLength(45)
                    .HasColumnName("aerolinea");

                entity.Property(e => e.Destino)
                    .HasMaxLength(45)
                    .HasColumnName("destino");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_modificacion");

                entity.Property(e => e.Idobservacion)
                    .HasColumnType("int(11)")
                    .HasColumnName("idobservacion");

                entity.Property(e => e.Puerta)
                    .HasColumnType("int(11)")
                    .HasColumnName("puerta");

                entity.HasOne(d => d.IdobservacionNavigation)
                    .WithMany(p => p.Vuelo)
                    .HasForeignKey(d => d.Idobservacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkVueloObservacion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
