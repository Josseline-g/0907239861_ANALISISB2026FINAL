namespace NetGuard_GT.Models
{
    public class Incidente
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoIncidente { get; set; } = string.Empty;
        public string Severidad { get; set; } = string.Empty;
        public string Estado { get; set; } = "Registrado";
        public bool Escalado { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaLimiteResolucion { get; set; }

        public int SitioRedId { get; set; }
        public int? TecnicoId { get; set; }
    }
}
