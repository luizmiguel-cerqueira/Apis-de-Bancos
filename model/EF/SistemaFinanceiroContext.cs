using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api_para_banco.model.EF;

public partial class SistemaFinanceiroContext : DbContext
{
    public SistemaFinanceiroContext()
    {
    }

    public SistemaFinanceiroContext(DbContextOptions<SistemaFinanceiroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cofrinho> Cofrinhos { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<HistoricoTransferencium> HistoricoTransferencia { get; set; }

    public virtual DbSet<Permisso> Permissoes { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<PessoaPermissao> PessoaPermissaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("StrCon");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D594664269D7AE3E");


            //entity.HasIndex(e => e.NumeroConta, "UQ__Clientes__C551C75C28835933").IsUnique();

            entity.Property(e => e.IdCliente).ValueGeneratedNever();
            entity.Property(e => e.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ATIVO");
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroConta)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Saldo).HasColumnType("decimal(18, 0)");

            //entity.HasOne(d => d.IdClienteNavigation).WithOne(p => p.Cliente)
            //    .HasForeignKey<Cliente>(d => d.IdCliente)
            //    .HasPrincipalKey<Pessoa>(p=> p.IdPessoa)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_IdCliente_Cliente");
            entity.HasOne(c => c.IdClienteNavigation)
            .WithOne()
            .HasForeignKey<Cliente>(c => c.IdCliente)
            .HasPrincipalKey<Pessoa>(p => p.IdPessoa);

            entity.HasAlternateKey(c => c.NumeroConta);
        });

        modelBuilder.Entity<Cofrinho>(entity =>
        {
            entity.HasKey(e => e.IdCofre).HasName("PK__Cofrinho__E885DB823CD9407B");

            entity.ToTable("Cofrinho");

            entity.Property(e => e.IdCofre).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataCriacao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Saldo).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany(p => p.Cofrinhos)
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdPessoa_Cofrinho");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.IdFuncionario).HasName("PK__Funciona__35CB052AA8249C45");

            entity.Property(e => e.IdFuncionario).ValueGeneratedNever();
            entity.Property(e => e.Cargo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DataAdimissao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentoAdimissao)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Salario)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SALARIO");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(d => d.IdFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdFuncionario");
        });

        modelBuilder.Entity<HistoricoTransferencium>(entity =>
        {
            entity.HasKey(e => e.IdTransferencia).HasName("PK__Historic__6E5969EC3E24A8CF");

            entity.Property(e => e.IdTransferencia).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataTransferencia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NumeroContaBeneficiado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("numeroContaBeneficiado");
            entity.Property(e => e.NumeroContaPagador)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("numeroContaPagador");
            entity.Property(e => e.ValorTransferencia).HasColumnType("decimal(18, 0)");

            entity.HasOne(h => h.NumeroContaBeneficiadoNavigation)
                .WithMany(c => c.HistoricoTransferenciumNumeroContaBeneficiadoNavigations)
                .HasForeignKey(h => h.NumeroContaBeneficiado)
                .HasPrincipalKey(c => c.NumeroConta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_numeroConta_Beneficiado");

            entity.HasOne(h => h.NumeroContaPagadorNavigation)
                .WithMany(c => c.HistoricoTransferenciumNumeroContaPagadorNavigations)
                .HasForeignKey(h => h.NumeroContaPagador)
                .HasPrincipalKey(c => c.NumeroConta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_numeroConta_Pagador");

            //entity.HasOne(d => d.NumeroContaPagadorNavigation).WithMany(p => p.HistoricoTransferenciumNumeroContaPagadorNavigations)
            //    .HasPrincipalKey(p => p.NumeroConta)
            //    .HasForeignKey(d => d.NumeroContaPagador)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_numeroContaPagador");
        });

        modelBuilder.Entity<Permisso>(entity =>
        {
            entity.HasKey(e => e.IdPermissao).HasName("PK__Permisso__356F319AD89EB29B");

            entity.Property(e => e.Descricao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.IdPessoa).HasName("PK__Pessoa__7061465D08F08711");

            entity.ToTable("Pessoa");

            entity.HasIndex(e => e.Cpf, "UQ__Pessoa__C1F89731B35C829C").IsUnique();

            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Data_Nascimento)
                .HasColumnType("datetime")
                .HasColumnName("Data_Nascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Endereco)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Genero)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken).IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(14)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PessoaPermissao>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PessoaPermissao");

            entity.HasOne(d => d.IdPermissaoNavigation).WithMany()
                .HasForeignKey(d => d.IdPermissao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdPermissao");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany()
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdPessoa");
        });

        //OnModelCreatingPartial(modelBuilder);
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
