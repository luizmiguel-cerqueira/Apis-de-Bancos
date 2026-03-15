using api_para_banco.model;
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
        public async Task<int> VerSaldo(string titular)
        {
            try
            {
                var Contacorrente = _context.ContaCorrente.FirstOrDefault(x => x.Titular == titular);
                if (Contacorrente != null)
                    return int.Parse((Contacorrente.Saldo * 1000).ToString());
                return 404;

            }
            catch
            {
                return 500;
            }
        }
        public async Task<int> Tranferenciabancaria(string titular, string contaBeneficiada, decimal quantia)
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
                        return 404;

                    return 409;

                }

                var ContaBeneficiada = _context.ContaCorrente.Where(y => y.Titular == contaBeneficiada)
                    .ExecuteUpdate(s => s.SetProperty(e => e.Saldo, e => e.Saldo + quantia));
                if (ContaBeneficiada == 0)
                {
                    var ContabeneficiadaExistente = _context.ContaCorrente.Any(x => x.Titular == contaBeneficiada);
                    transaction.Rollback();
                    if(!ContabeneficiadaExistente)
                        return 404;
                    return 409;
                }
                
                _context.SaveChanges();
                transaction.Commit();
                return 200;
            } 
            
            catch
            {
                return 500;
            }
        }
        public async Task<int> ColocarNaCaixinha(string cpf, decimal saldo)
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
                        return 404;
                    return 409;
                }
                var ContaPoupanca = _context.ContaPoupanca.Where(y => y.Cpf == cpf)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo + saldo));
                if (ContaPoupanca == 0)
                {
                    var ContaPoupancaExistente = _context.ContaPoupanca.Any(x => x.Cpf == cpf);
                    transaction.Rollback();
                    if (!ContaPoupancaExistente)
                        return 404;
                    return 409;
                }
                _context.SaveChanges();
                transaction.Commit();

                return 200;
            }
            catch 
            {
                return 500;
            }
        }
        public async Task<int> RetirarDaCaixinha(string cpf, decimal saldo)
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
                        return 404;
                    return 409;
                }
                var ContaCorrente = _context.ContaCorrente.Where(y => y.Cpf == cpf)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo + saldo));
                if (ContaCorrente == 0)
                {
                    var ContaTitularExistente = _context.ContaCorrente.Any(x => x.Cpf == cpf);
                    transaction.Rollback();

                    if (!ContaTitularExistente)
                        return 404;
                    return 409;
                }
                _context.SaveChanges();
                transaction.Commit();
                return 200;
            }
            catch 
            {
                return 500;
            }
        }

    }
}
