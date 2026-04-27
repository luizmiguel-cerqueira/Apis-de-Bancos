using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_para_banco.model.EF;

public partial class Cofrinho
{
    [Key]
    public Guid IdCofre { get; set; }

    public int IdPessoa { get; set; }

    public decimal Saldo { get; set; }

    public DateOnly DataCriacao { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
