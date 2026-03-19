using api_para_banco.Domain.Enums;
using api_para_banco.model;
using api_para_banco.model.DTO;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.EntityFrameworkCore;
namespace api_para_banco.Services
{
    public class Utilidade
    {
        private readonly EntityFrameWorkModel _context;

        public Utilidade(EntityFrameWorkModel context)
        {
            _context = context;
        }
        public async Task<VerSaldoDTO> VerSaldo(string titular)
        {
            try
            {
                var Contacorrente = _context.ContaCorrente.FirstOrDefault(x => x.Titular == titular);
                if (Contacorrente != null)
                    return new VerSaldoDTO() {retorno = TipoRetorno.Sucesso , valor = int.Parse((Contacorrente.Saldo * 1000).ToString()) };

                return new VerSaldoDTO() { retorno = TipoRetorno.NaoEncontrado, valor = 0 };

            }
            catch
            {
                return new VerSaldoDTO() { retorno = TipoRetorno.ErroInterno, valor = 0};
            }
        }
        public async Task<TipoRetorno> Tranferenciabancaria(string titular, string contaBeneficiada, decimal quantia)
        {
            try 
            { 
            
                using var transaction = await _context.Database.BeginTransactionAsync();

                var ContaTitular = _context.ContaCorrente.Where(x => x.Titular == titular && x.Saldo >= quantia)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - quantia));
                if (ContaTitular == 0)
                {
                    var ContaTitularExistente = _context.ContaCorrente.Any(x => x.Titular == titular);
                    transaction.Rollback();

                    if (!ContaTitularExistente)
                        return TipoRetorno.NaoEncontrado;

                    return TipoRetorno.Conflito;

                }

                var ContaBeneficiada = _context.ContaCorrente.Where(y => y.Titular == contaBeneficiada)
                    .ExecuteUpdate(s => s.SetProperty(e => e.Saldo, e => e.Saldo + quantia));
                if (ContaBeneficiada == 0)
                {
                    var ContabeneficiadaExistente = _context.ContaCorrente.Any(x => x.Titular == contaBeneficiada);
                    transaction.Rollback();
                    if(!ContabeneficiadaExistente)
                        return TipoRetorno.NaoEncontrado;
                    return TipoRetorno.Conflito;
                }
                
                _context.SaveChanges();
                transaction.Commit();
                return TipoRetorno.Conflito;
            } 
            
            catch
            {
                return TipoRetorno.ErroInterno;
            }
        }
        public async Task<TipoRetorno> ColocarNaCaixinha(string cpf, decimal saldo)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var ContaTitular = _context.ContaCorrente.Where(x => x.Cpf == cpf && x.Saldo >= saldo)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - saldo));
                if (ContaTitular == 0)
                {   
                    var ContaTitularExistente = _context.ContaCorrente.Any(x => x.Cpf == cpf);
                    transaction.Rollback();
                    if(!ContaTitularExistente)
                        return TipoRetorno.NaoEncontrado;
                    return TipoRetorno.Conflito;
                }
                var ContaPoupanca = _context.ContaPoupanca.Where(y => y.Cpf == cpf)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo + saldo));
                if (ContaPoupanca == 0)
                {
                    var ContaPoupancaExistente = _context.ContaPoupanca.Any(x => x.Cpf == cpf);
                    transaction.Rollback();
                    if (!ContaPoupancaExistente)
                        return TipoRetorno.NaoEncontrado;
                    return TipoRetorno.Conflito;
                }
                _context.SaveChanges();
                transaction.Commit();

                return TipoRetorno.Sucesso;
            }
            catch 
            {
                return TipoRetorno.ErroInterno;
            }
        }
        public async Task<TipoRetorno> RetirarDaCaixinha(string cpf, decimal saldo)
        {
            try
            { 
                using var transaction = await _context.Database.BeginTransactionAsync();
                var ContaPoupanca = _context.ContaPoupanca.Where(x => x.Cpf == cpf && x.Saldo >= saldo)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - saldo));
                if (ContaPoupanca == 0)
                {   
                    var ContaPoupancaExistente = _context.ContaPoupanca.Any(x => x.Cpf == cpf);
                    transaction.Rollback();
                    
                    if (!ContaPoupancaExistente)
                        return TipoRetorno.NaoEncontrado;
                    return TipoRetorno.Conflito;
                }
                var ContaCorrente = _context.ContaCorrente.Where(y => y.Cpf == cpf)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo + saldo));
                if (ContaCorrente == 0)
                {
                    var ContaTitularExistente = _context.ContaCorrente.Any(x => x.Cpf == cpf);
                    transaction.Rollback();

                    if (!ContaTitularExistente)
                        return TipoRetorno.NaoEncontrado;
                    return TipoRetorno.Conflito;
                }
                _context.SaveChanges();
                transaction.Commit();
                return TipoRetorno.Sucesso;
            }
            catch 
            {
                return TipoRetorno.ErroInterno;
            }
        }

    }
}
