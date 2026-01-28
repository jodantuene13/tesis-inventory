# Registro de Cambios - Épica: Base de Datos

| Fecha | Autor | HU / Épica | Descripción | Impacto |
|-------|-------|------------|-------------|---------|
| 2026-01-20 | Antigravity AI | Inicialización | Creación inicial del esquema de base de datos (DDL). | Alto (Estructura Base) |
| 2026-01-20 | Antigravity AI | HU-DB-01 a 04 | Refactorización de tablas a Naming Singular (Usuarios -> Usuario, etc.) para cumplir convención. | Medio (Cambio en DDL) |
| 2026-01-21 | Antigravity AI | HU-AUTH-01 | Modificación `Usuario`: `googleId` y password nullable para SSO. | Bajo (DDL Alter) |
