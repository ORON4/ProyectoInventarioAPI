using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoInventarioAPI.Models
{
    public class CorteDiario
    {
        [Key]
        public int CorteId { get; set; }
        public DateTime Fecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public int CantidadTransacciones { get; set; }
        public DateTime HoraCorte { get; set; }
    }
}
