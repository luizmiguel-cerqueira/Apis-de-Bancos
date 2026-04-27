using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_para_banco.model.EF;

public partial class Pessoa
{
    [Key]
    public int IdPessoa { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public DateTime Data_Nascimento { get; set; }

    public string Telefone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Endereco { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string? Senha { get; set; }

    //public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<Cofrinho> Cofrinhos { get; set; } = new List<Cofrinho>();

    public virtual Funcionario? Funcionario { get; set; }
}
