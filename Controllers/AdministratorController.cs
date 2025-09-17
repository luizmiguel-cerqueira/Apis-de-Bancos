using api_para_banco.Controllers;
using api_para_banco.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace api_para_banco.Controllers
{
    public class Filtro 
    {
        public TipoFiltro? tipo { get; set; }
    }
    public enum TipoFiltro 
    {
        valorMinimo,
        valorMaximo
    }
    public class AdministratorController : Controller
    {
        readonly string _strDeConexao;

        model.ModeloBusca molde = new model.ModeloBusca();
        public AdministratorController(model.ClasseCon strDeCon)
        {
            _strDeConexao = strDeCon.strDeConexao;
        }
        [HttpGet("pessoasNaCaixinha")]
        public IActionResult PessoasComCaixinha([FromQuery]Filtro? filtro, decimal? valor)
        {
            int tamanho = 0;
            string query = "";
            switch (filtro.tipo) 
            {
                case TipoFiltro.valorMinimo:
                    query = "SELECT * FROM ContaPoupanca AS N JOIN ContaCorrente AS C ON Investidor = C.Titular AND n.Saldo > 0 AND C.Saldo > @valor";
                    break;
                case TipoFiltro.valorMaximo:
                    query = "SELECT * FROM ContaPoupanca AS N JOIN ContaCorrente ON Investidor = Titular AND n.Saldo > 0 AND C.Saldo < @valor";
                    break;
                default:
                    query = "SELECT * FROM ContaPoupanca AS N JOIN ContaCorrente ON Investidor = Titular AND n.Saldo > 0";
                    break;
            }

            List<ModeloBusca> pessoas = new List<ModeloBusca>();
            try 
            {
                using (SqlConnection con = new SqlConnection(_strDeConexao)) 
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(query, con);
                    if (filtro.tipo != null) 
                    {
                    var param = cmd.Parameters.Add("@valor", SqlDbType.Decimal,15);
                    param.Precision = 15;   
                    param.Scale = 2;
                    param.Value =valor;
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {
                        var item = new ModeloBusca
                        {
                            titular = reader["Investidor"].ToString(),

                            saldo = reader["Saldo"].ToString(),

                            cpf = reader["cpf"].ToString(),

                            id = Convert.ToInt32(reader["id"])

                        };
                        pessoas.Add(item);
                    }
                    return Ok(pessoas);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ ex.Message}");
            }
        }
        [HttpPut("alterar_saldo")]
        public IActionResult AlterarSaldo(string Titular, int valor)
        {   string Saldo = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_strDeConexao))
                {
                    con.Open();
                    string querry = $"UPDATE ContaCorrente SET Saldo = Saldo + @valor OUTPUT Inserted.Saldo WHERE Titular = @Titular";
                    SqlCommand cmd = new SqlCommand(querry, con);
                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.Parameters.AddWithValue("@Titular", Titular);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Saldo = reader["Saldo"].ToString();
                    }
                    return Ok($"Seu saldo atual é {Saldo}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro{ex.Message}");
            }
        }
    }
}
//pouco eficiente mas tem o linq ent vou deixar salvo
//public IActionResult PessoasComCaixinha([FromQuery] Filtro? filtro, string? valorOuIdentificacao)
//{
//    int tamanho = 0;
//    string querry = "";

//    List<ModeloBusca> pessoas = new List<ModeloBusca>();
//    try
//    {
//        using (SqlConnection con = new SqlConnection(_strDeConexao))
//        {
//            querry = "SELECT * FROM ContaPoupanca AS N JOIN ContaCorrente ON Investidor = Titular AND N.Saldo > 0";
//            con.Open();

//            SqlCommand cmd = new SqlCommand(querry, con);
//            SqlDataReader reader = cmd.ExecuteReader();
//            while (reader.Read())
//            {
//                var item = new ModeloBusca
//                {
//                    titular = reader["Investidor"].ToString(),

//                    saldo = reader["Saldo"].ToString(),

//                    cpf = reader["cpf"].ToString(),

//                    id = Convert.ToInt32(reader["id"])

//                };
//                pessoas.Add(item);
//            }
//            switch (filtro.tipo)
//            {
//                case TipoFiltro.valorMinimo:
//                    pessoas = pessoas.Where(x => Convert.ToDecimal(x.saldo) >= Convert.ToDecimal(valorOuIdentificacao)).ToList();
//                    break;
//                case TipoFiltro.valorMaximo:
//                    pessoas = pessoas.Where(x => Convert.ToDecimal(x.saldo) <= Convert.ToDecimal(valorOuIdentificacao)).ToList();
//                    break;
//                case TipoFiltro.cpf:
//                    pessoas = pessoas.Where(x => x.cpf == valorOuIdentificacao).ToList();
//                    break;
//            }


//            return Ok(pessoas);
//        }
//    }
//    catch (Exception ex)
//    {
//        return BadRequest(new List<string> { ex.Message });
//    }
//}
