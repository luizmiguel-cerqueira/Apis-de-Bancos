using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;
using api_para_banco.model.EF;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace api_para_banco.Services
{
    public class SafesServices(IAccountServices accountServices, SistemaFinanceiroContext context): ISafesServices
    {
        public async Task<bool> ExistSafeAsync(string name)
        {
            var exists = await context.Cofrinhos.AnyAsync(c => c.Nome == name);
            if (!exists) return false;

            return true;
        }

        public async Task<TipoRetorno> DepositInSavingsAsync(TransferDTO request)
        {
            try
            {
                var exists = await accountServices.AccountExistsAsync(request.FromAccount);
                var existsSafe = await ExistSafeAsync(request.ToAccount);

                if (!exists || !existsSafe) return TipoRetorno.NaoEncontrado;

                var withDraw = await context.Clientes.Where(c => c.NumeroConta == request.FromAccount && c.Saldo >= request.quantity).ExecuteUpdateAsync(
                    y => y.SetProperty(c => c.Saldo, c => c.Saldo - request.quantity));
                if (withDraw == 0) return TipoRetorno.Conflito;

                var deposit = await context.Cofrinhos.Where(c => c.Nome == request.ToAccount).ExecuteUpdateAsync(
                    y => y.SetProperty(c => c.Saldo, c => c.Saldo + request.quantity));

                if (deposit == 0) return TipoRetorno.Conflito;



                return TipoRetorno.Sucesso;

            }
            catch (Exception ex)
            {
                return TipoRetorno.ErroInterno;
            }
        }

        public async Task<TipoRetorno> WithdrawFromSavingsAsync(TransferDTO request)
        {
            try
            {
                var existsSafe = await ExistSafeAsync(request.FromAccount);
                var exists = await accountServices.AccountExistsAsync(request.ToAccount);
                if (!exists || !existsSafe) return TipoRetorno.NaoEncontrado;

                var withDraw = await context.Cofrinhos.Where(c => c.Nome == request.FromAccount && c.Saldo >= request.quantity).ExecuteUpdateAsync(
                    y => y.SetProperty(c => c.Saldo, c => c.Saldo - request.quantity));
                if (withDraw == 0) return TipoRetorno.Conflito;

                var deposit = await context.Clientes.Where(c => c.NumeroConta == request.ToAccount).ExecuteUpdateAsync(
                    y => y.SetProperty(c => c.Saldo, c => c.Saldo + request.quantity));

                if (deposit == 0) return TipoRetorno.Conflito;

                return TipoRetorno.Sucesso;

            }
            catch (Exception ex)
            {
                return TipoRetorno.ErroInterno;
            }

        }

        public async Task<GetSafesResultDTO> GetAllSafesAsync(int id)
        {
            var safes = await context.Cofrinhos.Where(c=> c.IdPessoa == id).Select(y => new SafeDTO { Nome = y.Nome, Saldo = y.Saldo }).ToListAsync();
            if (safes is null) return new GetSafesResultDTO { tipoRetorno = TipoRetorno.NaoEncontrado };

            return new GetSafesResultDTO { tipoRetorno = TipoRetorno.Sucesso, safes = safes };
        }

        public async Task<GetSafesResultDTO> GetExpecificSafeAsync(string name)
        {
            var safe = await context.Cofrinhos.Where(c => c.Nome == name).Select(y => new SafeDTO { Nome = y.Nome, Saldo = y.Saldo }).FirstAsync();
            if (safe is null) return new GetSafesResultDTO { tipoRetorno = TipoRetorno.NaoEncontrado };

            return new GetSafesResultDTO { tipoRetorno = TipoRetorno.Sucesso, safes = new List<SafeDTO> { safe } };
        }
    }
}
