using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Dtos; 

namespace ProyectoInventarioAPI.Services
{
    public interface IInventarioService
    {
        Task<EntradaInventario> RegistrarEntrada(EntradaInventarioDto entradaDto);
        Task<IEnumerable<EntradaResponseDto>> ObtenerHistorial();
        Task<EntradaResponseDto?> ObtenerPorId(int id);
    }
}