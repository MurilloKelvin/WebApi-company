using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi;
using WebApi.Infrastructure.Repositories;
using WebApi.Application.Mapping;
using WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(typeof(DomainToDTOMapping)); // Registra o AutoMapper e especifica a classe de mapeamento do employee

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(o => // Configura a versionamento da API
{
    o.AssumeDefaultVersionWhenUnspecified = true; // Assume a versão padrão quando nenhuma versão é especificada na requisição
    o.DefaultApiVersion = new ApiVersion(1, 0); // Define a versão padrão como 1.0
}); 

builder.Services.AddVersionedApiExplorer(o => // Configura o explorador de API versionada para gerar documentação específica para cada versão
{
    o.GroupNameFormat = "'v'VVV"; // Define o formato do nome do grupo de versões (ex: v1, v2)
    o.SubstituteApiVersionInUrl = true; // Substitui a versão na URL da documentação
});

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerDefaultValues>(); // Adiciona um filtro para definir valores padrão na documentação Swagger

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


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>(); // Registra o repositório para injeção de dependência
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>(); // Registra a configuração personalizada para o Swagger

// Configura o CORS para permitir solicitações de qualquer origem, método e cabeçalho, facilitando o desenvolvimento e testes da API
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.AllowAnyOrigin() // Permite solicitações de qualquer origem
                  .AllowAnyMethod() // Permite qualquer método HTTP (GET, POST, etc.)
                  .AllowAnyHeader(); // Permite qualquer cabeçalho
        });
});


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
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>(); // Obtém o provedor de descrição de versão da API para configurar o Swagger


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development"); // usa o manipulador de exceções para desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions) // Itera sobre as descrições de versão da API para configurar os endpoints do Swagger para cada versão
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", // Define o endpoint do Swagger para cada versão da API, usando o nome do grupo de versão como parte da URL e do título
                $"Web API - {description.GroupName.ToUpperInvariant()}");
        }
    });

}else 
{
    app.UseExceptionHandler("/error"); // usa o manipulador de exceções para produção
    app.UseHsts();
}
app.UseCors("MyPolicy"); // Aplica a política de CORS definida anteriormente para permitir solicitações de qualquer origem, método e cabeçalho
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

