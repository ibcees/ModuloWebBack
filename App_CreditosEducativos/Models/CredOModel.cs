namespace App_CreditosEducativos.Models
{
    public class CreditoDetalle
    {
        public string? TipoCredito { get; set; }
        public string? TipoEvaluacion { get; set; }
        public int Beneficiarios { get; set; }
        public int Contratos { get; set; }
        public decimal Importe { get; set; }
    }

    public class CreditoTotal
    {
        public string? Concepto { get; set; }
        public decimal Valor { get; set; }
    }
}
