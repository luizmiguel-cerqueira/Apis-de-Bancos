using api_para_banco.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace api_para_banco.Services.Implamentations
{
    public class ApiHelpper
    {

        public IActionResult TratarErros(ControllerBase controller, TipoRetorno resultado, string ErrorNotFound, string ErrorConflict, string Error500) => resultado switch
        {
            TipoRetorno.NaoEncontrado => controller.NotFound(ErrorNotFound),
            TipoRetorno.Conflito => controller.Conflict(ErrorConflict),
            TipoRetorno.ErroInterno => controller.StatusCode(500, Error500),
            _ => controller.StatusCode(500, "Erro desconhecido.")
        };
    }
}
