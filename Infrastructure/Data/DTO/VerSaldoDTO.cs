using api_para_banco.Domain.Enums;

namespace api_para_banco.Infrastructure.Data.DTO
{
    public class VerSaldoDTO
    {
        public TipoRetorno retorno { get; set; }
        public decimal valor { get; set; }
    }
}
