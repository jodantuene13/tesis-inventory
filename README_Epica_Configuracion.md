# README - Épica: Módulo de Configuración

## Objetivo
Implementar la gestión integral de usuarios y roles del sistema, permitiendo al administrador registrar, modificar, dar de baja y asignar permisos a los operadores del sistema.

## Historias de Usuario (HU) / Casos de Uso (CF)
- **CF01**: Registrar nuevo usuario (Manual, correo institucional).
- **CF02**: Asignar roles y permisos.
- **CF03**: Modificar datos de usuario (Nombre, Rol, Sede, Contraseña).
- **CF04**: Cambiar estado de usuario (Activo/Inactivo).
- **CF05**: Registrar historial de modificaciones (Auditoría automática).
- **CF06**: Consultar lista de usuarios.
- **CF07**: Filtrar usuarios (Por Rol o Sede).

## Alcance Funcional
- ABM (Alta, Baja, Modificación) de Usuarios.
- Asignación de Roles predefinidos.
- Visualización de listados con filtros.
- Registro de auditoría.

## Capas Involucradas (Onion Architecture)
- **Presentation**: `UsersController`, Componentes Angular (`users-list`, `user-form`).
- **Application**: `UsersService`, DTOs (`CreateUserDto`, `UpdateUserDto`), Validaciones.
- **Domain**: Entidades `Usuario`, `Rol`. Reglas de unicidad de email.
- **Infrastructure**: `UserRepository`, `InventoryDbContext`.

## Flujo General
1. Administrador accede al Módulo de Configuración.
2. Visualiza listado de usuarios.
3. Puede Crear Nuevo, Editar existente o Cambiar Estado.
4. Toda modificación genera un registro en `HistorialModificacion`.

---

# Arquitectura de la Épica

## Componentes
- **Backend API**: Endpoints RESTful para gestión de recursos.
- **Frontend SPA**: Interfaces reactivas en Angular.

## Dependencias
- Depende de `AuthService` para la protección de rutas (solo Admin).
- Relación con tablas `Sede` y `Rol` para integridad referencial.

---

# Registro de Cambios (Cambios.md)

| Fecha | Autor | HU/Caso | Descripción | Impacto |
|-------|-------|---------|-------------|---------|
| 2026-01-26 | AI Agent | CF01-CF07 | Creación inicial de documentación y plan | N/A |
