using Microsoft.EntityFrameworkCore;
using PeloterosMcpServer.Data.Context;
using PeloterosMcpServer.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add the MCP services: the transport to use (http) and the tools to register.
builder.Services
    .AddMcpServer()
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
    .WithTools<EquipoTools>();

// Configurar la cadena de conexión y EF Core
builder.Services.AddDbContext<PeloterosDbContext>(opt =>
                                opt.UseSqlServer(builder.Configuration.GetConnectionString("Peloteros"))
                            );

var app = builder.Build();
app.MapMcp();
//app.UseHttpsRedirection();

app.Run();
