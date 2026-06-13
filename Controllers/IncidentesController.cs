using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetGuard_GT.Data;
using NetGuard_GT.Models;

namespace NetGuard_GT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IncidentesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incidente>>> GetIncidentes()
        {
            return await _context.Incidentes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Incidente>> CrearIncidente(Incidente incidente)
        {
            incidente.Estado = "Registrado";
            incidente.FechaCreacion = DateTime.Now;

            incidente.FechaLimiteResolucion =
                incidente.Severidad.ToLower() switch
                {
                    "critica" => DateTime.Now.AddHours(4),
                    "urgente" => DateTime.Now.AddHours(8),
                    "alta" => DateTime.Now.AddHours(24),
                    "media" => DateTime.Now.AddHours(48),
                    "baja" => DateTime.Now.AddHours(72),
                    _ => DateTime.Now.AddHours(48)
                };

            _context.Incidentes.Add(incidente);
            await _context.SaveChangesAsync();

            return Ok(incidente);
        }

        [HttpPut("{incidenteId}/asignar/{tecnicoId}")]
        public async Task<ActionResult> AsignarTecnico(int incidenteId, int tecnicoId)
        {
            var incidente = await _context.Incidentes.FindAsync(incidenteId);

            if (incidente == null)
                return NotFound("Incidente no encontrado.");

            var tecnico = await _context.Tecnicos.FindAsync(tecnicoId);

            if (tecnico == null)
                return NotFound("Técnico no encontrado.");

            if (!tecnico.Activo)
                return BadRequest("El técnico no está activo.");

            if (tecnico.Especialidad.ToLower() != incidente.TipoIncidente.ToLower())
                return BadRequest("El técnico no tiene la especialidad requerida para este incidente.");

            var incidentesActivos = await _context.Incidentes
                .CountAsync(i => i.TecnicoId == tecnicoId &&
                                 i.Estado != "Cerrado" &&
                                 i.Estado != "Resuelto");

            if (incidentesActivos >= 3)
                return BadRequest("El técnico ya tiene 3 incidentes activos asignados.");

            var estadoAnterior = incidente.Estado;

            incidente.TecnicoId = tecnicoId;
            incidente.Estado = "Asignado";

            var historial = new HistorialIncidente
            {
                IncidenteId = incidente.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = "Asignado",
                Accion = "Asignación de técnico",
                FechaCambio = DateTime.Now,
                Responsable = tecnico.Nombre
            };

            _context.HistorialIncidentes.Add(historial);
            await _context.SaveChangesAsync();

            return Ok("Técnico asignado correctamente.");
        }

        [HttpGet("historial")]
        public async Task<ActionResult<IEnumerable<HistorialIncidente>>> GetHistorial()
        {
            return await _context.HistorialIncidentes.ToListAsync();
        }

        [HttpPut("{id}/estado")]
        public async Task<ActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            var incidente = await _context.Incidentes.FindAsync(id);

            if (incidente == null)
                return NotFound("Incidente no encontrado.");

            bool valido =
                (incidente.Estado == "Registrado" && nuevoEstado == "Asignado") ||
                (incidente.Estado == "Asignado" && nuevoEstado == "En Proceso") ||
                (incidente.Estado == "En Proceso" && nuevoEstado == "Resuelto") ||
                (incidente.Estado == "Resuelto" && nuevoEstado == "Cerrado");

            if (!valido)
                return BadRequest("Transición de estado no permitida.");

            var historial = new HistorialIncidente
            {
                IncidenteId = incidente.Id,
                EstadoAnterior = incidente.Estado,
                EstadoNuevo = nuevoEstado,
                Accion = "Cambio de estado",
                FechaCambio = DateTime.Now,
                Responsable = "Sistema"
            };

            incidente.Estado = nuevoEstado;

            _context.HistorialIncidentes.Add(historial);
            await _context.SaveChangesAsync();

            return Ok("Estado actualizado correctamente.");
        }

        [HttpPut("{incidenteId}/reasignar/{nuevoTecnicoId}")]
        public async Task<ActionResult> ReasignarTecnico(int incidenteId, int nuevoTecnicoId)
        {
            var incidente = await _context.Incidentes.FindAsync(incidenteId);

            if (incidente == null)
                return NotFound("Incidente no encontrado.");

            var nuevoTecnico = await _context.Tecnicos.FindAsync(nuevoTecnicoId);

            if (nuevoTecnico == null)
                return NotFound("Nuevo técnico no encontrado.");

            if (!nuevoTecnico.Activo)
                return BadRequest("El nuevo técnico no está activo.");

            if (nuevoTecnico.Especialidad.ToLower() != incidente.TipoIncidente.ToLower())
                return BadRequest("El nuevo técnico no tiene la especialidad requerida.");

            var incidentesActivos = await _context.Incidentes
                .CountAsync(i => i.TecnicoId == nuevoTecnicoId &&
                                 i.Estado != "Cerrado" &&
                                 i.Estado != "Resuelto");

            if (incidentesActivos >= 3)
                return BadRequest("El nuevo técnico ya tiene 3 incidentes activos asignados.");

            var tecnicoAnteriorId = incidente.TecnicoId;

            incidente.TecnicoId = nuevoTecnicoId;

            var historial = new HistorialIncidente
            {
                IncidenteId = incidente.Id,
                EstadoAnterior = incidente.Estado,
                EstadoNuevo = incidente.Estado,
                Accion = $"Reasignación de técnico. Técnico anterior ID: {tecnicoAnteriorId}, nuevo técnico ID: {nuevoTecnicoId}",
                FechaCambio = DateTime.Now,
                Responsable = nuevoTecnico.Nombre
            };

            _context.HistorialIncidentes.Add(historial);
            await _context.SaveChangesAsync();

            return Ok("Incidente reasignado correctamente.");
        }

        [HttpGet("reporte/estado")]
        public async Task<ActionResult> ReportePorEstado()
        {
            var reporte = await _context.Incidentes
                .GroupBy(i => i.Estado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            return Ok(reporte);
        }

        [HttpGet("reporte/severidad")]
        public async Task<ActionResult> ReportePorSeveridad()
        {
            var reporte = await _context.Incidentes
                .GroupBy(i => i.Severidad)
                .Select(g => new
                {
                    Severidad = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            return Ok(reporte);
        }
        [HttpGet("reporte/escalados")]
        public async Task<ActionResult> ReporteEscalados()
        {
            var totalEscalados = await _context.Incidentes
                .CountAsync(i => i.Escalado == true);

            return Ok(new
            {
                TotalEscalados = totalEscalados
            });
        }

        [HttpGet("reporte/tecnico")]
        public async Task<ActionResult> ReportePorTecnico()
        {
            var reporte = await _context.Incidentes
                .Where(i => i.TecnicoId != null)
                .GroupBy(i => i.TecnicoId)
                .Select(g => new
                {
                    TecnicoId = g.Key,
                    TotalIncidentes = g.Count()
                })
                .ToListAsync();

            return Ok(reporte);
        }
    }
}
