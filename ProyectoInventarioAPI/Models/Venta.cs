using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProyectoInventarioAPI.Models
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }

        [Required]
        public string NumeroVenta { get; set; } = string.Empty; // UUID o Folio

        public int UsuarioId { get; set; } // Empleado que hizo la venta

        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Impuestos { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // "Efectivo", "Tarjeta", "Transferencia" [cite: 216]
        public string MetodoPago { get; set; } = "Efectivo";

        // "Completada", "Cancelada", "Devuelta"
        public string Estado { get; set; } = "Completada";

        public string? Observaciones { get; set; }

        // Relaciones
        public Usuario? Usuario { get; set; }
        public ICollection<DetalleVenta>? Detalles { get; set; }
    }
}
