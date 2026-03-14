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
        public async Task<string> VerSaldo(string titular)
        {
            var Contacorrente = _context.ContaCorrente.FirstOrDefault(x => x.Titular == titular);
            if (Contacorrente != null)
                return Contacorrente.Saldo.ToString();

            return "";
        }
        public async Task<string> Tranferenciabancaria(string titular, string contaBeneficiada, decimal quantia)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var ContaTitular = _context.ContaCorrente.Where(x => x.Titular == titular && x.Saldo >= quantia)
                .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - quantia));
            if (ContaTitular == 0)
            {
                transaction.Rollback();
                return "Saldo insuficiente ou titular não encontrado.";
            }

            var ContaBeneficiada = _context.ContaCorrente.Where(y => y.Titular == contaBeneficiada)
                .ExecuteUpdate(s => s.SetProperty(e => e.Saldo, e => e.Saldo + quantia));
            if (ContaTitular == 0)
            {
                transaction.Rollback();
                return "Conta beneficiaria não encontrada não encontrado.";
            }
            _context.SaveChanges();
            transaction.Commit();
            return $"O titular {titular} tranferiu {quantia:F2} para a conta {contaBeneficiada}";
        }
        public async Task<string> ColocarNaCaixinha(string cpf, decimal saldo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var ContaTitular = _context.ContaCorrente.Where(x => x.Cpf == cpf && x.Saldo >= saldo)
                .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - saldo));
            if (ContaTitular == 0)
            {
                transaction.Rollback();
                return "Saldo insuficiente ou titular não encontrado.";
            }
            var ContaPoupanca = _context.ContaPoupanca.Where(y => y.Cpf == cpf)
                .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo + saldo));
            if (ContaPoupanca == 0)
            {
                transaction.Rollback();
                return "Conta poupança não encontrada.";
            }
            _context.SaveChanges();
            transaction.Commit();

            return $"O titular {cpf} colocou {saldo:F2} na caixinha.";
        }
        public async Task<string> RetirarDaCaixinha(string cpf, decimal saldo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var ContaPoupanca = _context.ContaPoupanca.Where(x => x.Cpf == cpf && x.Saldo >= saldo)
                .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo - saldo));
            if (ContaPoupanca == 0)
            {
                transaction.Rollback();
                return "Saldo insuficiente ou titular não encontrado.";
            }
            var ContaCorrente = _context.ContaCorrente.Where(y => y.Cpf == cpf)
                .ExecuteUpdate(s => s.SetProperty(c => c.Saldo, c => c.Saldo + saldo));
            if (ContaCorrente == 0)
            {
                transaction.Rollback();
                return "Conta corrente não encontrada.";
            }
            _context.SaveChanges();
            transaction.Commit();
            return $"O titular {cpf} retirou {saldo:F2} da caixinha.";
        }
    }
}
