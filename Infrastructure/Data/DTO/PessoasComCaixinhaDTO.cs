using api_para_banco.Infrastructure.model;

namespace api_para_banco.Infrastructure.Data.DTO
{
    public class PessoaComCaixinhaDTO
    {
        public ContaPoupanca ContaPoupanca { get; set; }
        public ContaCorrente ContaCorrente { get; set; }
    }

}
