using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Chaira.MesaServicio.Api.Middleware;
using Chaira.MesaServicio.Application;
using Chaira.MesaServicio.Infrastructure;
using MongoDB.Driver;
using DotNetEnv;

// Cargar variables de entorno desde .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger + Redoc
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Chaira API",
        Version = "v1",
        Description = "API Mesa de Servicios - Sistema de GestiÃ³n de Soporte TÃ©cnico"
    });

    // ConfiguraciÃ³n para JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT de la aplicaciÃ³n madre.\n\nEjemplo: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Entity Framework - Usar variables de entorno
var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("SqlServerConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// MongoDB (Evidencias / Soportes)
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("MongoConnection")
    ?? throw new InvalidOperationException("MongoConnection no configurada");
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME")
    ?? builder.Configuration["DatabaseSettings:MongoDatabaseName"]
    ?? throw new InvalidOperationException("MongoDatabaseName no configurada");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});

// Repositorios y Servicios
builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();

// JWT Authentication (validaciÃ³n de tokens de la aplicaciÃ³n madre)
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
    ?? builder.Configuration["JwtSettings:SecretKey"] 
    ?? throw new InvalidOperationException("JWT SecretKey no configurada");

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
    ?? builder.Configuration["JwtSettings:Issuer"] 
    ?? "ChairaAPI";

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
    ?? builder.Configuration["JwtSettings:Audience"] 
    ?? "ChairaFrontend";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"âŒ JWT Auth Failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"âœ… JWT Token validated for: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// CORS
var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            // Agregar x-tagGroups para Redoc
            swagger.Extensions["x-tagGroups"] = new Microsoft.OpenApi.Any.OpenApiArray
            {
                new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["name"] = new Microsoft.OpenApi.Any.OpenApiString("ðŸ”§ Sistema"),
                    ["tags"] = new Microsoft.OpenApi.Any.OpenApiArray
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("Sistema")
                    }
                },
                new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["name"] = new Microsoft.OpenApi.Any.OpenApiString("ðŸ‘¤ Acceso"),
                    ["tags"] = new Microsoft.OpenApi.Any.OpenApiArray
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("Rol"),
                        new Microsoft.OpenApi.Any.OpenApiString("Usuario")
                    }
                },
                new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["name"] = new Microsoft.OpenApi.Any.OpenApiString("ðŸ“‹ CatÃ¡logo"),
                    ["tags"] = new Microsoft.OpenApi.Any.OpenApiArray
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("AreaTecnica"),
                        new Microsoft.OpenApi.Any.OpenApiString("CanalIngreso"),
                        new Microsoft.OpenApi.Any.OpenApiString("CategoriaActivo"),
                        new Microsoft.OpenApi.Any.OpenApiString("EstadoGeneral"),
                        new Microsoft.OpenApi.Any.OpenApiString("EstadoCaso"),
                        new Microsoft.OpenApi.Any.OpenApiString("EstadoActivo"),
                        new Microsoft.OpenApi.Any.OpenApiString("EstadoConsumible"),
                        new Microsoft.OpenApi.Any.OpenApiString("EstadoIntervencion"),
                        new Microsoft.OpenApi.Any.OpenApiString("Prioridad"),
                        new Microsoft.OpenApi.Any.OpenApiString("Sede"),
                        new Microsoft.OpenApi.Any.OpenApiString("TipoCaso"),
                        new Microsoft.OpenApi.Any.OpenApiString("TipoConsumible"),
                        new Microsoft.OpenApi.Any.OpenApiString("TipoTrabajo")
                    }
                },
                new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["name"] = new Microsoft.OpenApi.Any.OpenApiString("ðŸ“¦ Inventario"),
                    ["tags"] = new Microsoft.OpenApi.Any.OpenApiArray
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("Ubicacion"),
                        new Microsoft.OpenApi.Any.OpenApiString("Inventario"),
                        new Microsoft.OpenApi.Any.OpenApiString("Componente"),
                        new Microsoft.OpenApi.Any.OpenApiString("Consumible"),
                        new Microsoft.OpenApi.Any.OpenApiString("Activo"),
                        new Microsoft.OpenApi.Any.OpenApiString("HojaDeVidaActivo")
                    }
                },
                new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["name"] = new Microsoft.OpenApi.Any.OpenApiString("ðŸŽ« Soporte"),
                    ["tags"] = new Microsoft.OpenApi.Any.OpenApiArray
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("Caso"),
                        new Microsoft.OpenApi.Any.OpenApiString("Evidencia"),
                        new Microsoft.OpenApi.Any.OpenApiString("TrazabilidadCaso"),
                        new Microsoft.OpenApi.Any.OpenApiString("IntervencionTecnica"),
                        new Microsoft.OpenApi.Any.OpenApiString("DetalleCambioComponentes"),
                        new Microsoft.OpenApi.Any.OpenApiString("DetalleConsumible"),
                        new Microsoft.OpenApi.Any.OpenApiString("RevisionAdmi"),
                        new Microsoft.OpenApi.Any.OpenApiString("EncuestaCalidad"),
                        new Microsoft.OpenApi.Any.OpenApiString("DetalleEncuesta")
                    }
                }
            };
        });
    });
    
    // Swagger UI en la raÃ­z
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chaira API v1");
        c.RoutePrefix = string.Empty;
    });
    
    // Redoc en /docs (con grupos jerÃ¡rquicos)
    app.UseReDoc(c =>
    {
        c.SpecUrl = "/swagger/v1/swagger.json";
        c.RoutePrefix = "docs";
        c.DocumentTitle = "Chaira API - DocumentaciÃ³n";
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowFrontend");

// Middleware de autenticaciÃ³n y autorizaciÃ³n (orden importante)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health Check
app.MapGet("/api/health", async (ApplicationDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { status = "healthy", database = canConnect ? "connected" : "disconnected", timestamp = DateTime.UtcNow });
}).WithTags("Sistema").WithOpenApi();

app.Run();

