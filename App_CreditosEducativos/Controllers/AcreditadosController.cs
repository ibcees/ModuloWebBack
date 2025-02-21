using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using App_CreditosEducativos.Models;
using System;

namespace App_CreditosEducativos.Controllers
{
    [ApiController]
    [Route("api/Acreditados")]
    public class AcreditadosController : ControllerBase
    {
        private readonly string _connectionString;

        public AcreditadosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexionCreditos");
        }

        // GET: api/Acreditados
        [HttpGet("ConsultarAcreditados")]
        public async Task<ActionResult<IEnumerable<AcreditadosModel>>> GetAcreditados([FromQuery] DateTime fechaInicial, [FromQuery] DateTime fechaFinal)
        {
            var acreditadosList = new List<AcreditadosModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("Acreditados_Procedure", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros de la fecha
                cmd.Parameters.AddWithValue("@FechaInicial", fechaInicial);
                cmd.Parameters.AddWithValue("@FechaFinal", fechaFinal);

                await connection.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        AcreditadosModel acreditado = new AcreditadosModel
                        {
                            Expediente = reader.GetInt64(reader.GetOrdinal("Expediente")),
                            Nombre_Acreditado = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                            Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? null : reader.GetString(reader.GetOrdinal("Sexo")),
                            CiudadLocalidad = reader.GetString(reader.GetOrdinal("CiudadLocalidad")),
                            Contrato = reader.IsDBNull(reader.GetOrdinal("Contrato")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("Contrato")),
                            TipoCredito = reader.IsDBNull(reader.GetOrdinal("Tipo de Credito")) ? null : reader.GetString(reader.GetOrdinal("Tipo de Credito")),
                            FechaTermEstudios = reader.IsDBNull(reader.GetOrdinal("Fecha Term Estudios")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha Term Estudios")),
                            CampusID = reader.GetInt64(reader.GetOrdinal("CampusID")),
                            NombreEscuela = reader.GetString(reader.GetOrdinal("Nombre Escuela")),
                            TipoEscuela = reader.IsDBNull(reader.GetOrdinal("TipoEscuela")) ? null : reader.GetString(reader.GetOrdinal("TipoEscuela")), //byte
                            Estrato = reader.IsDBNull(reader.GetOrdinal("Estrato")) ? null : reader.GetString(reader.GetOrdinal("Estrato")), //byte
                            Nivel = reader.IsDBNull(reader.GetOrdinal("Nivel")) ? null : reader.GetString(reader.GetOrdinal("Nivel")), //byte
                            Semestre = reader.IsDBNull(reader.GetOrdinal("Semestre")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("Semestre")),
                            CarreraNombre = reader.GetString(reader.GetOrdinal("CarreraNombre")),
                            Capital = reader.IsDBNull(reader.GetOrdinal("Capital")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Capital")),
                            Sucursal = reader.IsDBNull(reader.GetOrdinal("Sucursal")) ? null : reader.GetString(reader.GetOrdinal("Sucursal")),
                            MunicipioFam = reader.GetString(reader.GetOrdinal("Municipio Fam")),
                            FechaAsignacion = reader.IsDBNull(reader.GetOrdinal("Fecha Asignacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Fecha Asignacion")),
                            FechaAutorizacion = reader.IsDBNull(reader.GetOrdinal("Fecha Autorizacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Fecha Autorizacion")),
                            CampusEstadoID = reader.GetInt32(reader.GetOrdinal("Campus Estado ID")),
                            TipoEvaluacion = reader.IsDBNull(reader.GetOrdinal("TipoEvaluacion")) ? null : reader.GetString(reader.GetOrdinal("TipoEvaluacion")) //byte
                        };
                        acreditadosList.Add(acreditado);
                    }
                }
            }
            return Ok(acreditadosList);
        }
    }
}
