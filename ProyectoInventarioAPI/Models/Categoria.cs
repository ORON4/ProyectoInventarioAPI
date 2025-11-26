using System.ComponentModel.DataAnnotations;

namespace ProyectoInventarioAPI.Models
{
    public class Categoria
    {

        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación: Una categoría tiene muchos productos
        public ICollection<Producto>? Productos { get; set; }
    }
}
