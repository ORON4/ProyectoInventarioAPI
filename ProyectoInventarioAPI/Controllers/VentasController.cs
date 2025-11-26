using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Services;

namespace ProyectoInventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            var ventas = await _ventaService.ObtenerHistorial();
            return Ok(ventas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _ventaService.ObtenerPorId(id);
            if (venta == null) return NotFound();
            return Ok(venta);
        }

        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(Venta venta)
        {
            try
            {
                var nuevaVenta = await _ventaService.RegistrarVenta(venta);
                return CreatedAtAction("GetVenta", new { id = nuevaVenta.VentaId }, nuevaVenta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}