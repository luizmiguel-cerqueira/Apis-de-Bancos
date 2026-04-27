/*
 Todo:
 - Adcionar outros retornos como 404, 500 , etc para casos de erro , como conta já existente, ou erro de conexão. -- feito
 - Implementar autenticação e autorização para garantir que apenas usuários autorizados possam acessar as funcionalidades do Adm.
 - Implementar testes unitários e de integração para garantir a qualidade do código e a funcionalidade correta das operações bancárias.
 - Considerar a implementação de um sistema de log para monitorar as atividades do cliente e facilitar a depuração em caso de problemas.
 - Avaliar a possibilidade de adicionar funcionalidades adicionais, como visualização de extrato bancário, gerenciamento de cartões de crédito, ou integração com serviços de terceiros para oferecer uma experiência mais completa aos clientes.
 - Garantir a segurança dos dados dos clientes, implementando medidas de proteção contra ataques cibernéticos e garantindo a conformidade com as regulamentações de privacidade de dados. Calma Copilot n to nesse nivel
 - Otimizar o desempenho das operações bancárias, especialmente para transações de alta frequência, utilizando técnicas como caching ou otimização de consultas ao banco de dados.
 -  
*/

using api_para_banco.Domain.Enums;
using api_para_banco.model;
using api_para_banco.model.DTO;
using api_para_banco.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace api_para_banco.Controllers
{
    public class Filtro2 
    {
       public Tipo2? tipo { get; set; }
    }
    public enum Tipo2 
    {
        valorMinimo,
        valorMaximo,
        cpf
    }
    public class AdminstratorControllerV3(IAccountManagerServices accountManagerServices) : Controller
    {

        //[ApiVersion(3.0)]
        //[HttpGet("/V3/Pessoas_Com_Caixinha")]
        //public async Task<IActionResult> BuscaPessoasComCaixinhas([FromQuery] Filtro2 tipofiltro, string? cpf)
        //{
        //    ResultadoOperacaoDTO resultado = await _admUtilidade.PessoasComCaixinha(tipofiltro.tipo.ToString(), cpf);
        //    if (resultado.statusCode == TipoRetorno.Sucesso)
        //        return Ok(resultado);

        //    return TratarErros(resultado.statusCode);

        //}
        //[ApiVersion(3.0)]
        //[HttpGet("/V3/Alterar_Saldo")]
        //public async Task<IActionResult> AlterarSaldo (string titular, decimal valor) 
        //{
        //    TipoRetorno resultado = await _admUtilidade.AlterarSaldo(titular, valor);

        //    if(resultado == TipoRetorno.Sucesso)
        //        return Ok(resultado);

        //    return TratarErros(resultado);
        //}
        
        [ApiVersion(3.0)]
        [HttpPost("/V3/Criar_Conta")]
        public async Task<IActionResult> CriarConta(CreateAccountDTO request)
        {
            TipoRetorno resultado = await accountManagerServices.CreateAccount(request);
            if (resultado == TipoRetorno.Sucesso)
                return Ok(resultado);

            return TratarErros(resultado);

        }
        [ApiVersion(3.0)]
        [HttpPost("/V3/Excluir_Conta")]
        public async Task<IActionResult> ExcluirConta(string titular)
        {
            TipoRetorno resultado = await accountManagerServices.DeleteAccount(titular);
            if (resultado == TipoRetorno.Sucesso)
                return Ok(resultado);

            return TratarErros(resultado);
        }
        public IActionResult TratarErros(TipoRetorno resultado)
        {
            return resultado switch
            {
                TipoRetorno.NaoEncontrado => NotFound("Conta não encontrada."),
                TipoRetorno.Conflito => Conflict("Operação inválida."),
                TipoRetorno.ErroInterno => StatusCode(500, "Erro interno."),
                _ => StatusCode(500, "Erro desconhecido.")
            };
        }
    }
}
