using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Repositories;
using Microsoft.EntityFrameworkCore; // Necesario si quisieras hacer includes manuales, aunque el repo lo abstrae

namespace ProyectoInventarioAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IRepository<Producto> _repository;

        // Inyectamos el Repositorio, NO el DbContext directamente
        public ProductoService(IRepository<Producto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodos()
        {
            // Aquí podríamos filtrar solo productos activos, por ejemplo:
            var productos = await _repository.GetAllAsync();
            // Nota: Con el repositorio genérico básico, no traemos la "Categoria" (Join).
            // Si necesitas la categoría, tendrías que modificar el repositorio para aceptar "Includes"
            // o crear un ProductoRepository específico. Por ahora, funcionará básico.
            return productos;
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Producto?> ObtenerPorCodigo(string codigo)
        {
            // Usamos el método FindAsync del repositorio genérico
            return await _repository.FindAsync(p => p.CodigoBarras == codigo);
        }

        public async Task<Producto> CrearProducto(Producto producto)
        {
            // Regla de Negocio: Validar código duplicado
            var existente = await _repository.FindAsync(p => p.CodigoBarras == producto.CodigoBarras);
            if (existente != null)
            {
                throw new Exception("Ya existe un producto con ese código de barras.");
            }

            // Regla de Negocio: Validar precios
            if (producto.PrecioVenta < producto.PrecioCompra)
            {
                throw new Exception("El precio de venta no puede ser menor al costo.");
            }

            await _repository.AddAsync(producto);
            return producto;
        }

        public async Task ActualizarProducto(Producto producto)
        {
            await _repository.UpdateAsync(producto);
        }

        public async Task EliminarProducto(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}