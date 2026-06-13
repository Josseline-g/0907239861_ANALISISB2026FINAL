using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetGuard_GT.Data;
using NetGuard_GT.Models;

namespace NetGuard_GT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TecnicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TecnicosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tecnico>>> GetTecnicos()
        {
            return await _context.Tecnicos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Tecnico>> CrearTecnico(Tecnico tecnico)
        {
            _context.Tecnicos.Add(tecnico);
            await _context.SaveChangesAsync();

            return Ok(tecnico);
        }
    }
}
