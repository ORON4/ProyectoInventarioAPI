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
    }
}