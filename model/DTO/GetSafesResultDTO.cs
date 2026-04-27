using api_para_banco.Domain.Enums;

namespace api_para_banco.model.DTO
{
    public class GetSafesResultDTO
    {
        public TipoRetorno tipoRetorno { get; set; }
        public List<SafeDTO> safes { get; set; }
    }
}
