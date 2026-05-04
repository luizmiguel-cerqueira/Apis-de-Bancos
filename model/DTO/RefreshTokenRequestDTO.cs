namespace api_para_banco.model.DTO
{
    public class RefreshTokenRequestDTO
    {
        public int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
