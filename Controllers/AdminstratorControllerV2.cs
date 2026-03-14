/*
 Todo:
 - Implementar autenticação e autorização para garantir que apenas usuários autorizados possam acessar as funcionalidades do cliente.
 - Adicionar melhores tratamento de erros e mensagens de resposta mais informativas para os clientes.
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
        private readonly EntityFrameWorkModel _context;
        private readonly AdmUtilidade _admUtilidade;
        
        public AdminstratorControllerV2(EntityFrameWorkModel context)
        {
            _context = context;
            _admUtilidade = new AdmUtilidade(context);
        }

        [ApiVersion(2.0)]
        [HttpGet("/V2/Pessoas_Com_Caixinha")]
        public async Task<IActionResult> BuscaPessoasComCaixinhas([FromQuery]Filtro2 tipofiltro, string? cpf)
        {
            string resultado = await _admUtilidade.PessoasComCaixinha(tipofiltro.tipo.ToString(), cpf);

            return Ok(resultado);
        }

    }
}
