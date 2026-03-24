using api_para_banco.Aplication.Commands;
using api_para_banco.Domain.Enums;
using api_para_banco.Infrastructure.Data.DTO;
using api_para_banco.Infrastructure.model;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
namespace api_para_banco.Aplication.Services
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
                    return new VerSaldoDTO() { retorno = TipoRetorno.Sucesso, valor = Contacorrente.Saldo};

                return new VerSaldoDTO() { retorno = TipoRetorno.NaoEncontrado, valor = 0 };

            }
            catch
            {
                return new VerSaldoDTO() { retorno = TipoRetorno.ErroInterno, valor = 0 };
            }
        }
        public async Task<TipoRetorno> Tranferenciabancaria(TransferenciaCommand command)
        {
            try 
            { 
            
                using var transaction = await _context.Database.BeginTransactionAsync();

                var ContaTitular = _context.ContaCorrente.Where(x => x.NumConta == command.contaTitular && x.Saldo >= command.valor)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - command.valor));
                if (ContaTitular == 0)
                {
                    var ContaTitularExistente = _context.ContaCorrente.Any(x => x.Titular == command.contaTitular);
                    transaction.Rollback();

                    if (!ContaTitularExistente)
                        return TipoRetorno.NaoEncontrado;

                    return TipoRetorno.Conflito;

                }

                var ContaBeneficiada = _context.ContaCorrente.Where(y => y.Titular == command.contaFavorecido)
                    .ExecuteUpdate(s => s.SetProperty(e => e.Saldo, e => e.Saldo + command.valor));
                if (ContaBeneficiada == 0)
                {
                    var ContabeneficiadaExistente = _context.ContaCorrente.Any(x => x.Titular == command.contaFavorecido);
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
