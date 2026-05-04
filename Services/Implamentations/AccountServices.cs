using api_para_banco.Controllers;
using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;
using api_para_banco.model.EF;
using api_para_banco.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace api_para_banco.Services.Implamentations
{
    public class AccountServices(SistemaFinanceiroContext context) : IAccountServices
    {
        public async Task<bool> AccountExistsAsync(string account)
        {
            var exists = context.Clientes.Any(c => c.NumeroConta == account);
            return exists;
        }

        public async Task<TipoRetorno> DeposityAsync(string ToAccount, decimal quantity)
        {
            try
            {
                var exists = await AccountExistsAsync(ToAccount);
                if (!exists) return TipoRetorno.NaoEncontrado;

                var deposit = await context.Clientes.Where(c => c.NumeroConta == ToAccount).ExecuteUpdateAsync(
                    a => a.SetProperty(y => y.Saldo, y => y.Saldo + quantity));

                await context.SaveChangesAsync();

                return TipoRetorno.Sucesso;
            }
            catch (Exception ex)
            {
                return TipoRetorno.ErroInterno;
            }
        }

        public async Task<(TipoRetorno, decimal)> GetBalanceAsync(string account)
        {
            var exists = await AccountExistsAsync(account);
            if (!exists)
                return (TipoRetorno.NaoEncontrado, 0);
            var balance =  context.Clientes.FirstOrDefault(c => c.NumeroConta == account)?.Saldo ?? 0;

            return (TipoRetorno.Sucesso, balance);
        }

        public async Task<TipoRetorno> WithdrawalAsync(string ToAccount, decimal quantity)
        {
            try
            {

                var exists = await AccountExistsAsync(ToAccount);
                if (!exists) return TipoRetorno.NaoEncontrado;

                var withDraw = await context.Clientes.Where(c => c.NumeroConta == ToAccount && c.Saldo >= quantity).ExecuteUpdateAsync(
                    y => y.SetProperty(c => c.Saldo, c => c.Saldo - quantity));
                if (withDraw == 0) return TipoRetorno.Conflito;

                await context.SaveChangesAsync();

                return TipoRetorno.Sucesso;
            }
            catch (Exception ex)
            {
                return TipoRetorno.ErroInterno;
            }
        }
    }
}
