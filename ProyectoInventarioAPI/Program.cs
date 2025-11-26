using Microsoft.EntityFrameworkCore;
using ProyectoInventarioAPI.Data;
using ProyectoInventarioAPI.Repositories;
using ProyectoInventarioAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>(); 
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
