# Registro de Cambios - Épica: Base de Datos

| Fecha | Autor | HU / Épica | Descripción | Impacto |
|-------|-------|------------|-------------|---------|
| 2026-01-20 | Antigravity AI | Inicialización | Creación inicial del esquema de base de datos (DDL). | Alto (Estructura Base) |
| 2026-01-20 | Antigravity AI | HU-DB-01 a 04 | Refactorización de tablas a Naming Singular (Usuarios -> Usuario, etc.) para cumplir convención. | Medio (Cambio en DDL) |
| 2026-01-21 | Antigravity AI | HU-AUTH-01 | Modificación `Usuario`: `googleId` y password nullable para SSO. | Bajo (DDL Alter) |
| 2026-01-30 | Antigravity AI | HU-ROLES-01/02/03 | Implementación de ABM de Roles con validación de integridad referencial. Backend y Frontend. | Medio (Nueva Funcionalidad) |
| 2026-01-30 | Antigravity AI | HU-AUTH-04 | Habilitación de edición de Estado (Activo/Inactivo) de usuarios desde lista y formulario. Update Backend DTO. | Bajo (Mejora UX) |
| 2026-01-30 | Antigravity AI | HU-AUDIT-01 | Implementación de Historial de Modificaciones (Audit Log) para Usuarios. Backend (Entity, Service, Controller) y Frontend (Page, Service). | Medio (Nueva Funcionalidad) |
| 2026-03-06 | Antigravity AI | HU-SEDES-01/02/03/04 | Implementación del ABM de Sedes (Módulo Configuración) incorporando validación anti-borrado sobre usuarios asignados según DIC y añadiendo atributo Direccion. | Medio (Nueva Funcionalidad) |
| 2026-03-07 | Antigravity AI | Módulo Inventario | Adición de DbSets faltantes en `InventoryDbContext`, configuración de FKs mediante FluentAPI y ejecución de migraciones EF Core (`InventorySchemaInitialization`). | Alto (Esquema BD) |
| 2026-03-07 | Antigravity AI | Módulo Inventario | Implementación fullstack ABM de Rubros, Familias, Atributos y Productos, incluyendo autogeneración de SKU y borrado lógico con validación estricta de nombre en UI. | Alto (Nueva Funcionalidad) |
| 2026-03-08 | Antigravity AI | Módulo Inventario (Stock) | Implementación fullstack del Submódulo Stock (Entidades de Movimiento y Transferencia, StockService, StockController y Componente Angular completo con Modales, Filtros y Paginación). Aplicado a la base de datos vía AddStockModule. | Alto (Nueva Funcionalidad) |
