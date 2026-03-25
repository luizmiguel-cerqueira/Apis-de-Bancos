/*
 Already todo:
    //Melhorar a validação, como sempre garantir que o titular exista. -- feito
    //Colocar Try catch para tratar casos como erro de conexão ou outros erros inesperados.-- feito
Todo:
    - Adicionar logs para facilitar a identificação de problemas.
    - 
 */
using api_para_banco.Aplication.Commands;
using api_para_banco.Domain.Enums;
using api_para_banco.Infrastructure.Data.DTO;
using api_para_banco.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace api_para_banco.Aplication.Services
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
                    return new ResultadoOperacaoDTO() { statusCode = TipoRetorno.NaoEncontrado, resultados = new List<string>() };

                var resultado = await query.Select(y => y.ContaCorrente.Titular).ToListAsync();

                return new ResultadoOperacaoDTO() { statusCode = TipoRetorno.Sucesso, resultados = resultado };
            }
            catch
            {

                return new ResultadoOperacaoDTO() { statusCode = TipoRetorno.ErroInterno, resultados = new List<string>() };
                
            }
        }
        
        public async  Task<TipoRetorno> AlterarSaldo (string titular, decimal valor) 
        {
            try
            {
                var conta =  _context.ContaCorrente.Where(x=> x.Titular == titular).ExecuteUpdate(y=> y.SetProperty(s=> s.Saldo, s => s.Saldo + valor)) ;
                if (conta == 0)
                    return TipoRetorno.NaoEncontrado;

                return TipoRetorno.Sucesso;
            }
            catch 
            {
                return TipoRetorno.ErroInterno;
            }
        }
        public async Task<TipoRetorno> AdcionarConta(CriarContaCommand command)
        {
            try
            {
                var novaConta = _context.ContaCorrente.Add(new ContaCorrente 
                { 
                    Titular = command.titular, 
                    Senha = command.senha, Cpf = command.cpf, 
                    DataCriacao = DateTime.Now, 
                    Dtnascimento = command.dataNascimento, 
                    NumConta = command.numConta, 
                    Ativo = command.Ativa, 
                    Saldo = 0
                });
                await _context.SaveChangesAsync();
                
                return TipoRetorno.Sucesso;

            }
            catch(DbUpdateException)
            { 
                return TipoRetorno.Conflito;

            }
            catch  (Exception) 
            {
                return TipoRetorno.ErroInterno;
            }
        }
        public async Task<TipoRetorno> ExcluirConta(string titular) 
        {
            try
            {
                if (!await _context.ContaCorrente.Where(x => x.Titular == titular).AnyAsync())
                    return TipoRetorno.NaoEncontrado;
                var conta = _context.ContaCorrente.Where(x => x.Titular == titular);
                
                conta.ExecuteDelete();
                _context.SaveChanges();
            
                return TipoRetorno.Sucesso;

            }
            catch 
            {
                return TipoRetorno.ErroInterno;
            }
        }
    }
}
