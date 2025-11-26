using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Repositories;

namespace ProyectoInventarioAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IRepository<Categoria> _repository;

        public CategoriaService(IRepository<Categoria> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodas()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Categoria?> ObtenerPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Categoria> CrearCategoria(Categoria categoria)
        {
            // Opcional: Validar que no exista otra categoría con el mismo nombre
            var existente = await _repository.FindAsync(c => c.Nombre == categoria.Nombre);
            if (existente != null)
            {
                throw new Exception($"Ya existe la categoría '{categoria.Nombre}'.");
            }

            await _repository.AddAsync(categoria);
            return categoria;
        }

        public async Task ActualizarCategoria(Categoria categoria)
        {
            await _repository.UpdateAsync(categoria);
        }

        public async Task EliminarCategoria(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}