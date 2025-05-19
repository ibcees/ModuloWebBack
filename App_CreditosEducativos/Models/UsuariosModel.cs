using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App_CreditosEducativos.Models;

[Table("usuarios")]
public partial class UsuariosModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string? Nombre { get; set; }

    [Column("usuario")]
    public string? Usuario1 { get; set; } 

    [Column("correo")]
    public string? Correo { get; set; } 

    [Column("contraseña")]
    public string? Contraseña { get; set; }

    [Column("rol")]

    public string? Rol { get; set; }
}
