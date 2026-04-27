using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_para_banco.model.EF;

public partial class Cliente
{
    [Key]
    public int IdCliente { get; set; }
    public string NumeroConta { get; set; } = null!;

    public decimal Saldo { get; set; }

    public bool Ativo { get; set; }

    public DateTime DataCriacao { get; set; }
    [InverseProperty(nameof(HistoricoTransferencium.NumeroContaBeneficiadoNavigation))]
    public virtual ICollection<HistoricoTransferencium> HistoricoTransferenciumNumeroContaBeneficiadoNavigations { get; set; } = new List<HistoricoTransferencium>();
    [InverseProperty(nameof(HistoricoTransferencium.NumeroContaPagadorNavigation))]
    public virtual ICollection<HistoricoTransferencium> HistoricoTransferenciumNumeroContaPagadorNavigations { get; set; } = new List<HistoricoTransferencium>();
    public virtual Pessoa IdClienteNavigation { get; set; } = null!;
}
