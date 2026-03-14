namespace api_para_banco.model;
using System.ComponentModel.DataAnnotations;

public partial class HistoricoTransferencium
{
    [Key]
    public int Id { get; set; }

    public string CpfPagador { get; set; } = null!;

    public string CpfRecebedor { get; set; } = null!;

    public decimal Valor { get; set; }

    public DateTime? DataHora { get; set; }

    public string MeioPagamento { get; set; } = null!;

    public string Estatus { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string? Descricao { get; set; }

    public string Status { get; set; } = null!;

    public virtual ContaCorrente CpfPagadorNavigation { get; set; } = null!;

    public virtual ContaCorrente CpfRecebedorNavigation { get; set; } = null!;
}
