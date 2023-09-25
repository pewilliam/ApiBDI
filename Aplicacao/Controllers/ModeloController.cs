using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Aplicacao.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModeloController : Controller
    {
        private readonly IConfiguration _config;

        public ModeloController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM rentcar.modelo ORDER BY id;";

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
        public IActionResult Post([FromBody] Modelo modelo)
        {
            if (modelo == null)
            {
                return BadRequest("Dados do modelo inválidos.");
            }

            string insertQuery = $@"
                INSERT INTO rentcar.modelo (ano, motor, tipocambio, idmarca, idcategoria, valorhora, idcarro, combustivel, capacidade, portas, placa)
                VALUES (
                    '{modelo.Ano}',
                    '{modelo.Motor}',
                    {modelo.TipoCambio},
                    {modelo.IdMarca},
                    {modelo.IdCategoria},
                    {modelo.ValorHora},
                    {modelo.IdCarro},
                    '{modelo.Combustivel}',
                    {modelo.Capacidade},
                    {modelo.Portas},
                    '{modelo.Placa}'
                );";

            string sqlDataSource = _config.GetConnectionString("RentCarCon");
            using (NpgsqlConnection connection = new NpgsqlConnection(sqlDataSource))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok("Inserção bem-sucedida.");
                    }
                    else
                    {
                        return BadRequest("A inserção falhou.");
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Modelo modelo)
        {
            try
            {
                string query = $@"
                    UPDATE rentcar.modelo
                    SET
                        ano = '{modelo.Ano}',
                        motor = '{modelo.Motor}',
                        tipocambio = {modelo.TipoCambio},
                        idmarca = {modelo.IdMarca},
                        idcategoria = {modelo.IdCategoria},
                        valorhora = {modelo.ValorHora},
                        idcarro = {modelo.IdCarro},
                        combustivel = '{modelo.Combustivel}',
                        capacidade = {modelo.Capacidade},
                        portas = {modelo.Portas},
                        placa = '{modelo.Placa}'
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
                    DELETE FROM rentcar.modelo
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
