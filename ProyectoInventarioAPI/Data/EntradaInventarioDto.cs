namespace ProyectoInventarioAPI.Dtos
{
    public class EntradaInventarioDto
    {
        public int UsuarioId { get; set; }
        public string? Observaciones { get; set; }
        public List<DetalleEntradaDto> Productos { get; set; } = new List<DetalleEntradaDto>();
    }

    public class DetalleEntradaDto
    {
        // Datos obligatorios para identificar o crear
        public string CodigoBarras { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; } // Precio de compra

        // Datos requeridos SOLO si el producto es NUEVO
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PrecioVenta { get; set; }
        public int? CategoriaId { get; set; }
    }
}