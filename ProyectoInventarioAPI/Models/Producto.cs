using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoInventarioAPI.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CodigoBarras { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public int CategoriaId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioCompra { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioVenta { get; set; }

        
        public int StockActual { get; set; } = 0;

        public int StockMinimo { get; set; } = 10;

        public string? ImagenUrl { get; set; }

        public bool Activo { get; set; } = true;

        // Relaciones
        public Categoria? Categoria { get; set; }
    }
}
