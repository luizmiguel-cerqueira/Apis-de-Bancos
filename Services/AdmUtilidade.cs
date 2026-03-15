/*
Todo:

- Melhorar a validação, como sempre garantir que o titular exista.
- Colocar Try catch para tratar casos como erro de conexão ou outros erros inesperados.
- Adicionar logs para facilitar a identificação de problemas.
- 
 */

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

        public async Task<ResultadoOperacaoDTO> PessoasComCaixinha(string? tipofiltro, string? filtro) 
        {

            try
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
                if(await query.Select(y=> y.ContaCorrente.Titular).AnyAsync())
                    return new ResultadoOperacaoDTO() { statusCode = 404, resultados = new List<string>() };

                var resultado = await query.Select(y => y.ContaCorrente.Titular).ToListAsync();

                return new ResultadoOperacaoDTO() { statusCode = 200, resultados = resultado };
            }
            catch
            {

                return new ResultadoOperacaoDTO() { statusCode = 500, resultados = new List<string>() };
                
            }
        }    
        public async Task<int> AlterarSaldo (string titular, decimal valor) 
        {
            try
            {
                var conta = _context.ContaCorrente.Where(x=> x.Titular == titular).ExecuteUpdate(y=> y.SetProperty(s=> s.Saldo, s => s.Saldo + valor)) ;
                if (conta == 0)
                    return 404;

                return 200;
            }
            catch 
            {
                return 500;
            }
        }
        public async Task<int> AdcionarConta(string titular, string senha, string cpf, string numconta, DateOnly dataNascimento)
        {
            try
            {
                var novaConta = _context.ContaCorrente.Add(new ContaCorrente { Titular = titular, Senha = senha, Cpf = cpf, DataCriacao = DateTime.Now, Dtnascimento = dataNascimento, NumConta = numconta, Ativo = true, Saldo = 0});
                await _context.SaveChangesAsync();
                
                return 200;

            }
            catch(DbUpdateException)
            { 
                return 409;

            }
            catch  (Exception) 
            {
                return 500;
            }
        }
        public async Task<int> ExcluirConta(string titular) 
        {
            try
            {
                if (!await _context.ContaCorrente.Where(x => x.Titular == titular).AnyAsync())
                    return 404;
                var conta = _context.ContaCorrente.Where(x => x.Titular == titular);
                
                conta.ExecuteDelete();
                _context.SaveChanges();
            
                return 200;

            }
            catch 
            {
                return 500;
            }
        }
    }
}
