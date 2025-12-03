using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoInventarioAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNombreAdetalleEntrada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreProducto",
                table: "DetalleEntradas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreProducto",
                table: "DetalleEntradas");
        }
    }
}
