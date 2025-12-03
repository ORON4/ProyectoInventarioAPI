using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Repositories;
using Microsoft.EntityFrameworkCore; // Necesario si quisieras hacer includes manuales, aunque el repo lo abstrae

namespace ProyectoInventarioAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IRepository<Producto> _repository;

        
        public ProductoService(IRepository<Producto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodos()
        {
            
            var productos = await _repository.GetAllAsync();
            
            return productos;
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Producto?> ObtenerPorCodigo(string codigo)
        {
           
            return await _repository.FindAsync(p => p.CodigoBarras == codigo);
        }

        public async Task<Producto> CrearProducto(Producto producto)
        {
            
            var existente = await _repository.FindAsync(p => p.CodigoBarras == producto.CodigoBarras);
            if (existente != null)
            {
                throw new Exception("Ya existe un producto con ese código de barras.");
            }

          
            if (producto.PrecioVenta < producto.PrecioCompra)
            {
                throw new Exception("El precio de venta no puede ser menor al costo.");
            }

            await _repository.AddAsync(producto);
            return producto;
        }

        public async Task ActualizarProducto(Producto producto)
        {
            // Buscamos el producto original en la Base de Datos
            var productoExistente = await _repository.GetByIdAsync(producto.ProductoId);

            if (productoExistente == null)
            {
                throw new Exception("El producto no existe.");
            }

            //  ACTUALIZAMOS SOLO LOS DATOS PERMITIDOS 
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.CategoriaId = producto.CategoriaId;
            productoExistente.PrecioCompra = producto.PrecioCompra;
            productoExistente.PrecioVenta = producto.PrecioVenta;
            productoExistente.StockMinimo = producto.StockMinimo;
            productoExistente.ImagenUrl = producto.ImagenUrl;
            productoExistente.Activo = producto.Activo;

            //Guardamos los cambios
            await _repository.UpdateAsync(productoExistente);
        }

        public async Task EliminarProducto(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}