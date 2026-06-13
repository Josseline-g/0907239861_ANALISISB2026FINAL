using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetGuard_GT.Data;
using NetGuard_GT.Models;


namespace NetGuard_GT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitiosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SitiosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SitioRed>>> GetSitios()
        {
            return await _context.SitiosRed.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SitioRed>> CrearSitio(SitioRed sitio)
        {
            _context.SitiosRed.Add(sitio);
            await _context.SaveChangesAsync();

            return Ok(sitio);
        }
    }
}
