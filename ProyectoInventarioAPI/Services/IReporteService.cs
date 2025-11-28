using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public interface IReporteService
    {
        Task<object> ObtenerVentasHoy();
        // Devuelve todos los productos con stock bajo (StockActual <= StockMinimo)
        Task<IEnumerable<Producto>> ObtenerStockBajo();
        Task<object> ObtenerMasVendidos();
        Task<object> ObtenerVentasPorMetodoPago();
    }
}
