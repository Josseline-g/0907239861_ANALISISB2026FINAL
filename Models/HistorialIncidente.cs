namespace NetGuard_GT.Models
{
    public class HistorialIncidente
    {
        public int Id { get; set; }
        public int IncidenteId { get; set; }
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; } = DateTime.Now;
        public string Responsable { get; set; } = string.Empty;
    }
}
