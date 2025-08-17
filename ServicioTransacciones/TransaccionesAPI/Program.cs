using Microsoft.EntityFrameworkCore;
using TransaccionesAplicacion.Abstracciones;
using TransaccionesAplicacion.Externos;
using TransaccionesInfraestructura.Externos;
using TransaccionesInfraestructura.Persistencia;
using TransaccionesInfraestructura.Servicios;

var builder = WebApplication.CreateBuilder(args);

const string politicaCors = "PermitirAngular";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(politicaCors, p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextoTransacciones>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHttpClient<IProductosClient, ProductosClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Servicios:Productos:BaseUrl"]!);
});

builder.Services.AddScoped<ITransaccionServicio, TransaccionServicio>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(politicaCors);

app.MapControllers();
app.Run();
