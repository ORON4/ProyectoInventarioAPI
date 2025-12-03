namespace ProyectoInventarioAPI.Dtos
{
    public class EntradaResponseDto
    {
        public int EntradaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty; // Nombre del empleado
        public decimal TotalCosto { get; set; }
        public string? Observaciones { get; set; }

        // Lista de productos de esa entrada
        public List<DetalleEntradaResponseDto> Detalles { get; set; } = new();
    }

    public class DetalleEntradaResponseDto
    {
        public int ProductoId { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty; // El nombre histórico
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal => Cantidad * CostoUnitario; // Calculado
    }
}