using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodos();
        Task<Producto?> ObtenerPorId(int id);
        Task<Producto?> ObtenerPorCodigo(string codigo);
        Task<Producto> CrearProducto(Producto producto);
        Task ActualizarProducto(Producto producto);
        Task EliminarProducto(int id);
    }
}