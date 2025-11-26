using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Services; // Usamos el Servicio

namespace ProyectoInventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        // Inyectamos el Servicio en lugar del Contexto
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _productoService.ObtenerTodos();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return NotFound("Producto no encontrado.");
            return Ok(producto);
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<Producto>> GetProductoPorCodigo(string codigo)
        {
            var producto = await _productoService.ObtenerPorCodigo(codigo);
            if (producto == null) return NotFound("Código de barras no registrado.");
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            try
            {
                var nuevoProducto = await _productoService.CrearProducto(producto);
                return CreatedAtAction("GetProducto", new { id = nuevoProducto.ProductoId }, nuevoProducto);
            }
            catch (Exception ex)
            {
                // Aquí atrapamos las reglas de negocio (ej. código duplicado)
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoId) return BadRequest("El ID no coincide.");

            try
            {
                await _productoService.ActualizarProducto(producto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            await _productoService.EliminarProducto(id);
            return NoContent();
        }
    }
}