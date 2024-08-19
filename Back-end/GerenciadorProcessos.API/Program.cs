using GerenciadorProcessos.Application.Extensions;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Infra;
using GerenciadorProcessos.Infra.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddScoped<IProcessoRepository, ProcessoRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();

builder.Services.Configurar();

builder.Services.AddCors();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.MapControllers();

app.Run();

