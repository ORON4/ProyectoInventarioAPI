using ProyectoInventarioAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace ProyectoInventarioAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets: Representan tus tablas
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        // Agrega aquí MovimientosInventario y Devoluciones si las implementas igual

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales (Fluent API)

            // Ejemplo: Índice único para CodigoBarras (Optimización que discutimos)
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoBarras)
                .IsUnique();

            // Ejemplo: Índice para búsquedas rápidas de ventas por fecha
            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.FechaVenta);
        }
    }
}
