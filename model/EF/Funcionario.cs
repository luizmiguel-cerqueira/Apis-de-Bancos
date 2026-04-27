using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_para_banco.model.EF;

public partial class Funcionario
{
    [Key]
    public int IdFuncionario { get; set; }

    public string Cargo { get; set; } = null!;

    public bool Tipo { get; set; }

    public string? DocumentoAdimissao { get; set; }

    public DateTime? DataAdimissao { get; set; }

    public decimal Salario { get; set; }
    [ForeignKey(nameof(IdFuncionario))]
    public virtual Pessoa IdFuncionarioNavigation { get; set; } = null!;
}
