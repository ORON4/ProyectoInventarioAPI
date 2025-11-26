using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public interface IVentaService
    {
        Task<IEnumerable<Venta>> ObtenerHistorial();
        Task<Venta?> ObtenerPorId(int id);
        Task<Venta> RegistrarVenta(Venta venta);
    }
}