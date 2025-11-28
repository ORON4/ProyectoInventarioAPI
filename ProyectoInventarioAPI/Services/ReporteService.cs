using Microsoft.EntityFrameworkCore;
using ProyectoInventarioAPI.Data;
using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public class ReporteService : IReporteService
    {
        private readonly ApplicationDbContext _context;

        public ReporteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> ObtenerVentasHoy()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            var ventasHoy = await _context.Ventas
                .Where(v => v.FechaVenta >= hoy && v.FechaVenta < manana)
                .Include(v => v.Usuario)
                .ToListAsync();

            return new
            {
                Fecha = hoy.ToShortDateString(),
                TotalIngresos = ventasHoy.Sum(v => v.Total),
                CantidadVentas = ventasHoy.Count,
                Ventas = ventasHoy
            };
        }

        public async Task<IEnumerable<Producto>> ObtenerStockBajo()
        {
            return await _context.Productos
                .Where(p => p.StockActual <= p.StockMinimo && p.Activo)
                .ToListAsync();
        }

        public async Task<object> ObtenerMasVendidos()
        {
            var masVendidos = await _context.DetalleVentas
                .Include(d => d.Producto)
                .GroupBy(d => d.ProductoId)
                .Select(g => new
                {
                    Producto = g.First().Producto.Nombre,
                    CantidadTotal = g.Sum(d => d.Cantidad),
                    Ingresos = g.Sum(d => d.Cantidad * d.PrecioUnitario)
                })
                .OrderByDescending(r => r.CantidadTotal)
                .Take(5) // Top 5
                .ToListAsync();

            return masVendidos;
        }

        public async Task<object> ObtenerVentasPorMetodoPago()
        {
            var resultado = await _context.Ventas
                .GroupBy(v => v.MetodoPago)
                .Select(g => new
                {
                    Metodo = g.Key,
                    Total = g.Sum(v => v.Total),
                    Cantidad = g.Count()
                })
                .ToListAsync();

            return resultado;
        }
    }
}