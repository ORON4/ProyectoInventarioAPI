using ProyectoInventarioAPI.Models;

namespace ProyectoInventarioAPI.Services
{
    public interface IVentaService
    {
        Task<IEnumerable<Venta>> ObtenerHistorial();
        Task<Venta?> ObtenerPorId(int id);
        Task<Venta> RegistrarVenta(Venta venta);

        // NUEVO: Método para el corte
        Task<ResultadoCorteDto> RealizarCorteDelDia();
    }

    // Clase auxiliar para devolver respuesta
    public class ResultadoCorteDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Transacciones { get; set; }
    }
}