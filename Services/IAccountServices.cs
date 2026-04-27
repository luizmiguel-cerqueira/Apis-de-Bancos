using api_para_banco.Controllers;
using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;

namespace api_para_banco.Services
{
    public interface IAccountServices
    {
        public Task<TipoRetorno> WithdrawalAsync(string ToAccount, decimal quantity);
        public Task<TipoRetorno> DeposityAsync(string ToAccount, decimal quantity);
        public Task<bool> AccountExistsAsync(string account);
        public Task<(TipoRetorno, decimal)> GetBalanceAsync(string account);
    }
}
