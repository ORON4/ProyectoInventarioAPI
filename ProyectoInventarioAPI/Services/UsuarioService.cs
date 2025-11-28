using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Repositories;

namespace ProyectoInventarioAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        // Esta variable solo debe aparecer UNA vez
        private readonly IRepository<Usuario> _repository;

        public UsuarioService(IRepository<Usuario> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Usuario?> ObtenerPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Usuario> RegistrarUsuario(Usuario usuario)
        {
            // Regla de Negocio: Email único
            // Usamos FindAsync del repositorio genérico para buscar si ya existe
            var existente = await _repository.FindAsync(u => u.Email == usuario.Email);
            if (existente != null)
            {
                throw new Exception("El correo electrónico ya está registrado.");
            }

            await _repository.AddAsync(usuario);
            return usuario;
        }

        public async Task<Usuario?> ValidarCredenciales(string email, string password)
        {
            // Busca usuario por email y password
            return await _repository.FindAsync(u => u.Email == email && u.PasswordHash == password);
        }

        public async Task ActualizarUsuario(Usuario usuario)
        {
            await _repository.UpdateAsync(usuario);
        }
    }
}