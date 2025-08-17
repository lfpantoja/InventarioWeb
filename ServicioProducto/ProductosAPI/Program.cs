using Microsoft.EntityFrameworkCore;
using ProductosAplicacion.Abstracciones;
using ProductosInfraestructura.Persistencia;
using ProductosInfraestructura.Servicios;

var builder = WebApplication.CreateBuilder(args);

const string politicaCors = "PermitirAngular";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(politicaCors, p =>
        p.WithOrigins("http://localhost:4200") // URL del front en dev
         .AllowAnyHeader()
         .AllowAnyMethod()
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string desde appsettings.json
builder.Services.AddDbContext<ContextoProductos>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Inyección del servicio de aplicación
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(politicaCors);

app.MapControllers();

app.Run();
