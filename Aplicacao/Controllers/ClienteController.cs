using Aplicacao.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private readonly IConfiguration _config;

        public ClienteController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM rentcar.cliente ORDER BY id;";

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
        public IActionResult Post([FromBody] Cliente cliente)
        {
            try
            {
                string query = $@"
                    INSERT INTO rentcar.cliente (iduser, nome, email, dtnascimento, cpf)
                    VALUES (
                        {cliente.IdUser},
                        '{cliente.Nome}',
                        '{cliente.Email}',
                        '{cliente.DtNascimento:yyyy-MM-dd}',
                        '{cliente.Cpf}'
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
        public IActionResult Put(int id, [FromBody] Cliente cliente)
        {
            try
            {
                string query = $@"
                    UPDATE rentcar.cliente
                    SET
                        iduser = {cliente.IdUser},
                        nome = '{cliente.Nome}',
                        email = '{cliente.Email}',
                        dtnascimento = '{cliente.DtNascimento:yyyy-MM-dd}',
                        cpf = '{cliente.Cpf}'
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
                string query = $@"
                    DELETE FROM rentcar.cliente
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

