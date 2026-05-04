using api_para_banco.model.DTO;

namespace api_para_banco.Services.Interfaces
{
    public interface IAuthorizationServices
    {
        Task<TokenResponseDTO?> LoginAsync(UserDTO request);

        Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
    }
}
