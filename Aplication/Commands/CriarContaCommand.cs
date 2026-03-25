namespace api_para_banco.Aplication.Commands
{
    public class CriarContaCommand
    {
        public string titular { get; set; }
        public string senha { get; set; }
        public string cpf { get; set; }
        public string numConta { get; set; } 
        public DateOnly dataNascimento { get; set; }
        public Boolean Ativa { get; set; } = true;
    }
}
