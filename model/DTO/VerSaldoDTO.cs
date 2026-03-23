using api_para_banco.Domain.Enums;

namespace api_para_banco.model.DTO
{
    public class VerSaldoDTO
    {
        public TipoRetorno retorno { get; set; }
        public decimal valor { get; set; }
    }
}
