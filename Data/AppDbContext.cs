using Microsoft.EntityFrameworkCore;
using NetGuard_GT.Models;

namespace NetGuard_GT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<SitioRed> SitiosRed { get; set; }
        public DbSet<Incidente> Incidentes { get; set; }
        public DbSet<HistorialIncidente> HistorialIncidentes { get; set; }
    }
}