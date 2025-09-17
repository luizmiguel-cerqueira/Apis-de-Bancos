using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using api_para_banco.model;
using Microsoft.Data.SqlClient;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_para_banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        readonly string _strDeConexao;
        public ClienteController(ClasseCon strDeCon)
        {
            _strDeConexao = strDeCon.strDeConexao;

        }

        public string Saldo { get; private set; }

        [HttpGet("ver_saldo")]
        public IActionResult VerSaldo(string Titular)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_strDeConexao))
                {
                    con.Open();
                    string query = $"SELECT Saldo FROM ContaCorrente WHERE Titular = @Titular";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Titular", Titular);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Saldo = reader["Saldo"].ToString();
                    }
                    return Ok(Saldo);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro{ex.Message}");
            }
        }
        
        [HttpPut("tranferencia_bancaria")]
        public IActionResult Transferencia(string numconta, string Contafavorecido, decimal valor) 
        {
            try 
            {
                using (SqlConnection con = new SqlConnection(_strDeConexao)) 
                {
                    con.Open();
                    string querry = $"BEGIN TRANSACTION " +
                            $"UPDATE ContaCorrente SET Saldo = Saldo - @valor WHERE NumConta = @numconta AND Saldo>= @valor;" +
                            $"If @@ROWCOUNT = 0 BEGIN ROLLBACK TRANSACTION; PRINT 'valor insuficiente' RETURN; END " +
                            $"UPDATE ContaCorrente SET Saldo = Saldo + @valor WHERE NumConta = @Contafavorecido;" +
                            $"COMMIT TRANSACTION";
                    SqlCommand cmd = new SqlCommand(querry, con);
                    cmd.Parameters.Add("@valor", SqlDbType.Decimal).Value = valor;
                    cmd.Parameters.Add("@numconta", SqlDbType.VarChar, 10).Value = numconta;
                    cmd.Parameters.Add("@Contafavorecido", SqlDbType.VarChar,10).Value = Contafavorecido;
                    //cmd.Parameters.AddWithValue("@cpf", cpf); menos eficiente, string vai como varchar(400)

                    cmd.ExecuteNonQuery();
                }
                return Ok($"Transferência de {valor} do {numconta} para o {Contafavorecido} realizada com sucesso.");
            }
            catch (Exception ex) 
            {
                return BadRequest($"Erro{ex.Message}");
            }
        }
        [HttpPut("transferir_caixinha")]
        public IActionResult TransferirParaCaixinha(decimal valor, string cpf) 
        {
            try 
            {
                using (SqlConnection con = new SqlConnection(_strDeConexao)) 
                {
                    con.Open();
                    string querry = $"BEGIN TRANSACTION " +
                                    $"UPDATE contaCorrente SET Saldo = Saldo - @valor WHERE Cpf = @cpf AND Saldo >= @valor " +
                                    $"IF @@ROWCOUNT = 0 BEGIN ROLLBACK TRANSACTION; PRINT 'valor insuficiente' RETURN; END " +
                                    $"UPDATE cofrinho_Nubank SET Saldo = Saldo + @valor WHERE Cpf = @cpf " +
                                    $"COMMIT TRANSACTION";

                    SqlCommand cmd = new SqlCommand(querry, con);
                    cmd.Parameters.Add("@valor", SqlDbType.Decimal).Value = valor;
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    //cmd.Parameters.AddWithValue("@cpf", cpf); menos eficiente, string vai como varchar(400)
                    cmd.ExecuteNonQuery();

                    return Ok($"Tranferência de {valor} para a caixinha realizada com sucesso.");
                }
            }
            catch(Exception ex) 
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut("tirar_da_caxinha")]
        public IActionResult Tirar_da_caixinha(decimal valor, string cpf) 
        {
            try 
            {
                using (SqlConnection con = new SqlConnection(_strDeConexao)) 
                {
                    con.Open();
                    string query = $"BEGIN TRANSACTION " +
                                    $"UPDATE cofrinho_Nubank SET Saldo = Saldo - @valor WHERE Cpf = @cpf AND Saldo >= @valor " +
                                    $"IF @@ROWCOUNT = 0 BEGIN ROLLBACK TRANSACTION; PRINT 'valor insuficiente' RETURN; END " +
                                    $"UPDATE contaCorrente SET Saldo = Saldo + @valor WHERE Cpf = @cpf " +
                                    $"COMMIT TRANSACTION";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.Add("@valor", SqlDbType.Decimal).Value = valor;
                    cmd.Parameters.Add("@cpf",SqlDbType.VarChar,11).Value = cpf;
                    //cmd.Parameters.AddWithValue("@cpf", cpf); menos eficiente, string vai como varchar(400)

                    cmd.ExecuteNonQuery();
                    return Ok($"Tranferência de {valor} da caixinha realizada com sucesso.");
                }
            }
            catch (Exception ex) 
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut("Simular_caixinha")]
        public IActionResult Simular_Caixinha(float valor, int quantidade_de_dias)
        {
            double resultado = valor * Math.Pow(1.01, quantidade_de_dias);
            return Ok($"Ao depositar na caixinha {valor} em {quantidade_de_dias} terá  {resultado:2f}");
        }
    }
}
