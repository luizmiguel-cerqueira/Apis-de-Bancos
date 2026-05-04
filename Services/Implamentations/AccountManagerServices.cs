using api_para_banco.Domain.Enums;
using api_para_banco.model;
using api_para_banco.model.DTO;
using api_para_banco.model.EF;
using api_para_banco.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_para_banco.Services.Implamentations
{
    public class AccountManagerServices(SistemaFinanceiroContext context, IAccountServices accountServices) : IAccountManagerServices
    {
        public async Task<TipoRetorno> CreateAccount(CreateAccountDTO request)
        {
            var transaction = await context.Database.BeginTransactionAsync();

            var exists = accountServices.AccountExistsAsync(request.NumeroConta).Result;
            if (exists) return TipoRetorno.Conflito;

            var createPessoa = await context.Pessoas.AddAsync(new Pessoa 
            {
                Nome = request.Nome,
                Cpf = request.Cpf.Trim(),
                Data_Nascimento = request.DataNascimento,
                Telefone = request.Telefone,
                Email = request.Email,
                Senha = request.Senha,
                Endereco = request.Endereco,
                Genero = request.Genero,

            });
            if (createPessoa is null) 
            {
                await transaction.RollbackAsync();
                return TipoRetorno.ErroInterno; 
            }
            await context.SaveChangesAsync();
            var id =  context.Pessoas.Where(p => p.Cpf == request.Cpf.Trim()).Select(p => p.IdPessoa).FirstOrDefault();

            var createAccount = await context.Clientes.AddAsync(new Cliente
            {
                IdCliente = id,
                NumeroConta = request.NumeroConta,
                DataCriacao = DateTime.Now,
            });
            if (createAccount is null) 
            {
                await transaction.RollbackAsync();
                return TipoRetorno.ErroInterno;
            }

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return TipoRetorno.Sucesso;
        }
        public async Task<TipoRetorno> DeleteAccount(string numeroConta) 
        {
            var exists = await accountServices.AccountExistsAsync(numeroConta);
            if (!exists) return TipoRetorno.NaoEncontrado;

            int id = await context.Clientes.Where(c => c.NumeroConta == numeroConta).Select(c => c.IdCliente).FirstOrDefaultAsync();
            var transaction = await context.Database.BeginTransactionAsync();
            
            var deleteAccount = await context.Clientes.Where(c => c.NumeroConta == numeroConta).ExecuteDeleteAsync();

            var deletePessoa = await context.Pessoas.Where(p => p.IdPessoa == id).ExecuteDeleteAsync();

            context.SaveChanges();
            await transaction.CommitAsync();
            return TipoRetorno.Sucesso;
        }
    }
}
