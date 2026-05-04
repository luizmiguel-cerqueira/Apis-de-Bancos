using api_para_banco.model.DTO;
using api_para_banco.model.EF;
using api_para_banco.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api_para_banco.Services.Implamentations
{
    public class AuthorizationServices(SistemaFinanceiroContext context, IConfiguration configuration) : IAuthorizationServices
    {
        public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
        {
            var user = context.Pessoas.Where(c => c.Nome == request.username && c.Senha == request.password).FirstOrDefault();
            if (user == null) return null;

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDTO?> CreateTokenResponse(Pessoa user)
        {
            return new TokenResponseDTO
            {
                AccessToken = await CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
        private async Task<Pessoa?> ValidateRefreshTokenAsync(int id, string refreshToken)
        {
            var user = await context.Pessoas.FindAsync(id);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow) return null;
            return user;
        }
        public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null) return null;

            return await CreateTokenResponse(user);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(Pessoa user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<string> CreateToken(Pessoa user)
        {
            List<string> roles = await GetPermissao(user.IdPessoa);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.NameIdentifier, user.IdPessoa.ToString()),
                //new Claim(ClaimTypes.Role, roles)
            };
            foreach (string role in roles)
            { 
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("JWT:Issuer"),
                audience: configuration.GetValue<string>("JWT:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<string>> GetPermissao(int user)
        {
            var roles = await context.PessoaPermissaos.Where(c => c.IdPessoa == user).Join(context.Permissoes,
                            p => p.IdPermissao,
                            r => r.IdPermissao,
                            (p, r) => r.Descricao).ToListAsync();

            return roles;
        }

    }
}
