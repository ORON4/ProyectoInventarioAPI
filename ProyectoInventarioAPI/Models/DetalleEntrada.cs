using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoInventarioAPI.Models
{
    public class DetalleEntrada
    {
        [Key]
        public int DetalleEntradaId { get; set; }

        public int EntradaId { get; set; }
        public string NombreProducto {get; set; } = string.Empty;

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostoUnitario { get; set; } // A cómo se compró 

        // Relaciones
        public EntradaInventario? Entrada { get; set; }
        public Producto? Producto { get; set; }
    }
}