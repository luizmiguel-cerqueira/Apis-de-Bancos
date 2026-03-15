using api_para_banco.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace api_para_banco.Services
{
    public class AdmUtilidade
    {
        private readonly EntityFrameWorkModel _context;

        public AdmUtilidade(EntityFrameWorkModel context)
        {
            _context = context;
        }
        
        public async Task<List<string>> PessoasComCaixinha(string? tipofiltro, string? filtro) 
        {
            var query = _context.ContaPoupanca.Join(_context.ContaCorrente,
                        p => p.Investidor,
                        c => c.Titular,
                        (p, c) => new PessoaComCaixinhaDTO
                        { 
                           ContaPoupanca = p,
                           ContaCorrente = c 
                        });

            switch (tipofiltro) 
            {

                case "valorMinimo":
                    query = query.Where(x => x.ContaPoupanca.Saldo > decimal.Parse(filtro));
                    break;
                    
                case "valorMaximo":
                    query = query.Where(x => x.ContaPoupanca.Saldo < decimal.Parse(filtro));
                    break;

                case "cpf":
                    query = query.Where(x => x.ContaCorrente.Titular == filtro);
                    break;
            }
            var resultado = await query.Select(y=> y.ContaCorrente.Titular).ToListAsync();
            return resultado;
        }    
        public async Task<string> AlterarSaldo (string titular, decimal valor) 
        {
            var conta = _context.ContaCorrente.Where(x=> x.Titular == titular).ExecuteUpdate(y=> y.SetProperty(s=> s.Saldo, s => s.Saldo + valor)) ;
            return $"adicionado {valor} a conta do titular {titular}";
        }
    }
}
