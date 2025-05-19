using App_CreditosEducativos.Data;
using App_CreditosEducativos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace App_CreditosEducativos.Controllers
{
    [ApiController]
    [Route("api/Usuarios")]
    public class UsuariosController(IConfiguration configuration) : ControllerBase
    {
        private readonly string? _connectionString = configuration.GetConnectionString("ConexionCreditos");

        // GET: api/Usuarios/ConsultarUsuarios
        [HttpGet]
        [Route("ConsultarUsuarios")]
        public async Task<ActionResult<List<UsuariosModel>>> GetUsuarios()
        {
            List<UsuariosModel> usuarios = new List<UsuariosModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("ConsultaUsuarios_Proc", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                Thread.Sleep(2000);

                await connection.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var usuario = new UsuariosModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Usuario1 = reader.GetString(reader.GetOrdinal("Usuario")),
                            Correo = reader.GetString(reader.GetOrdinal("Correo")),
                            Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                            Rol = reader.GetString(reader.GetOrdinal("Rol")) 
                        };
                        usuarios.Add(usuario);
                    }
                }
            }

            return Ok(usuarios);
        }


        // POST: api/Usuarios
        [HttpPost("RegistrarUsuario")]
        public async Task<ActionResult<string>> CreateUsuario([FromBody] UsuariosModel usuario)
        {
            int nuevoId = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                var addCmd = new SqlCommand("InsertarUsuario_Proc", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                addCmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                addCmd.Parameters.AddWithValue("@usuario", usuario.Usuario1);
                addCmd.Parameters.AddWithValue("@correo", usuario.Correo);
                addCmd.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                addCmd.Parameters.AddWithValue("@rol", usuario.Rol);

                await connection.OpenAsync();
                nuevoId = Convert.ToInt32(await addCmd.ExecuteScalarAsync());
            }

            if (nuevoId == -1)
            {
                return Conflict("El usuario o correo ya están registrados.");
            }

            return Ok(new { mensaje = $"Usuario registrado exitosamente con ID: {nuevoId}" });
        }


        [HttpPut("ModificarUsuario")]
        public async Task<ActionResult> ActualizarUsuario([FromQuery] int id, [FromBody] 
        UsuariosModel usuario)
        {
            if (usuario == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            if (id != usuario.Id)
            {
                return BadRequest("El ID del usuario no coincide con el ID de la ruta.");
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    // Verificar si el usuario existe
                    using (var checkCommand = new SqlCommand("SELECT COUNT(*) FROM usuarios WHERE id = @id", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@id", id);
                        int? userExists = (int?)await checkCommand.ExecuteScalarAsync();

                        if (userExists == 0)
                        {
                            return NotFound("No se encontró el usuario con el ID proporcionado.");
                        }
                    }
                    // Actualizar el usuario
                    using (var updateCommand = new SqlCommand("ActualizarUsuario_Proc", connection))
                    {
                        updateCommand.CommandType = CommandType.StoredProcedure;
                        updateCommand.Parameters.AddWithValue("@id", usuario.Id);
                        updateCommand.Parameters.AddWithValue("@nombre",usuario.Nombre);
                        updateCommand.Parameters.AddWithValue("@usuario", usuario.Usuario1);
                        updateCommand.Parameters.AddWithValue("@correo", usuario.Correo);
                        updateCommand.Parameters.AddWithValue("@contraseña", usuario.Contraseña ?? (object)DBNull.Value);
                        updateCommand.Parameters.AddWithValue("@rol", usuario.Rol);

                        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            return StatusCode(500, "No se pudo actualizar el usuario.");
                        }
                    }
                }
                return Ok(new { mensaje = "Usuario actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error en ActualizarUsuario: {ex}");
                return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
            }
        }

        // DELETE: api/Usuarios/{id}
        [HttpDelete("BorrarUsuario")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var deleteCmd = new SqlCommand("EliminarUsuario_Proc", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                deleteCmd.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                await deleteCmd.ExecuteNonQueryAsync(); // Ejecuta la eliminación
            }

           return Ok(new { message = "Usuario eliminado exitosamente" });

        }

        // GET: api/Usuarios/ConsultarUsuario
        [HttpGet]
        [Route("ConsultarUsuario")]
        public async Task<ActionResult<UsuariosModel>> GetUsuario([FromQuery] int id)
        {
            UsuariosModel? usuario = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var getCmd = new SqlCommand("ConsultaUsuarios_Proc", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    // parámetro de ID
                    getCmd.Parameters.AddWithValue("@id", id);

                    await connection.OpenAsync();  // Conexión asincrónica

                    using (var reader = await getCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Mapeo del resultado a un modelo de usuario
                            usuario = new UsuariosModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Usuario1 = reader.GetString(reader.GetOrdinal("Usuario")),
                                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                                Rol = reader.GetString(reader.GetOrdinal("Rol"))
                            };
                        }
                    }
                }

                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {id} no encontrado.");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener el usuario: {ex.Message}");
            }
        }

        [HttpPut("InicioSesion")]
        public async Task<ActionResult> InicioSesion([FromQuery] string usuario1, [FromQuery] string contraseña)
        {
            if (string.IsNullOrWhiteSpace(usuario1) || string.IsNullOrWhiteSpace(contraseña))
            {
                return BadRequest("Debe proporcionar usuario y contraseña.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Usuario = @usuario", connection);
                cmd.Parameters.AddWithValue("@usuario", usuario1);

                await connection.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string contraseñaBD = reader.GetString(reader.GetOrdinal("Contraseña"));

                        if (contraseñaBD == contraseña)
                        {
                            return Ok(new
                            {
                                mensaje = "Inicio de sesión exitoso",
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Usuario1 = reader.GetString(reader.GetOrdinal("Usuario")),
                                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                Rol = reader.GetString(reader.GetOrdinal("Rol")),
                            });
                        }
                        else
                        {
                            return Unauthorized("Contraseña incorrecta.");
                        }
                    }
                    else
                    {
                        return NotFound("Usuario no encontrado.");
                    }
                }
            }
        }
    }
 }

