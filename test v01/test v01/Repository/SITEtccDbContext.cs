using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using test_v01.Repository.Models;

namespace test_v01.Repository;

public partial class SITEtccDbContext : DbContext
{
    public SITEtccDbContext()
    {
    }

    public SITEtccDbContext(DbContextOptions<SITEtccDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Localizacao> Localizacaos { get; set; }

    public virtual DbSet<PalavraChave> PalavraChaves { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => //optionsBuilder.UseSqlServer("Data Source=.\\SENAI;Initial Catalog=DATAtcc;User ID=sa;Password=senai.123;TrustServerCertificate=True");
             optionsBuilder.UseSqlServer(" Data Source=DESKTOP-TKBMO8Q;Initial Catalog = DATAtcc; Integrated Security = True; Trust Server Certificate=True");

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.Documentoid).HasName("PK__document__69A574B52B997C43");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Documentos).HasConstraintName("fk_usuario_documento");

            entity.HasMany(d => d.IdPalavraChaves).WithMany(p => p.IdDocumentos)
                .UsingEntity<Dictionary<string, object>>(
                    "DocumentoPalavraChave",
                    r => r.HasOne<PalavraChave>().WithMany()
                        .HasForeignKey("IdPalavraChave")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_palavra_chave"),
                    l => l.HasOne<Documento>().WithMany()
                        .HasForeignKey("IdDocumento")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_documento"),
                    j =>
                    {
                        j.HasKey("IdDocumento", "IdPalavraChave").HasName("PK__document__7AB466A67BC07A39");
                        j.ToTable("documento_palavra_chave");
                        j.IndexerProperty<int>("IdDocumento").HasColumnName("id_documento");
                        j.IndexerProperty<int>("IdPalavraChave").HasColumnName("id_palavra_chave");
                    });
        });

        modelBuilder.Entity<Localizacao>(entity =>
        {
            entity.HasKey(e => e.IdLocalizacao).HasName("PK__localiza__3EE0C968DBF23C8D");
        });

        modelBuilder.Entity<PalavraChave>(entity =>
        {
            entity.HasKey(e => e.IdPalavraChave).HasName("PK__palavra___79A814338B0DFD39");

            entity.HasMany(d => d.IdLocalizacaos).WithMany(p => p.IdPalavraChaves)
                .UsingEntity<Dictionary<string, object>>(
                    "PalavraChaveLocalizacao",
                    r => r.HasOne<Localizacao>().WithMany()
                        .HasForeignKey("IdLocalizacao")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_localizacao"),
                    l => l.HasOne<PalavraChave>().WithMany()
                        .HasForeignKey("IdPalavraChave")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_palavra_chave_localizacao"),
                    j =>
                    {
                        j.HasKey("IdPalavraChave", "IdLocalizacao").HasName("PK__palavra___EA4618A5BD4D02FD");
                        j.ToTable("palavra_chave_localizacao");
                        j.IndexerProperty<int>("IdPalavraChave").HasColumnName("id_palavra_chave");
                        j.IndexerProperty<int>("IdLocalizacao").HasColumnName("id_localizacao");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__usuario__080A9743E8FE2A70");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
