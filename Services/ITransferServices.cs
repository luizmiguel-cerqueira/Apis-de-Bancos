using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;

namespace api_para_banco.Services
{
    public interface ITransferServices
    {
        public Task<TipoRetorno> TransferAsnyc(TransferDTO request);
        public Task<TipoRetorno> AddTransferToHistoric(TransferDTO request);
        public Task<GetTransfersResultDTO> GetTransfersByAccountAsync(string numeroConta);
        public Task<GetTransfersResultDTO> GetTransferByIdAsync(Guid id);
    }
}
