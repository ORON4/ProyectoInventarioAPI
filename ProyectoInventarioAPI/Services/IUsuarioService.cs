using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObtenerTodos();
        Task<Usuario?> ObtenerPorId(int id);
        Task<Usuario> RegistrarUsuario(Usuario usuario);
        Task<Usuario?> ValidarCredenciales(string email, string password); // Para Login
        Task ActualizarUsuario(Usuario usuario);
        
    }
}