using api_para_banco.Domain.Enums;
using api_para_banco.model.EF;

namespace api_para_banco.model.DTO
{
    public class GetTransfersResultDTO
    {
        public TipoRetorno TipoRetorno { get; set; }
        public List<HistoricoTransferencium> Transfers { get; set; }

    }
}
