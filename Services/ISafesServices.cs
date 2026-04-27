using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;

namespace api_para_banco.Services
{
    public interface ISafesServices
    {
        public Task<bool> ExistSafeAsync(string name);
        public Task<GetSafesResultDTO> GetAllSafesAsync(int id);
        public Task<GetSafesResultDTO> GetExpecificSafeAsync(string name);
        public Task<TipoRetorno> DepositInSavingsAsync(TransferDTO request);
        public Task<TipoRetorno> WithdrawFromSavingsAsync(TransferDTO request);
    }
}
