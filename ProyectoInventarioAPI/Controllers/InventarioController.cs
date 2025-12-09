using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioAPI.Dtos; 
using ProyectoInventarioAPI.Services;

namespace ProyectoInventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _inventarioService;

        public InventarioController(IInventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        [HttpPost("entrada")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] EntradaInventarioDto dto)
        {
            try
            {
                var entrada = await _inventarioService.RegistrarEntrada(dto);
                return Ok(new { Mensaje = "Entrada registrada exitosamente", EntradaId = entrada.EntradaId, Total = entrada.TotalCosto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntradaResponseDto>>> GetHistorial()
        {
            var historial = await _inventarioService.ObtenerHistorial();
            return Ok(historial);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntradaResponseDto>> GetEntrada(int id)
        {
            var entrada = await _inventarioService.ObtenerPorId(id);
            
            if (entrada == null)
            {
                return NotFound("No se encontró la entrada de inventario.");
            }

            return Ok(entrada);
        }
    }
}