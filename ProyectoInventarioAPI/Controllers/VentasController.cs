using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("CorteDelDia")]
        public async Task<IActionResult> RealizarCorteDelDia()
        {
            // Usamos una transacción para asegurar que no se borre nada si falla el reporte
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var hoy = DateTime.Today;

                // 1. Obtener las ventas de hoy
                var ventasDeHoy = await _context.Ventas
                    .Where(v => v.Fecha.Date == hoy)
                    .ToListAsync();

                if (!ventasDeHoy.Any())
                {
                    return BadRequest("No hay ventas registradas para el día de hoy.");
                }

                // 2. Calcular los datos del corte
                decimal totalVendido = ventasDeHoy.Sum(v => v.Total);
                int cantidadVentas = ventasDeHoy.Count;

                // 3. (Opcional) Guardar en una tabla de Historial de Cortes
                // Esto es CRÍTICO para no perder la contabilidad financiera
                var nuevoCorte = new CorteDiario
                {
                    Fecha = hoy,
                    Total = totalVendido,
                    CantidadTransacciones = cantidadVentas,
                    HoraCorte = DateTime.Now
                };
                _context.CortesDiarios.Add(nuevoCorte);

                // 4. Borrar las ventas individuales (según tu requerimiento)
                _context.Ventas.RemoveRange(ventasDeHoy);

                // 5. Guardar cambios y confirmar transacción
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 6. Retornar el resumen al frontend
                return Ok(new
                {
                    Mensaje = "Corte realizado con éxito",
                    Total = totalVendido,
                    Transacciones = cantidadVentas
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al realizar el corte: {ex.Message}");
            }
        }
    }
}