using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioAPI.Services; 

namespace ProyectoInventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("ventas-hoy")]
        public async Task<IActionResult> GetVentasHoy()
        {
            var reporte = await _reporteService.ObtenerVentasHoy();
            return Ok(reporte);
        }

        [HttpGet("stock-bajo")]
        public async Task<IActionResult> GetStockBajo()
        {
            var productos = await _reporteService.ObtenerStockBajo();
            return Ok(productos);
        }

        [HttpGet("mas-vendidos")]
        public async Task<IActionResult> GetMasVendidos()
        {
            var reporte = await _reporteService.ObtenerMasVendidos();
            return Ok(reporte);
        }

        [HttpGet("metodos-pago")]
        public async Task<IActionResult> GetPorMetodoPago()
        {
            var reporte = await _reporteService.ObtenerVentasPorMetodoPago();
            return Ok(reporte);
        }
    }
}