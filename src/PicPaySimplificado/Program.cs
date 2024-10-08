using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Infra;
using PicPaySimplificado.Infra.Repository.Carteiras;
using PicPaySimplificado.Infra.Repository.Transferencias;
using PicPaySimplificado.Services.Autorizador;
using PicPaySimplificado.Services.Carteiras;
using PicPaySimplificado.Services.Notificacoes;
using PicPaySimplificado.Services.Transferencias;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("defaultConnection"), serverVersion));

builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<ITransferenciaRepository, TransferenciaRepository>();
builder.Services.AddScoped<ICarteiraService, CarteiraService>();
builder.Services.AddScoped<ITransferenciaService, TransferenciaService>();

builder.Services.AddHttpClient<IAutorizadorService, AutorizadorService>();
builder.Services.AddScoped<INotificacaoService, NotificationService>();

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