namespace api_para_banco.Aplication.Commands
{
    public class TransferenciaCommand
    {
        public string contaTitular { get; set; }
        public string contaFavorecido { get; set; }
        public decimal valor { get; set; }
    }
}
