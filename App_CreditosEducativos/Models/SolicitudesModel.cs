using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App_CreditosEducativos.Models
{
    public class SolicitudesModel
    {
        public long Expr1 { get; set; } 
        public string? Nombre_Solicitante { get; set; }
        public string? Paterno { get; set; }
        public string? Materno { get; set; }
        public long? IdContrato { get; set; }
        public long Folio { get; set; }
        public string? Expediente { get; set; }
        public int IdTipoCredito { get; set; } 
        public string? TipoCreditoDescripcion { get; set; }
        public decimal ImporteSolicitado { get; set; } 
        public string? TipoSolicitud { get; set; }
        public byte EscTipoSector { get; set; } 
        public string? EscNombreOficial { get; set; }
        public long IdEscuela { get; set; }
        public int IdNivel { get; set; } 
        public short? IdCarrera { get; set; } 
        public string? CarreraNombre { get; set; }
        public string? Citas { get; set; }
        public long CitaId { get; set; }
        public bool EsVerificada { get; set; } 
        public DateTime? FechaEstatus { get; set; } 
        public string? EstatusDescripcion { get; set; }
        public int EstatusId { get; set; } 
    }

}
