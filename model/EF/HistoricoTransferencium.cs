using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_para_banco.model.EF;


public partial class HistoricoTransferencium
{
    
    public Guid IdTransferencia { get; set; }

    public decimal ValorTransferencia { get; set; }

    public DateTime DataTransferencia { get; set; }

    public string NumeroContaPagador { get; set; } = null!;

    public string NumeroContaBeneficiado { get; set; } = null!;

    //[ForeignKey(nameof(NumeroContaBeneficiado))]
    public virtual Cliente NumeroContaBeneficiadoNavigation { get; set; } = null!;

    //[ForeignKey(nameof(NumeroContaPagador))]
    public virtual Cliente NumeroContaPagadorNavigation { get; set; } = null!;
}
