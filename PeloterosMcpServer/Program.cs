using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.Tools;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();  // OHD: Registra los controladores en la inyección de dependencias

// Add the MCP services: the transport to use (http) and the tools to register.
builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        // Stateless mode is recommended for servers that don't need
        // server-to-client requests like sampling or elicitation.
        // See https://csharp.sdk.modelcontextprotocol.io/concepts/transports/transports.html for details.
        options.Stateless = true;
    })
    .WithTools<RandomNumberTools>()
    .WithTools<JugadorTools>()
    .WithTools<CampeonatoTools>()
    .WithTools<EquipoTools>()
    .WithTools<PartidoTools>()
    .WithTools<TransferenciaTools>()
    .WithTools<ReunionTools>();

// Servicio acceder al request HTTP
builder.Services.AddHttpContextAccessor();

// Configurar la cadena de conexión y EF Core
builder.Services.AddDbContext<PeloterosDbContext>(opt =>
                                opt.UseSqlServer(builder.Configuration.GetConnectionString("Peloteros"))
                            );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = "PeloterosMcp",
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapMcp()
    .RequireAuthorization(); ;
//app.UseHttpsRedirection();
app.MapControllers();   // OHD: Habilita el ruteo hacia Controllers como /api/test

app.Run();
