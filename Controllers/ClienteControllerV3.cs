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
using api_para_banco.Domain.Enums;
using api_para_banco.model;
using api_para_banco.model.DTO;
using api_para_banco.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Drawing;

namespace api_para_banco.Controllers
{
    public class ClienteControllerV3(ITransferServices transferServices, IAccountServices accountServices, ISafesServices safesServices) : Controller
    {


        [ApiVersion(3.0)]
        [HttpGet("/V3/Ver_saldo")]
        public async Task<IActionResult> Versaldo(string titular)
        {
            var resultado = await accountServices.GetBalanceAsync(titular);

            if (resultado.Item1 == TipoRetorno.NaoEncontrado)
                return NotFound($"Titular {titular} não encontrado.");

            return Ok(resultado.Item2);     
        }


        [ApiVersion(3.0)]
        [HttpPut("/V3/Tranferencia_Bancaria")]
        public async Task<IActionResult> TranferenciaBancaria(TransferDTO request)
        {
            var resultado = await transferServices.TransferAsnyc(new TransferDTO { FromAccount = "111111111111111", ToAccount= "21212121211" , quantity = 1 });

            if (resultado == TipoRetorno.Sucesso)
                return Ok($"Foi tranferido, {request.quantity} da conta do titular {request.FromAccount} para {request.ToAccount}");

            return TratarErros(resultado, "Conta Beneficiaria ou titular não encontrado", "Saldo insuficiente", "Erro interno do servidor");
        }


        [ApiVersion(3.0)]
        [HttpPut("/V3/Colocar_Na_Caixinha")]
        public async Task<IActionResult> ColocarNaCaixinha(TransferDTO request)
        {
            var resultado = await safesServices.DepositInSavingsAsync(request);

            if (resultado == TipoRetorno.Sucesso)
                return Ok($"tranferido {request.quantity} para a caixinha");

            return TratarErros(resultado, "Caixinha ou conta não encontrada", "Saldo insuficiente", "Erro interno do servidor");
        }


        [ApiVersion(3.0)]
        [HttpPut("/V3/Retirar_Da_Caixinha")]
        public async Task<IActionResult> RetirarDaCaixinha(TransferDTO request)
        {
            var resultado = await safesServices.WithdrawFromSavingsAsync(request);
            if (resultado == TipoRetorno.Sucesso)
                return Ok($"Retirado {request.quantity} da caixinha");


            return TratarErros(resultado, "Caixinha ou conta não encontrada", "Saldo insuficiente", "Erro interno do servidor");

        }
        [ApiVersion(3.0)]
        [HttpGet("/V3/Histórico_por_IdTranferencia")]
        public async Task<IActionResult> HistoricoPorIdTranferencia(Guid idTranferencia)
        {
            var resultado = await transferServices.GetTransferByIdAsync(idTranferencia);
            if(resultado.TipoRetorno == TipoRetorno.Sucesso)
            {
                return Ok(resultado.Transfers);
            }
            return TratarErros(resultado.TipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }
        [ApiVersion(3.0)]
        [HttpGet("/V3/Histórico_por_NumeroConta")]
        public async Task<IActionResult> HistoricoPorNumeroConta(string numeroConta)
        {
            var resultado = await transferServices.GetTransfersByAccountAsync(numeroConta);
            if (resultado.TipoRetorno == TipoRetorno.Sucesso)
            {
                return Ok(resultado.Transfers);
            }
            return TratarErros(resultado.TipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }
        [ApiVersion(3.0)]
        [HttpGet("/V3/Todos_os_cofres")]
        public async Task<IActionResult> TodosOsCofres(int id)
        {
            var resultado = await safesServices.GetAllSafesAsync(id);
            if (resultado.tipoRetorno == TipoRetorno.Sucesso)
            {
                return Ok(resultado.safes);
            }
            return TratarErros(resultado.tipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }
        [ApiVersion(3.0)]
        [HttpGet("/V3/Cofres_por_nome")]
        public async Task<IActionResult> CofresPorNome(string nome)
        {
            var resultado = await safesServices.GetExpecificSafeAsync(nome);
            if (resultado.tipoRetorno == TipoRetorno.Sucesso)
            {
                return Ok(resultado.safes);
            }
            return TratarErros(resultado.tipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
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
