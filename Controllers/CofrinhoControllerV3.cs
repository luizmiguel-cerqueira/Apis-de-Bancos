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
    public class CofrinhoControllerV3(ISafesServices safesServices, ApiHelpper apiHelpper) : ControllerBase
    {
        [HttpPut("/V3/Colocar_Na_Caixinha")]
        public async Task<IActionResult> ColocarNaCaixinha(TransferDTO request)
        {
            var resultado = await safesServices.DepositInSavingsAsync(request);

            if (resultado == TipoRetorno.Sucesso)
                return Ok($"tranferido {request.quantity} para a caixinha");

            return apiHelpper.TratarErros(this, resultado, "Caixinha ou conta não encontrada", "Saldo insuficiente", "Erro interno do servidor");
        }
        [HttpPut("/V3/Retirar_Da_Caixinha")]
        public async Task<IActionResult> RetirarDaCaixinha(TransferDTO request)
        {
            var resultado = await safesServices.WithdrawFromSavingsAsync(request);
            if (resultado == TipoRetorno.Sucesso)
                return Ok($"Retirado {request.quantity} da caixinha");

            return apiHelpper.TratarErros(this, resultado, "Caixinha ou conta não encontrada", "Saldo insuficiente", "Erro interno do servidor");

        }
        [HttpGet("/V3/Cofres_por_nome")]
        public async Task<IActionResult> CofresPorNome(string nome)
        {
            var resultado = await safesServices.GetExpecificSafeAsync(nome);
            if (resultado.tipoRetorno == TipoRetorno.Sucesso)
                return Ok(resultado.safes);
            
            return apiHelpper.TratarErros(this, resultado.tipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }
        [HttpGet("/V3/Todos_os_cofres")]
        public async Task<IActionResult> TodosOsCofres(int id)
        {
            var resultado = await safesServices.GetAllSafesAsync(id);
            if (resultado.tipoRetorno == TipoRetorno.Sucesso)
                return Ok(resultado.safes);

            return apiHelpper.TratarErros(this, resultado.tipoRetorno, "Transferência não encontrada", "Conflito", "Erro interno do servidor");
        }
    }
}
