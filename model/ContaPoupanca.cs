using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_para_banco.model;

public partial class ContaPoupanca
{
    [Key]
    public int Id { get; set; }
    public string Cpf { get; set; } = null!;

    public string Investidor { get; set; } = null!;

    public decimal? Saldo { get; set; }

    public DateTime? DataCriacao { get; set; }

    public virtual ContaCorrente CpfNavigation { get; set; } = null!;
}
