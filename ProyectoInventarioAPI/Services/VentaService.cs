using ProyectoInventarioAPI.Data;
using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProyectoInventarioAPI.Services
{
    public class VentaService : IVentaService
    {
        private readonly IRepository<Venta> _ventaRepository;
        private readonly IRepository<Producto> _productoRepository;
        private readonly ApplicationDbContext _context;

        public VentaService(
            IRepository<Venta> ventaRepository,
            IRepository<Producto> productoRepository,
            ApplicationDbContext context)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _context = context;
        }

        public async Task<IEnumerable<Venta>> ObtenerHistorial()
        {
            return await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();
        }

        public async Task<Venta?> ObtenerPorId(int id)
        {
            return await _context.Ventas
               .Include(v => v.Usuario)
               .Include(v => v.Detalles)
               .ThenInclude(d => d.Producto)
               .FirstOrDefaultAsync(v => v.VentaId == id);
        }

        public async Task<Venta> RegistrarVenta(Venta venta)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                venta.FechaVenta = DateTime.Now;

                if (string.IsNullOrEmpty(venta.NumeroVenta))
                {
                    venta.NumeroVenta = DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                // Lógica de negocio: Validar Stock y Descontar
                foreach (var detalle in venta.Detalles)
                {
                    // Usamos el repositorio de productos para buscar
                    var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);

                    if (producto == null)
                        throw new Exception($"El producto con ID {detalle.ProductoId} no existe.");

                    if (producto.StockActual < detalle.Cantidad)
                        throw new Exception($"Stock insuficiente para: {producto.Nombre}. Stock actual: {producto.StockActual}");

                    // Descontar Stock
                    producto.StockActual -= detalle.Cantidad;

                    // Actualizar Producto en BD
                    await _productoRepository.UpdateAsync(producto);

                    // Asegurar precio histórico
                    detalle.PrecioUnitario = producto.PrecioVenta;
                }

                // Guardar la Venta
                await _ventaRepository.AddAsync(venta);

                await transaction.CommitAsync();
                return venta;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Re-lanzamos el error para que el Controller lo vea
            }
        }

        public async Task<ResultadoCorteDto> RealizarCorteDelDia()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var hoy = DateTime.Today;

                // 1. Obtener ventas de hoy
                var ventasDeHoy = await _context.Ventas
                    .Where(v => v.FechaVenta.Date == hoy) // Ojo: en tu modelo es FechaVenta, no Fecha
                    .ToListAsync();

                if (!ventasDeHoy.Any())
                {
                    return new ResultadoCorteDto { Exito = false, Mensaje = "No hay ventas hoy." };
                }

                // 2. Calcular totales
                decimal total = ventasDeHoy.Sum(v => v.Total);
                int cantidad = ventasDeHoy.Count;

                // 3. Guardar Historial
                var corte = new CorteDiario
                {
                    Fecha = hoy,
                    Total = total,
                    CantidadTransacciones = cantidad,
                    HoraCorte = DateTime.Now
                };
                _context.CortesDiarios.Add(corte);

                // 4. Borrar ventas
                _context.Ventas.RemoveRange(ventasDeHoy);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResultadoCorteDto
                {
                    Exito = true,
                    Mensaje = "Corte realizado correctamente",
                    Total = total,
                    Transacciones = cantidad
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoCorteDto { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }
    }
}