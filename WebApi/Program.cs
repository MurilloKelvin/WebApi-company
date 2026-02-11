using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>() 
        }
    });

});


var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString"); // Obtém a string de conexão do arquivo de configuração (appsettings.json)

builder.Services.AddDbContext<ConnectionDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); // Configura o DbContext para usar MySQL, detectando automaticamente a versão do servidor com base na string de conexão

builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>(); // Registra o repositório para injeção de dependência

var key = Encoding.ASCII.GetBytes(Key.Secret); // Converte a chave secreta para bytes, usada para a assinatura do token JWT

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autenticação padrão como JWT Bearer
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de desafio padrão como JWT Bearer
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Desativa a exigência de HTTPS para o token (não recomendado para produção)
    x.SaveToken = true; // Salva o token no contexto de autenticação
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Habilita a validação da chave de assinatura do token
        IssuerSigningKey = new SymmetricSecurityKey(key), // Define a chave de assinatura do token usando a chave secreta
        ValidateIssuer = false, // Desativa a validação do emissor do token
        ValidateAudience = false // Desativa a validação do público do token
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

