# PeloterosMcpServer ⚽🤖

**PeloterosMcpServer** es un servidor basado en el estándar **MCP (Model Context Protocol)** desarrollado en .NET. Su objetivo es actuar como un puente de comunicación entre asistentes de Inteligencia Artificial (LLMs) y el sistema de gestión de campeonatos de fútbol [**Peloteros.**](https://peloteros.runasp.net/)

A través de esta integración, los asistentes de IA van a poder consultar información de los torneos, jugadores, equipos, etc. estructurada en tiempo real. En versiones posteriores, tambien ejecutar operaciones sobre los torneos.

---

## 🎯 Características Principales

* **Integración con IA:** Expone contexto y datos de torneos de fútbol mediante el protocolo MCP.
* **Consulta de Datos:** Permite a la IA acceder a información sobre equipos, jugadores, partidos, tablas de posiciones y estadísticas de campeonatos.
* **Acciones Futuras:** Capacidad de ejecutar operaciones sobre el sistema Peloteros de forma guiada por lenguaje natural.
* **Arquitectura Moderna:** Desarrollado sobre la plataforma .NET para garantizar rendimiento y mantenibilidad.

---

## 🏗️ Arquitectura de la Solución

El proyecto está compuesto por los siguientes componentes clave:

1. **PeloterosMcpServer:** Proyecto principal que implementa el protocolo MCP y maneja las solicitudes provenientes del cliente de IA.
2. **PeloterosMcpServer.Data:** Capa de acceso a datos.

---

## 🚀 Requisitos e Instalación

### Prerrequisitos
* [.NET SDK](https://dotnet.microsoft.com/) (versión 10.0.400)
* Un cliente compatible con MCP (por ejemplo, Claude Desktop)


### 🔐 Cadena de Conexión (User Secrets)
Para configurar la cadena de conexión en entorno de desarrollo local, usa la herramienta **User Secrets** de .NET:
```JSON
{
  "ConnectionStrings": {
    "Peloteros": "Server=[NombreServidor];Database=[NombreBaseDatos];Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```