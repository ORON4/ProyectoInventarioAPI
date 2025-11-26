using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Services;

namespace ProyectoInventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return Ok(await _categoriaService.ObtenerTodas());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _categoriaService.ObtenerPorId(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            try
            {
                var nueva = await _categoriaService.CrearCategoria(categoria);
                return CreatedAtAction("GetCategoria", new { id = nueva.CategoriaId }, nueva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId) return BadRequest();
            await _categoriaService.ActualizarCategoria(categoria);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            await _categoriaService.EliminarCategoria(id);
            return NoContent();
        }
    }
}