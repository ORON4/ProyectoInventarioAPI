using System.ComponentModel.DataAnnotations;

namespace ProyectoInventarioAPI.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Roles
        [Required]
        public string Rol { get; set; } = "Cajero";

        public bool Activo { get; set; } = true;
    }
}
