using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoInventarioAPI.Models
{
    public class EntradaInventario
    {
        [Key]
        public int EntradaId { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; } // El empleado que recibió la mercancía

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCosto { get; set; } // Cuánto costó esta entrada en total

        public string? Observaciones { get; set; } // Ej: "Factura A-123 de Proveedor X"

        // Relación con el usuario
        public Usuario? Usuario { get; set; }

        // Lista de productos que entraron
        public List<DetalleEntrada> Detalles { get; set; } = new List<DetalleEntrada>();
    }
}