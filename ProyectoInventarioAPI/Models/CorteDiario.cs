using System.ComponentModel.DataAnnotations;

namespace ProyectoInventarioAPI.Models
{
    public class CorteDiario
    {
        [Key]
        public int CorteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int CantidadTransacciones { get; set; }
        public DateTime HoraCorte { get; set; }
    }
}
