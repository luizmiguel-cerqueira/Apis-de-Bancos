using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace api_para_banco.model.EF;

[Keyless]
public partial class PessoaPermissao
{
    public int IdPermissao { get; set; }

    public int IdPessoa { get; set; }

    public virtual Permisso IdPermissaoNavigation { get; set; } = null!;

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;
}
