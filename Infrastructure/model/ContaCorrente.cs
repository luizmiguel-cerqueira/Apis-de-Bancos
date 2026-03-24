using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_para_banco.Infrastructure.model;

public partial class ContaCorrente
{
    public decimal Saldo { get; set; }

    [Key]
    public string Cpf { get; set; } = null!;

    public string Titular { get; set; } = null!;

    public string NumConta { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public DateOnly Dtnascimento { get; set; }

    public DateTime? DataCriacao { get; set; }

    public bool? Ativo { get; set; }

    [NotMapped]
    public virtual ICollection<ContaPoupanca> ContaPoupancas { get; set; } = new List<ContaPoupanca>();
    
    [NotMapped]
    public virtual ICollection<HistoricoTransferencium> HistoricoTransferenciumCpfPagadorNavigations { get; set; } = new List<HistoricoTransferencium>();
    
    [NotMapped]
    public virtual ICollection<HistoricoTransferencium> HistoricoTransferenciumCpfRecebedorNavigations { get; set; } = new List<HistoricoTransferencium>();
}
