using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;
using api_para_banco.model.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace api_para_banco.Services
{
    public class TransferServices(SistemaFinanceiroContext context, IAccountServices accountServices) : ITransferServices
    {
        public async Task<TipoRetorno> AddTransferToHistoric(TransferDTO request)
        {

           var entity = await context.HistoricoTransferencia.AddAsync(new HistoricoTransferencium
           {
                DataTransferencia = DateTime.Now,
                ValorTransferencia = request.quantity,
                NumeroContaBeneficiado = request.ToAccount,
                NumeroContaPagador = request.FromAccount,
                IdTransferencia = Guid.NewGuid()
           });
            if (entity is null)
                return TipoRetorno.ErroInterno;
            try
            {
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return TipoRetorno.ErroInterno;
            }

            return TipoRetorno.Sucesso;
        }

        public async Task<GetTransfersResultDTO> GetTransferByIdAsync(Guid id)
        {
            var existId =  context.HistoricoTransferencia.Any(x => x.IdTransferencia == id);
            if (!existId) return new GetTransfersResultDTO
            {
                TipoRetorno = TipoRetorno.NaoEncontrado,
                Transfers = new List<HistoricoTransferencium>()
            };

            var transfers = await context.HistoricoTransferencia.Where(x => x.IdTransferencia == id).ToListAsync();

            return new GetTransfersResultDTO
            {
                TipoRetorno = TipoRetorno.Sucesso,
                Transfers = transfers
            };
        }

        public async Task<GetTransfersResultDTO> GetTransfersByAccountAsync(string numeroConta)
        {
            var existAccount = await accountServices.AccountExistsAsync(numeroConta);
            if (!existAccount) return new GetTransfersResultDTO
            {
                TipoRetorno = TipoRetorno.NaoEncontrado,
                Transfers = new List<HistoricoTransferencium>()
            };

            var transfers = await context.HistoricoTransferencia.Where(c => c.NumeroContaPagador == numeroConta || c.NumeroContaBeneficiado == numeroConta).ToListAsync();

            return new GetTransfersResultDTO 
            {
                TipoRetorno = TipoRetorno.Sucesso,
                Transfers = transfers
            };
        }

        public async Task<TipoRetorno> TransferAsnyc(TransferDTO request)
        {
            //dps refatorar para usar o deposito e o withdrawl.
            try
            {
                using var transaction = await context.Database.BeginTransactionAsync();

                var WithdrawalResult = await accountServices.WithdrawalAsync(request.FromAccount, request.quantity);
                if (WithdrawalResult != TipoRetorno.Sucesso)
                {
                    await transaction.RollbackAsync();
                    return WithdrawalResult;
                }

                var DepositResult = await accountServices.DeposityAsync(request.ToAccount, request.quantity);
                if (DepositResult != TipoRetorno.Sucesso)
                {
                    await transaction.RollbackAsync();
                    return DepositResult;
                }
                var addHistoric = await AddTransferToHistoric(request);
                if (addHistoric != TipoRetorno.Sucesso)
                {
                    await transaction.RollbackAsync();
                    return addHistoric;
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return TipoRetorno.Sucesso;

            }
            catch (Exception ex)
            {
                return TipoRetorno.ErroInterno;
            }
        }

    }
}