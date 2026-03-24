using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api_para_banco.Infrastructure.model;

public partial class SistemaFinanceiroContext : DbContext
{
    SqlServerModel _sqlServerModel = new SqlServerModel();
    public SistemaFinanceiroContext(SqlServerModel sql)
    {
        _sqlServerModel = sql;
    }

    public SistemaFinanceiroContext(DbContextOptions<SistemaFinanceiroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContaCorrente> ContaCorrentes { get; set; }

    public virtual DbSet<ContaPoupanca> ContaPoupancas { get; set; }

    public virtual DbSet<HistoricoTransferencium> HistoricoTransferencia { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(_sqlServerModel.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContaCorrente>(entity =>
        {
            entity.HasKey(e => e.Cpf).HasName("PK__ContaCor__C1FF93081D69681A");

            entity.ToTable("ContaCorrente");

            entity.HasIndex(e => e.NumConta, "UQ__ContaCor__50CF69CE28343CA3").IsUnique();

            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dtnascimento).HasColumnName("DTNascimento");
            entity.Property(e => e.NumConta)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Saldo)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(15, 2)");
            entity.Property(e => e.Senha)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Titular)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ContaPoupanca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContaPou__3214EC271428B57A");

            entity.ToTable("ContaPoupanca");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Investidor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Saldo)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.CpfNavigation).WithMany(p => p.ContaPoupancas)
                .HasForeignKey(d => d.Cpf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ContaPoupan__Cpf__5165187F");
        });

        modelBuilder.Entity<HistoricoTransferencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__historic__3214EC27FDCBF2AF");

            entity.ToTable("historicoTransferencia");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CpfPagador)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.CpfRecebedor)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.DataHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Estatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MeioPagamento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.CpfPagadorNavigation).WithMany(p => p.HistoricoTransferenciumCpfPagadorNavigations)
                .HasForeignKey(d => d.CpfPagador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__historico__CpfPa__5535A963");

            entity.HasOne(d => d.CpfRecebedorNavigation).WithMany(p => p.HistoricoTransferenciumCpfRecebedorNavigations)
                .HasForeignKey(d => d.CpfRecebedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__historico__CpfRe__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("userID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
