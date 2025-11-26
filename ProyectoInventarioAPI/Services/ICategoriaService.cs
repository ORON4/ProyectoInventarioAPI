using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ObtenerTodas();
        Task<Categoria?> ObtenerPorId(int id);
        Task<Categoria> CrearCategoria(Categoria categoria);
        Task ActualizarCategoria(Categoria categoria);
        Task EliminarCategoria(int id);
    }
}