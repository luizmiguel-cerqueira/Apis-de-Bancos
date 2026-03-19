using api_para_banco.Domain.Enums;
namespace api_para_banco.model.DTO
{
    public class ResultadoOperacaoDTO
    {
        public TipoRetorno statusCode { get; set; }
        public List<string> resultados { get; set; }
    }
}
