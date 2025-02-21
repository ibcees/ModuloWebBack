using Microsoft.EntityFrameworkCore;
namespace App_CreditosEducativos.Models
{
    public class AcreditadosModel
    {
        public long Expediente { get; set; }
        public string Nombre_Acreditado { get; set; }
        public String? Sexo { get; set; }
        public string CiudadLocalidad { get; set; }
        public byte? Contrato { get; set; }
        public string? TipoCredito { get; set; }
        public DateTime? FechaTermEstudios { get; set; }
        public long CampusID { get; set; }
        public string NombreEscuela { get; set; }
        public string? TipoEscuela { get; set; }
        public string? Estrato { get; set; }
        public string? Nivel { get; set; }
        public short? Semestre { get; set; }
        public string CarreraNombre { get; set; }
        public decimal? Capital { get; set; }
        public string? Sucursal { get; set; }
        public string MunicipioFam { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public DateTime FechaAutorizacion { get; set; }
        public int? CampusEstadoID { get; set; }
        public string? TipoEvaluacion { get; set; }
    }
}


