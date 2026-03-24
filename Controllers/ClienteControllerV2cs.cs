/*
 Todo:
 - Adcionar outros retornos como 404, 500 , etc para casos de erro , como conta já existente, ou erro de conexão. -- feito
 - Implementar autenticação e autorização para garantir que apenas usuários autorizados possam acessar as funcionalidades do cliente.
 - Implementar testes unitários e de integração para garantir a qualidade do código e a funcionalidade correta das operações bancárias.
 - Considerar a implementação de um sistema de log para monitorar as atividades do cliente e facilitar a depuração em caso de problemas.
 - Avaliar a possibilidade de adicionar funcionalidades adicionais, como visualização de extrato bancário, gerenciamento de cartões de crédito, ou integração com serviços de terceiros para oferecer uma experiência mais completa aos clientes.
 - Garantir a segurança dos dados dos clientes, implementando medidas de proteção contra ataques cibernéticos e garantindo a conformidade com as regulamentações de privacidade de dados. Calma Copilot n to nesse nivel
 - Otimizar o desempenho das operações bancárias, especialmente para transações de alta frequência, utilizando técnicas como caching ou otimização de consultas ao banco de dados.
*/
using api_para_banco.Aplication.Commands;
using api_para_banco.Aplication.Services;
using api_para_banco.Domain.Enums;
using api_para_banco.Infrastructure.model;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Drawing;

namespace api_para_banco.Controllers
{
    public class ClienteControllerV2cs : Controller
    {

        readonly Utilidade _utilidade;

        public ClienteControllerV2cs(ClasseCon strDeCon, EntityFrameWorkModel context, HttpClient httpClient)
        {
            _utilidade = new Utilidade(context);
        }
        
        
        [ApiVersion(2.0)]
        [HttpGet("/V2/Ver_saldo")]
        public async Task<IActionResult> Versaldo(string titular)
        {
            var resultado = await _utilidade.VerSaldo(titular);

            if (resultado.retorno == TipoRetorno.NaoEncontrado)
                return NotFound($"Titular {titular} não encontrado.");

            return Ok(resultado.valor);
        }
        
        
        [ApiVersion(2.0)]
        [HttpPut("/V2/Tranferencia_Bancaria")]
        public async Task<IActionResult> TranferenciaBancaria(TransferenciaCommand command)
        {
            TipoRetorno resultado = await _utilidade.Tranferenciabancaria(command);

            if(resultado == TipoRetorno.Sucesso)
                return Ok($"Foi tranferido, {command.valor} da conta do titular {command.contaTitular} para {command.contaFavorecido}");

            return TratarErros(resultado, "Conta Beneficiaria ou titular não encontrado", "Saldo insuficiente", "Erro interno do servidor");
        }


        [ApiVersion(2.0)]
        [HttpPut("/V2/Colocar_Na_Caixinha")]
        public async Task<IActionResult> ColocarNaCaixinha(string cpf, decimal saldo)
        {
            TipoRetorno resultado = await _utilidade.ColocarNaCaixinha(cpf, saldo);
     
            if(resultado == TipoRetorno.Sucesso)
                return Ok($"tranferido {saldo} para a caixinha");
            
            return TratarErros(resultado, "Caixinha ou conta não encontrada", "Saldo insuficiente", "Erro interno do servidor");
        }
        
        
        [ApiVersion(2.0)]
        [HttpPut("/V2/Retirar_Da_Caixinha")]
        public async Task<IActionResult> RetirarDaCaixinha(string cpf, decimal saldo)
        {
            TipoRetorno resultado = await _utilidade.RetirarDaCaixinha(cpf, saldo);
            if(resultado == TipoRetorno.Sucesso)
                return Ok($"Retirado {saldo} da caixinha");
    
            return TratarErros(resultado, "Caixinha ou conta não encontrada", "Saldo insuficiente", "Erro interno do servidor");

        }

        public IActionResult TratarErros(TipoRetorno resultado, string ErrorNotFound, string ErrorConflict, string Error500) 
        {
            return resultado switch
            {
                TipoRetorno.NaoEncontrado => NotFound(ErrorNotFound),
                TipoRetorno.Conflito => Conflict(ErrorConflict),
                TipoRetorno.ErroInterno => StatusCode(500, Error500),
                _ => StatusCode(500, "Erro desconhecido.")
            };
        }
    }
}
