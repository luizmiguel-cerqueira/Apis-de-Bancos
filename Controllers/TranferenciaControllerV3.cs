using api_para_banco.Domain.Enums;
using api_para_banco.model.DTO;
using api_para_banco.Services.Implamentations;
using api_para_banco.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_para_banco.Controllers
{
    [ApiVersion("3.0")]
    [Authorize(Roles = "cliente")]
    public class TranferenciaControllerV3(ITransferServices transferServices, ApiHelpper apiHelpper) : ControllerBase
    {
        [HttpPost("/V3/Tranferencia_Bancaria")]
        public async Task<IActionResult> TranferenciaBancaria(TransferDTO request)
        {
            var resultado = await transferServices.TransferAsnyc(request);

            if (resultado == TipoRetorno.Sucesso)
                return Ok($"Foi tranferido, {request.quantity} da conta do titular {request.FromAccount} para {request.ToAccount}");

            return apiHelpper.TratarErros(this,resultado, "Conta Beneficiaria ou titular não encontrado", "Saldo insuficiente", "Erro interno do servidor");
        }

        [HttpGet("/V3/Histórico_por_IdTranferencia")]
        public async Task<IActionResult> HistoricoPorIdTranferencia(Guid idTranferencia)
        {
            var resultado = await transferServices.GetTransferByIdAsync(idTranferencia);
            if (resultado.TipoRetorno == TipoRetorno.Sucesso)
            {
                return Ok(resultado.Transfers);
            }
            return apiHelpper.TratarErros(this, resultado.TipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }

        [HttpGet("/V3/Histórico_por_NumeroConta")]
        public async Task<IActionResult> HistoricoPorNumeroConta(string numeroConta)
        {
            var resultado = await transferServices.GetTransfersByAccountAsync(numeroConta);
            if (resultado.TipoRetorno == TipoRetorno.Sucesso)
            {
                return Ok(resultado.Transfers);
            }
            return apiHelpper.TratarErros(this, resultado.TipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }
    }
}
