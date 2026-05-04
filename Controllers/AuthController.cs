using api_para_banco.model.DTO;
using api_para_banco.Services.Implamentations;
using api_para_banco.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_para_banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthorizationServices authServices) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> algo(UserDTO request) 
        {
            var result = await authServices.LoginAsync(request);

            return Ok($"{result?.AccessToken})))))))))))))))))))))))))){result?.RefreshToken}");
        }
    }
}
