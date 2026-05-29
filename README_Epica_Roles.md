# Épica: Roles y Permisos (Configuración Atómica)

## Objetivo
Implementar la gestión de roles dentro del sistema permitiendo una configuración atómica de permisos y de alcance de sedes, además de habilitar a los usuarios para cambiar el contexto de operación entre sedes habilitadas (con permisos condicionados a si operan en su sede primaria o en una secundaria).

## Historias de Usuario
- Como Administrador, quiero poder crear roles y asignarles permisos específicos desde una lista en forma de árbol.
- Como Administrador, quiero definir si un rol tiene acceso a todas las sedes o a un conjunto limitado de ellas.
- Como Administrador, quiero configurar si las operaciones de un usuario (crear/modificar) deben limitarse sólo a su sede primaria.

## Alcance Funcional
- ABM de Roles (Nombre, Descripción, Flags: `TodasLasSedes`, `LimitarOperacionSedePrimaria`).
- Selección de permisos mediante checkboxes agrupados por módulo.
- Selección de sedes en formato multiselect en caso de no tener el flag `TodasLasSedes`.
- Ocultamiento de opciones en el menú lateral (`AdminLayoutComponent`) basado en los permisos de visualización (`*_Ver`).
- Envío de configuración de sedes y permisos al frontend durante el Login.

## Capas Involucradas
- **Domain**: `Rol`, `Permiso`, `RolPermiso`, `RolSede`.
- **Infrastructure**: `InventoryDbContext`, configuraciones de Fluent API, Repositorios (`RoleRepository`, `DbInitializer`).
- **Application**: `RolDto`, `RolCreateDto`, `RolUpdateDto`, `PermisoDto`, `IRolesService`, `RolesService`.
- **API**: `RolesController`, `AuthController` (actualización de respuesta de login).
- **Frontend**: `RoleFormComponent`, `AdminLayoutComponent`, `auth.service.ts`, `role.service.ts`, modelo `user`.

## Decisiones de Implementación y Ajustes
- **[2026-05-29]**:
  - **Descripción**: Implementación de las entidades relacionales `RolPermiso` y `RolSede`, creación de 20 permisos semilla en `DbInitializer.cs`, adición de campos configurables (`TodasLasSedes`, `LimitarOperacionSedePrimaria`) al `Rol`.
  - **Motivo técnico**: Proveer una arquitectura escalable de control de accesos usando un modelo relacional en vez de JSON o flags binarios, alineado con las buenas prácticas de EF Core.
  - **Impacto funcional**: Ninguno sobre las vistas previas, pero dota de la estructura subyacente para los árboles de selección en el form de rol y la seguridad real en operaciones de API.

  - **Descripción**: Refactorización de la respuesta de `GoogleLogin` para incrustar `permisos` (lista de nombres) y `sedesPermitidas` (lista de IDs) directamente en el token/cookie (vía localStorage user).
  - **Motivo técnico**: Evita tener que hacer request adicional de los permisos al cargar el `AdminLayoutComponent`. Se priorizó un payload JSON más denso en login por performance de UI.

  - **Descripción**: Implementación de la vista `RoleFormComponent` usando un grid dual para Sedes y Permisos.
  - **Motivo técnico**: El usuario solicitó un diagrama visual organizado por módulos, así que se agruparon dinámicamente los permisos por la propiedad `Modulo`.
