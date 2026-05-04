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
using api_para_banco.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Drawing;

namespace api_para_banco.Controllers
{
    [ApiVersion("3.0")]
    [Authorize(Roles = "cliente")]
    public class ClienteControllerV3(IAccountServices accountServices) : ControllerBase
    {

        [HttpGet("/V3/Ver_saldo")]
        public async Task<IActionResult> Versaldo(string titular)
        {
            var resultado = await accountServices.GetBalanceAsync(titular);

            if (resultado.Item1 == TipoRetorno.NaoEncontrado)
                return NotFound($"Titular {titular} não encontrado.");

            return Ok(resultado.Item2);     
        }
      
    }
}
