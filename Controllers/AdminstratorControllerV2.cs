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

using api_para_banco.model;
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
    public class AdminstratorControllerV2 : Controller
    {

        private readonly AdmUtilidade _admUtilidade;
        
        public AdminstratorControllerV2(EntityFrameWorkModel context)
        { 
            _admUtilidade = new AdmUtilidade(context);
        }

        [ApiVersion(2.0)]
        [HttpGet("/V2/Pessoas_Com_Caixinha")]
        public async Task<IActionResult> BuscaPessoasComCaixinhas([FromQuery]Filtro2 tipofiltro, string? cpf)
        {
            ResultadoOperacaoDTO resultado = await _admUtilidade.PessoasComCaixinha(tipofiltro.tipo.ToString(), cpf);
            if(resultado.statusCode == 200)
                return Ok(resultado);

            return TratarErros(resultado.statusCode, "Nenhuma pessoa encontrada com os critérios fornecidos.", "Para de manipular o retorno.", "Erro Interno.");

        }
        [ApiVersion(2.0)]
        [HttpGet("/V2/Alterar_Saldo")]
        public async Task<IActionResult> AlterarSaldo (string titular, decimal valor) 
        {
            int resultado = await _admUtilidade.AlterarSaldo(titular, valor);
            if(resultado == 200)
                return Ok(resultado);
            
            return TratarErros(resultado, "Conta não encontrada.", "Para de manipular o retorno.", "Erro interno.");
        }
        [ApiVersion(2.0)]
        [HttpPost("/V2/Criar_Conta")]
        public async Task<IActionResult> CriarConta(string titular, string senha, string cpf, string numConta , DateOnly dataNascimento) 
        {
            int resultado = await _admUtilidade.AdcionarConta(titular, senha, cpf, numConta, dataNascimento);
            if (resultado == 200)
                return Ok(resultado);

            return TratarErros(resultado, "Para de manipular o retorno.", "Conta ja existente.", "Erro interno.");

        }
        [ApiVersion(2.0)]
        [HttpPost("/V2/Excluir_Conta")]
        public async Task<IActionResult> ExcluirConta(string titular)
        {
            int resultado = await _admUtilidade.ExcluirConta(titular);
            if (resultado == 200)
                return Ok(resultado);
            return TratarErros(resultado, "Conta não encontrada.","Para de manipular o retorno.","Erro interno.");
        }
        public IActionResult TratarErros(int resultado, string ErrorNotFound, string ErrorConflict, string Error500)
        {
            return resultado switch
            {
                404 => NotFound(ErrorNotFound),
                409 => Conflict(ErrorConflict),
                500 => StatusCode(500, Error500),
                _ => StatusCode(500, "Erro desconhecido.")
            };
        }
    }
}
