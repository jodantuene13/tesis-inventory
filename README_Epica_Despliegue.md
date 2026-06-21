# Épica: Despliegue y Arquitectura (Railway)

## Objetivo
Configurar el proyecto para poder compilarse y desplegarse en contenedores Docker mediante Railway (Single Container Full-Stack).

## Alcance
- Compilación de Angular en stage de Node.
- Compilación de ASP.NET 9 en stage del SDK.
- Fusión de los binarios: .NET sirve los estáticos de Angular mediante el pipeline HTTP.

## Decisiones de Implementación y Ajustes

### [2026-05-19] Dockerización Full-Stack Unificada
- **Descripción del cambio:** Se creó el `Dockerfile` y `.dockerignore` en la raíz del repositorio. Se editó el archivo `Program.cs` del backend para agregar `app.UseDefaultFiles()`, `app.UseStaticFiles()` y `app.MapFallbackToFile("index.html")`.
- **Motivo técnico:** Optimizar costos en Railway ejecutando un único servicio (Single Container Deploy). El Kestrel de ASP.NET Core asume la responsabilidad de servir la Single Page Application compilada de Angular, redirigiendo todas las rutas no coincidentes al `index.html`.
- **Impacto funcional:** Listo para despliegue automatizado. Las variables de entorno necesarias para la BD (`ConnectionStrings__DefaultConnection`) deberán configurarse en Railway.
