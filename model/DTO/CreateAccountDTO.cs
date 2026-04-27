namespace api_para_banco.model.DTO
{
    public class CreateAccountDTO
    {

        public string Nome { get; set; } = null!;

        public string Cpf { get; set; } = null!;

        public DateTime DataNascimento { get; set; }

        public string Telefone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Endereco { get; set; } = null!;

        public string Genero { get; set; } = null!;

        public string NumeroConta { get; set; } = null!;
        public string Senha { get; set; } = null!;


    }
}
