using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;

namespace api_para_banco.Services.Interfaces
{
    public interface IAccountManagerServices
    {
        public Task<TipoRetorno> CreateAccount(CreateAccountDTO request);

        public Task<TipoRetorno> DeleteAccount(string numeroConta);
    }
}
