# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

An ASP.NET Core (.NET 10) **MCP server** (`ModelContextProtocol.AspNetCore`) that exposes the data of **Peloteros** — a football championship management system (https://peloteros.runasp.net/) — as MCP tools for LLM clients. It reads and (increasingly) writes directly to the Peloteros SQL Server database via EF Core.

The codebase is written in Spanish: identifiers, tool names, `[Description]` texts, user-facing messages, comments and commit messages. Keep new code consistent with that.

## Commands

```bash
dotnet build PeloterosMcpServer.slnx
dotnet run --project PeloterosMcpServer --launch-profile http    # http://localhost:6179 (what .mcp.json points to)
dotnet run --project PeloterosMcpServer --launch-profile https   # https://localhost:5239 + http://localhost:6179
```

There is no test project. `GET /api/test` (`Controllers/TestController.cs`) is a DB connectivity health check; `PeloterosMcpServer.http` holds ad-hoc requests.

Use the `http` endpoint for MCP clients — VS Code fails against the self-signed dev certificate on https.

### Local configuration (User Secrets)

`appsettings.json` only holds `Jwt:Issuer` (Peloteros.Web, `http://localhost:5012` in dev) and `Jwt:Audience` (this server, `http://localhost:6179`). The rest must come from user secrets on the `PeloterosMcpServer` project:

```bash
dotnet user-secrets set "ConnectionStrings:Peloteros" "Server=...;Database=...;Trusted_Connection=True;TrustServerCertificate=True;" --project PeloterosMcpServer
dotnet user-secrets set "Jwt:Key" "<same symmetric key Peloteros.Web signs with>" --project PeloterosMcpServer
```

## Architecture

Two projects in `PeloterosMcpServer.slnx`:

- **PeloterosMcpServer.Data** — EF Core model **scaffolded database-first** from the existing Peloteros DB (`Context/PeloterosDbContext.cs`, `Entities/`). That explains the odd pluralization (`Jugadors`, `Transferencium`, `ReunionAsistencium`). There are no migrations; the schema is owned by the Peloteros app, so don't add migrations here — re-scaffold if the schema changes.
- **PeloterosMcpServer** — the web host:
  - `Program.cs` registers each tool class explicitly with `.WithTools<T>()` (stateless HTTP transport). **A new tool class must be added there**, or it won't be exposed.
  - `Tools/*Tools.cs` — `[McpServerToolType]` classes with `static` `[McpServerTool]` methods. Dependencies (`PeloterosDbContext`, `IHttpContextAccessor`) are injected as method parameters; the remaining parameters become the tool's input schema, described via `[Description]`. Tools return DTOs from `DTOs/` (projected with `.Select`, read queries use `AsNoTracking()`), never entities.
  - `[Description]` on tools is prompt text for the LLM: it says when to use the tool, which other tool to use instead, and prerequisites (e.g. "obtain the CampeonatoId first with listar_campeonatos"). Maintain these cross-references when adding/renaming tools.

### Authentication / authorization

- The whole MCP endpoint (`app.MapMcp().RequireAuthorization()`) requires a JWT Bearer token issued by **Peloteros.Web** (symmetric key `Jwt:Key`, validated issuer/audience/lifetime).
- Unauthenticated requests get `401` with `WWW-Authenticate: Bearer resource_metadata=".../.well-known/oauth-protected-resource"`; that endpoint advertises Peloteros.Web as the authorization server (MCP OAuth discovery). Token rejections are logged with the reason.
- Per-tool role checks are done inside the tool with `httpContextAccessor.RequireRole("Administrator", ...)` (`Security/ToolAuthorizationExtensions.cs`), which throws `McpException` — the MCP-visible way to report errors to the client. Write operations require `Administrator` (some also `Manager`).

### Write-tool pattern (confirmation)

Mutating tools take a `bool confirmar = false` parameter. With `false` they return a preview result (`RequiereConfirmacion = true`, `Mensaje` explaining the consequences) and change nothing; only a second call with `confirmar = true` persists. Result types live in `DTOs/CampeonatoWorkflowResultados.cs`. Follow this pattern for any new write tool.

### Domain notes

- Status columns are single-char codes, e.g. `CampeonatoEstadoId` `"A"` active / `"I"` inactive; `PartidoEstadoId` `"D"` = played; `JugadorEstadoId` `"V"` verified.
- At most one championship may be active at a time — activating/creating one deactivates the current one.
- A championship "has a winner" when its `Fase.Nombre == "Final"` match is played (`"D"`) and has `EquipoIdGanador`.
