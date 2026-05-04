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
using api_para_banco.Services.Implamentations;
using api_para_banco.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_para_banco.Controllers
{
    public class Filtro 
    {
       public Tipo? tipo { get; set; }
    }
    public enum Tipo 
    {
        valorMinimo,
        valorMaximo,
        cpf
    }
    [ApiVersion(3.0)]
    [Authorize(Roles = "admin")]
    public class AdminstratorControllerV3(IAccountManagerServices accountManagerServices, ApiHelpper apiHelper) : ControllerBase
    {
        
        [HttpPost("/V3/Criar_Conta")]
        public async Task<IActionResult> CriarConta(CreateAccountDTO request)
        {
            TipoRetorno resultado = await accountManagerServices.CreateAccount(request);
            if (resultado == TipoRetorno.Sucesso)
                return Ok(resultado);

            return apiHelper.TratarErros(this, resultado, "Conta não encontrada", "Conflito", "Erro interno do servidor");

        }
        

        [HttpPost("/V3/Excluir_Conta")]
        public async Task<IActionResult> ExcluirConta(string titular)
        {
            TipoRetorno resultado = await accountManagerServices.DeleteAccount(titular);
            if (resultado == TipoRetorno.Sucesso)
                return Ok(resultado);

            return apiHelper.TratarErros(this, resultado, "Conta não encontrada", "Conflito", "Erro interno do servidor");
        }
    }
}
