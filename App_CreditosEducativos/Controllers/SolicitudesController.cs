using App_CreditosEducativos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace App_CreditosEducativos.Controllers
{
    [ApiController]
    [Route("api/Solicitudes")]
    public class SolicitudesController : ControllerBase
    {
        private readonly string _connectionString;

        public SolicitudesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexionCreditos");
        }

        [HttpGet("ConsultarSolicitudes")]
        public async Task<ActionResult<IEnumerable<SolicitudesModel>>> GetSolicitudes([FromQuery] DateTime fechaEstatusDesde, [FromQuery] string estatusSolicitudIds)
        {
            var solicitudesList = new List<SolicitudesModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SolicitudesCredito_Procedure", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros del procedimiento
                cmd.Parameters.AddWithValue("@FechaEstatusDesde", fechaEstatusDesde);
                cmd.Parameters.AddWithValue("@EstatusSolicitudIds", estatusSolicitudIds);

                await connection.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        SolicitudesModel solicitud = new SolicitudesModel
                        {
                            Expr1 = reader.GetInt64(reader.GetOrdinal("Expr1")),
                            Nombre_Solicitante = reader.GetString(reader.GetOrdinal("Nombre")),
                            Paterno =  reader.GetString(reader.GetOrdinal("Paterno")),
                            Materno =  reader.GetString(reader.GetOrdinal("Materno")),
                            IdContrato = reader.IsDBNull(reader.GetOrdinal("IdContrato")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("IdContrato")),
                            Folio = reader.GetInt64(reader.GetOrdinal("Folio")),
                            Expediente = reader.GetString(reader.GetOrdinal("Expediente")),
                            IdTipoCredito = reader.GetInt32(reader.GetOrdinal("IdTipoCredito")),
                            TipoCreditoDescripcion =  reader.GetString(reader.GetOrdinal("Descripcion")),
                            ImporteSolicitado = reader.GetDecimal(reader.GetOrdinal("ImporteSolicitado")),
                            TipoSolicitud = reader.GetString(reader.GetOrdinal("TipoSolicitud")),
                            EscTipoSector = reader.GetByte(reader.GetOrdinal("EscTipoSector")),
                            EscNombreOficial = reader.GetString(reader.GetOrdinal("EscNombreOficial")),
                            IdEscuela = reader.GetInt64(reader.GetOrdinal("IdEscuela")),
                            IdNivel = reader.GetInt32(reader.GetOrdinal("IdNivel")),
                            //valor nulo
                            IdCarrera = reader.IsDBNull(reader.GetOrdinal("IdCarrera")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("IdCarrera")),
                            CarreraNombre =  reader.GetString(reader.GetOrdinal("CatCNombre")),
                            Citas = reader.GetString(reader.GetOrdinal("Citas")),
                            CitaId =  reader.GetInt64(reader.GetOrdinal("CitaId")),
                            EsVerificada = reader.GetBoolean(reader.GetOrdinal("EsVerificada")),
                            FechaEstatus = reader.IsDBNull(reader.GetOrdinal("FechaEstatus")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaEstatus")),
                            EstatusDescripcion = reader.GetString(reader.GetOrdinal("EstatusDescripcion")),
                            EstatusId = reader.GetInt32(reader.GetOrdinal("EstatusId"))
                        };

                        solicitudesList.Add(solicitud);
                    }
                }
            }

            return Ok(solicitudesList);
        }
    }
}
