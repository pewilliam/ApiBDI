using Aplicacao.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AluguelController : Controller
    {
        private readonly IConfiguration _config;

        public AluguelController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM rentcar.aluguel ORDER BY id;";

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            string sqlDataSource = _config.GetConnectionString("RentCarCon");
            NpgsqlDataReader reader = null;
            using (NpgsqlConnection connection = new NpgsqlConnection(sqlDataSource))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader[i];
                        }
                        result.Add(row);
                    }

                    reader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Aluguel aluguel)
        {
            try
            {
                string query = $@"
                    INSERT INTO rentcar.aluguel (idfuncionario, idcliente, idmodelo, idformapagto, dhaluguel, periodoaluguel, valoraluguel, dhrenovacao, periodorenovacao, valortotal, valorrenovacao)
                    VALUES (
                        {aluguel.IdFuncionario},
                        {aluguel.IdCliente},
                        {aluguel.IdModelo},
                        {aluguel.IdFormaPagto},
                        '{aluguel.DhAluguel:yyyy-MM-dd HH:mm:ss}',
                        {aluguel.PeriodoAluguel},
                        {aluguel.ValorAluguel},
                        {(aluguel.DhRenovacao.HasValue ? $"'{aluguel.DhRenovacao:yyyy-MM-dd HH:mm:ss}'" : "NULL")},
                        {(aluguel.PeriodoRenovacao.HasValue ? aluguel.PeriodoRenovacao.ToString() : "NULL")},
                        {aluguel.ValorTotal},
                        {(aluguel.ValorRenovacao.HasValue ? aluguel.ValorRenovacao.ToString() : "NULL")}
                    );";

                string sqlDataSource = _config.GetConnectionString("RentCarCon");
                using (NpgsqlConnection connection = new NpgsqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Ok("Inserção realizada com sucesso.");
                        }
                        else
                        {
                            return BadRequest("A inserção falhou.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Aluguel aluguel)
        {
            try
            {
                string query = $@"
                    UPDATE rentcar.aluguel
                    SET
                        idfuncionario = {aluguel.IdFuncionario},
                        idcliente = {aluguel.IdCliente},
                        idmodelo = {aluguel.IdModelo},
                        idformapagto = {aluguel.IdFormaPagto},
                        dhaluguel = '{aluguel.DhAluguel:yyyy-MM-dd HH:mm:ss}',
                        periodoaluguel = {aluguel.PeriodoAluguel},
                        valoraluguel = {aluguel.ValorAluguel},
                        dhrenovacao = {(aluguel.DhRenovacao.HasValue ? $"'{aluguel.DhRenovacao:yyyy-MM-dd HH:mm:ss}'" : "NULL")},
                        periodorenovacao = {(aluguel.PeriodoRenovacao.HasValue ? $"'{aluguel.DhRenovacao:yyyy-MM-dd HH:mm:ss}'" : "NULL")},
                        valortotal = {aluguel.ValorTotal},
                        valorrenovacao = {(aluguel.ValorRenovacao.HasValue ? aluguel.ValorRenovacao.ToString() : "NULL")}
                    WHERE id = {id};";

                string sqlDataSource = _config.GetConnectionString("RentCarCon");
                using (NpgsqlConnection connection = new NpgsqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Ok("Atualização realizada com sucesso.");
                        }
                        else
                        {
                            return NotFound("Registro não encontrado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                string query = $@"DELETE FROM rentcar.aluguel WHERE id = {id};";

                string sqlDataSource = _config.GetConnectionString("RentCarCon");
                using (NpgsqlConnection connection = new NpgsqlConnection(sqlDataSource))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Ok("Exclusão realizada com sucesso.");
                        }
                        else
                        {
                            return NotFound("Registro não encontrado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
