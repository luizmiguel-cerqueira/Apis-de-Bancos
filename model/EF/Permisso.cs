using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_para_banco.model.EF;

public partial class Permisso
{
    [Key]
    public int IdPermissao { get; set; }

    public string Descricao { get; set; } = null!;
}
