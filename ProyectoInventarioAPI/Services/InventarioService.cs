using ProyectoInventarioAPI.Data;
using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Repositories;
using ProyectoInventarioAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ProyectoInventarioAPI.Services
{
    public class InventarioService : IInventarioService
    {
        private readonly IRepository<Producto> _productoRepo;
        private readonly IRepository<EntradaInventario> _entradaRepo;
        private readonly ApplicationDbContext _context; // Para la transacción

        public InventarioService(
            IRepository<Producto> productoRepo,
            IRepository<EntradaInventario> entradaRepo,
            ApplicationDbContext context)
        {
            _productoRepo = productoRepo;
            _entradaRepo = entradaRepo;
            _context = context;
        }

        public async Task<EntradaInventario> RegistrarEntrada(EntradaInventarioDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Crear cabecera de  entrada
                var nuevaEntrada = new EntradaInventario
                {
                    UsuarioId = dto.UsuarioId,
                    FechaRegistro = DateTime.Now,
                    Observaciones = dto.Observaciones,
                    Detalles = new List<DetalleEntrada>(),
                    TotalCosto = 0
                };

                // 2. Procesar cada producto
                foreach (var item in dto.Productos)
                {
                    // Buscar si existe por código de barras
                    var productoExistente = await _productoRepo.FindAsync(p => p.CodigoBarras == item.CodigoBarras);
                    int productoIdFinal;
                    string nombreProductoFinal;

                    if (productoExistente != null)
                    {
                        // --- ESCENARIO A: PRODUCTO YA EXISTE ---
                        // Actualizamos el stock
                        productoExistente.StockActual += item.Cantidad;
                        // Actualizamos el precio de compra (opcional, tomamos el último)
                        productoExistente.PrecioCompra = item.CostoUnitario;

                        await _productoRepo.UpdateAsync(productoExistente);
                        productoIdFinal = productoExistente.ProductoId;
                        nombreProductoFinal = productoExistente.Nombre;
                    }
                    else
                    {
                        // --- ESCENARIO B: PRODUCTO NUEVO ---
                        // Validamos que vengan los datos mínimos
                        if (string.IsNullOrEmpty(item.Nombre) || item.PrecioVenta == null || item.CategoriaId == null)
                        {
                            throw new Exception($"El producto con código {item.CodigoBarras} no existe y faltan datos para crearlo (Nombre, PrecioVenta o CategoriaId).");
                        }

                        var nuevoProducto = new Producto
                        {
                            CodigoBarras = item.CodigoBarras,
                            Nombre = item.Nombre,
                            Descripcion = item.Descripcion,
                            CategoriaId = item.CategoriaId.Value,
                            PrecioCompra = item.CostoUnitario,
                            PrecioVenta = item.PrecioVenta.Value,
                            StockActual = item.Cantidad, // El stock inicial es lo que llegó
                            StockMinimo = 5, // Valor por defecto
                            Activo = true
                        };

                        await _productoRepo.AddAsync(nuevoProducto);
                        // Al hacer AddAsync y SaveChanges (dentro del repo), se genera el ID
                        productoIdFinal = nuevoProducto.ProductoId;
                        nombreProductoFinal = nuevoProducto.Nombre;
                    }

                    // 3. Agregar al detalle de la entrada (Historial)
                    var detalle = new DetalleEntrada
                    {
                        ProductoId = productoIdFinal,
                        NombreProducto = nombreProductoFinal,
                        Cantidad = item.Cantidad,
                        CostoUnitario = item.CostoUnitario
                    };

                    nuevaEntrada.Detalles.Add(detalle);
                    nuevaEntrada.TotalCosto += (item.Cantidad * item.CostoUnitario);
                }

                // 4. Guardar la entrada completa
                await _entradaRepo.AddAsync(nuevaEntrada);

                await transaction.CommitAsync();
                return nuevaEntrada;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<IEnumerable<EntradaResponseDto>> ObtenerHistorial()
        {
            // Traemos las entradas ordenadas por fecha reciente
            var entradas = await _context.EntradasInventario
                .Include(e => e.Usuario) // Para saber quién registró
                .Include(e => e.Detalles) // Para calcular totales o mostrar resumen
                .OrderByDescending(e => e.FechaRegistro)
                .ToListAsync();

            // Mapeamos a DTO
            return entradas.Select(e => new EntradaResponseDto
            {
                EntradaId = e.EntradaId,
                FechaRegistro = e.FechaRegistro,
                UsuarioNombre = e.Usuario?.Nombre ?? "Desconocido",
                TotalCosto = e.TotalCosto,
                Observaciones = e.Observaciones,
                Detalles = null // En el listado general, quizás no necesites cargar todos los detalles para no hacerlo pesado
            });
        }

        public async Task<EntradaResponseDto?> ObtenerPorId(int id)
        {
            var entrada = await _context.EntradasInventario
                .Include(e => e.Usuario)
                .Include(e => e.Detalles)
                .ThenInclude(d => d.Producto) // Incluimos el producto para sacar el código de barras
                .FirstOrDefaultAsync(e => e.EntradaId == id);

            if (entrada == null) return null;

            return new EntradaResponseDto
            {
                EntradaId = entrada.EntradaId,
                FechaRegistro = entrada.FechaRegistro,
                UsuarioNombre = entrada.Usuario?.Nombre ?? "Desconocido",
                TotalCosto = entrada.TotalCosto,
                Observaciones = entrada.Observaciones,
                Detalles = entrada.Detalles.Select(d => new DetalleEntradaResponseDto
                {
                    ProductoId = d.ProductoId,
                    CodigoBarras = d.Producto?.CodigoBarras ?? "N/A",
                    NombreProducto = d.NombreProducto, // Usamos el nombre histórico que guardamos
                    Cantidad = d.Cantidad,
                    CostoUnitario = d.CostoUnitario
                }).ToList()
            };
        }
    }
}