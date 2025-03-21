using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar o DbContext para Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Configura��o dos servi�os
builder.Services.AddControllers();

// Adicionar o servi�o Singleton de configura��es
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<SettingsService>(serviceProvider =>
{
    var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
    return SettingsService.GetInstance(appSettings);
});

// Registrar o reposit�rio gen�rico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar reposit�rios espec�ficos
builder.Services.AddScoped<IRegistroHabitoRepository, RegistroHabitoRepository>();
builder.Services.AddScoped<IUnidadeRepository, UnidadeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registrar servi�os de neg�cio
builder.Services.AddScoped<HabitoService>();
builder.Services.AddScoped<RegistroHabitoService>();
builder.Services.AddScoped<UnidadesService>();
builder.Services.AddScoped<UsuariosService>();

// Configura��o do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Challenge Sprint 03 API",
        Version = "v1",
        Description = "Documenta��o da API para o desafio de sprint 03"
    });
});

var app = builder.Build();

// Configura��o do pipeline de requisi��es HTTP
// Ativa o Swagger para todos os ambientes:
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge Sprint 03 API V1");
});

// Redirecionamento HTTPS
app.UseHttpsRedirection();

// Autentica��o/Autoriza��o (se houver)
app.UseAuthorization();

// Mapear os controladores
app.MapControllers();

// Iniciar a aplica��o
app.Run();
