namespace api_para_banco.model.DTO
{
    public class TokenResponseDTO
    {
        public required string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
