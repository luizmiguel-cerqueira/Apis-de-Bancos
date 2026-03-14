/*
 Todo:
 - Implementar autenticação e autorização para garantir que apenas usuários autorizados possam acessar as funcionalidades do cliente.
 - Adicionar melhores tratamento de erros e mensagens de resposta mais informativas para os clientes.
 - Implementar testes unitários e de integração para garantir a qualidade do código e a funcionalidade correta das operações bancárias.
 - Considerar a implementação de um sistema de log para monitorar as atividades do cliente e facilitar a depuração em caso de problemas.
 - Avaliar a possibilidade de adicionar funcionalidades adicionais, como visualização de extrato bancário, gerenciamento de cartões de crédito, ou integração com serviços de terceiros para oferecer uma experiência mais completa aos clientes.
 - Garantir a segurança dos dados dos clientes, implementando medidas de proteção contra ataques cibernéticos e garantindo a conformidade com as regulamentações de privacidade de dados. Calma Copilot n to nesse nivel
 - Otimizar o desempenho das operações bancárias, especialmente para transações de alta frequência, utilizando técnicas como caching ou otimização de consultas ao banco de dados.
*/
using api_para_banco.model;
using api_para_banco.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api_para_banco.Controllers
{
    public class ClienteControllerV2cs : Controller
    {

        readonly string _strDeConexao;
        readonly EntityFrameWorkModel _context;
        readonly Utilidade _utilidade;

        public ClienteControllerV2cs(ClasseCon strDeCon, EntityFrameWorkModel context)
        {
            _context = context;
            _utilidade = new Utilidade(context);
            _strDeConexao = strDeCon.strDeConexao;
        }
        [ApiVersion(2.0)]
        [HttpGet("/V2/Ver_saldo")]
        public async Task<IActionResult> Versaldo(string titular)
        {
            string saldo = await _utilidade.VerSaldo(titular);

            if (saldo != "")
                return Ok(_utilidade.VerSaldo(titular));

            return NotFound($"Titular {titular} não encontrado.");
        }
        [ApiVersion(2.0)]
        [HttpPut("/V2/Tranferencia_Bancaria")]
        public async Task<IActionResult> TranferenciaBancaria(string titular, string contaBeneficiada, decimal quantia)
        {
            string resultado = await _utilidade.Tranferenciabancaria(titular, contaBeneficiada, quantia);

            if (resultado != "")
                return Ok(resultado);

            return BadRequest(resultado);
        }
        [ApiVersion(2.0)]
        [HttpPut("/V2/Colocar_Na_Caixinha")]

        public async Task<IActionResult> ColocarNaCaixinha(string cpf, decimal saldo)
        {
            string resultado = await _utilidade.ColocarNaCaixinha(cpf, saldo);
            if (resultado != "")
                return Ok(resultado);
            return BadRequest(resultado);
        }
        [ApiVersion(2.0)]
        [HttpPut("/V2/Retirar_Da_Caixinha")]
        public async Task<IActionResult> RetirarDaCaixinha(string cpf, decimal saldo)
        {
            string resultado = await _utilidade.RetirarDaCaixinha(cpf, saldo);
            
            if (resultado != "")
                return Ok(resultado);
            return BadRequest(resultado);
        }
           
    }
}
