using api_para_banco.Domain.Enums;
namespace api_para_banco.Infrastructure.Data.DTO
{
    public class ResultadoOperacaoDTO
    {
        public TipoRetorno statusCode { get; set; }
        public List<string> resultados { get; set; }
    }
}
