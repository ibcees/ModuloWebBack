using App_CreditosEducativos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace App_CreditosEducativos.Controllers
{
    [ApiController]
    [Route("api/Solicitudes")]
    public class SolicitudesController(IConfiguration configuration) : ControllerBase
    {
        private readonly string? _connectionString = configuration.GetConnectionString("ConexionCreditos");

        [HttpGet("ConsultarSolicitudes")]
        public async Task<ActionResult<IEnumerable<SolicitudesModel>>> GetSolicitudes([FromQuery]
        DateTime fechaEstatusDesde, [FromQuery] string estatusSolicitudIds)
        {
            var solicitudesList = new List<SolicitudesModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SolCredito_Procedure", connection)
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
                            Expr1 = reader.IsDBNull(reader.GetOrdinal("Expr1")) ? 0 :
                            reader.GetInt64(reader.GetOrdinal("Expr1")),
                            Nombre_Solicitante = reader.IsDBNull(reader.GetOrdinal("Nombre"))
                            ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                            Paterno = reader.IsDBNull(reader.GetOrdinal("Paterno")) ? string.Empty :
                            reader.GetString(reader.GetOrdinal("Paterno")),
                            Materno = reader.IsDBNull(reader.GetOrdinal("Materno")) ? string.Empty :
                            reader.GetString(reader.GetOrdinal("Materno")),
                            IdContrato = reader.IsDBNull(reader.GetOrdinal("IdContrato")) ? (long?)null :
                            reader.GetInt64(reader.GetOrdinal("IdContrato")),
                            Folio = reader.IsDBNull(reader.GetOrdinal("Folio")) ? 0 :
                            reader.GetInt64(reader.GetOrdinal("Folio")),
                            Expediente = reader.IsDBNull(reader.GetOrdinal("Expediente")) ? string.Empty :
                            reader.GetString(reader.GetOrdinal("Expediente")),
                            IdTipoCredito = reader.IsDBNull(reader.GetOrdinal("IdTipoCredito")) ? 0 :
                            reader.GetInt32(reader.GetOrdinal("IdTipoCredito")),
                            TipoCreditoDescripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                            ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                            ImporteSolicitado = reader.IsDBNull(reader.GetOrdinal("ImporteSolicitado")) ? 0 :
                            reader.GetDecimal(reader.GetOrdinal("ImporteSolicitado")),
                            TipoSolicitud = reader.IsDBNull(reader.GetOrdinal("TipoSolicitud")) ? string.Empty :
                            reader.GetString(reader.GetOrdinal("TipoSolicitud")),
                            EscTipoSector = reader.IsDBNull(reader.GetOrdinal("EscTipoSector")) ? (byte)0 :
                            reader.GetByte(reader.GetOrdinal("EscTipoSector")),
                            EscNombreOficial = reader.IsDBNull(reader.GetOrdinal("EscNombreOficial"))
                            ? string.Empty : reader.GetString(reader.GetOrdinal("EscNombreOficial")),
                            IdEscuela = reader.IsDBNull(reader.GetOrdinal("IdEscuela")) ? 0 :
                            reader.GetInt64(reader.GetOrdinal("IdEscuela")),
                            IdNivel = reader.IsDBNull(reader.GetOrdinal("IdNivel")) ? 0 :
                            reader.GetInt32(reader.GetOrdinal("IdNivel")),
                            IdCarrera = reader.IsDBNull(reader.GetOrdinal("IdCarrera")) ? (short?)null :
                            reader.GetInt16(reader.GetOrdinal("IdCarrera")),
                            CarreraNombre = reader.IsDBNull(reader.GetOrdinal("CatCNombre")) ? string.Empty :
                            reader.GetString(reader.GetOrdinal("CatCNombre")),
                            Citas = reader.IsDBNull(reader.GetOrdinal("Citas")) ? string.Empty :
                            reader.GetString(reader.GetOrdinal("Citas")),
                            CitaId = reader.IsDBNull(reader.GetOrdinal("CitaId")) ? 0 :
                            reader.GetInt64(reader.GetOrdinal("CitaId")),
                            EsVerificada = reader.IsDBNull(reader.GetOrdinal("EsVerificada")) ? false :
                            reader.GetBoolean(reader.GetOrdinal("EsVerificada")),
                            FechaEstatus = reader.IsDBNull(reader.GetOrdinal("FechaEstatus")) ? (DateTime?)null :
                            reader.GetDateTime(reader.GetOrdinal("FechaEstatus")),
                            EstatusDescripcion = reader.IsDBNull(reader.GetOrdinal("EstatusDescripcion"))
                            ? string.Empty : reader.GetString(reader.GetOrdinal("EstatusDescripcion")),
                            EstatusId = reader.IsDBNull(reader.GetOrdinal("EstatusId")) ? 0 :
                            reader.GetInt32(reader.GetOrdinal("EstatusId"))
                        };
                        solicitudesList.Add(solicitud);
                    }
                }
            }
            return Ok(solicitudesList);
        }

        [HttpGet("Sol_Reno")]
        public async Task<ActionResult<IEnumerable<SolRenoModel>>> GetSolReno([FromQuery] DateTime? fechaEstatusHasta)
        {
            var solicitudesList = new List<SolRenoModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SolReno_Procedure", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                   
                    if (fechaEstatusHasta.HasValue)
                    {
                        command.Parameters.Add(new SqlParameter("@FechaEstatusHasta", fechaEstatusHasta));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@FechaEstatusHasta", DBNull.Value));
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var solicitud = new SolRenoModel
                            {
                                EstatusDescripcion = reader.IsDBNull(reader.GetOrdinal("EstatusDescripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstatusDescripcion")),
                                TotalSolicitudes = reader.IsDBNull(reader.GetOrdinal("TotalSolicitudes")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalSolicitudes")),
                            };
                            solicitudesList.Add(solicitud);
                        }
                    }
                }
            }
            return Ok(solicitudesList);
        }

        [HttpGet("Sol_Nueva")]
        public async Task<ActionResult<IEnumerable<SolRenoModel>>> GetSolNueva([FromQuery] 
        DateTime? fechaEstatusNueva)
        {
            if (fechaEstatusNueva == null)
            {
                Console.WriteLine("El parámetro fechaEstatusDesde es NULL");
            }
            else
            {
                Console.WriteLine($"FechaEstatusDesde: {fechaEstatusNueva}");
            }
            var solicitudesList = new List<SolRenoModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SolNueva_Procedure", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    if (fechaEstatusNueva.HasValue)
                    {
                        command.Parameters.Add(new SqlParameter("@FechaEstatusDesde", fechaEstatusNueva));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@FechaEstatusDesde", DBNull.Value));
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No se encontraron resultados.");
                        }

                        while (await reader.ReadAsync())
                        {
                            var solicitud = new SolRenoModel
                            {
                                EstatusDescripcion = reader.IsDBNull(reader.GetOrdinal("EstatusDescripcion")) ? 
                                string.Empty : reader.GetString(reader.GetOrdinal("EstatusDescripcion")),
                                TotalSolicitudes = reader.IsDBNull(reader.GetOrdinal("TotalSolicitudes")) ? 0 :
                                reader.GetInt32(reader.GetOrdinal("TotalSolicitudes")),
                                ImporteEstimado = reader.IsDBNull(reader.GetOrdinal("ImporteEstimado")) ? 0 : 
                                reader.GetDecimal(reader.GetOrdinal("ImporteEstimado"))
                            };

                            solicitudesList.Add(solicitud);
                        }
                    }
                }
            }
            return Ok(solicitudesList);
        }


        [HttpGet("Sol_NoReno")]
        public async Task<ActionResult<IEnumerable<SolRenoModel>>> GetSolNoReno([FromQuery] DateTime? fechaEstatusNoR)
        {
            var solicitudesList = new List<SolRenoModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SolNoReno_Procedure", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    if (fechaEstatusNoR.HasValue)
                    {
                        command.Parameters.Add(new SqlParameter("@FechaEstatusDesde", fechaEstatusNoR));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@FechaEstatusDesde", DBNull.Value));
                    }
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var solicitud = new SolRenoModel
                            {
                                EstatusDescripcion = reader.IsDBNull(reader.GetOrdinal("EstatusDescripcion")) ? 
                                string.Empty : reader.GetString(reader.GetOrdinal("EstatusDescripcion")),
                                TotalSolicitudes = reader.IsDBNull(reader.GetOrdinal("TotalSolicitudes")) ? 0 : 
                                reader.GetInt32(reader.GetOrdinal("TotalSolicitudes")),
                                ImporteEstimado = reader.IsDBNull(reader.GetOrdinal("ImporteEstimado")) ? 0 : 
                                reader.GetDecimal(reader.GetOrdinal("ImporteEstimado"))
                            };
                            solicitudesList.Add(solicitud);
                        }
                    }
                }
            }
            return Ok(solicitudesList);
        }
    }
}
