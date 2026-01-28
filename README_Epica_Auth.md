# README - Épica: Inicio de Sesión / Autenticación

## Objetivo
Desarrollar la funcionalidad de autenticación segura del sistema, permitiendo el inicio de sesión único (SSO) mediante Google para usuarios institucionales (@ucc.edu.ar) y gestionando el control de sesiones y restricciones de acceso.

## Historias de Usuario (HU)
- **HU-AUTH-01**: Iniciar sesión con Google (SSO) [IS01, IS02].
- **HU-AUTH-02**: Restringir acceso a usuarios activos y dominios válidos [IS04].
- **HU-AUTH-03**: Cerrar sesión [IS03].

## Casos de Uso Detallados

### IS01: Iniciar Sesión (Trazo Grueso/Fino)
- **Actor**: Usuario del sistema.
- **Objetivo**: Autenticarse para acceder al sistema.
- **Flujo Principal**:
    1. Usuario accede a pantalla de login.
    2. Selecciona "Iniciar sesión con Google".
    3. Sistema redirige a proveedor de identidad (Google).
    4. Usuario ingresa credenciales de Google.
    5. Sistema recibe token, valida dominio `@ucc.edu.ar` y existencia/estado del usuario en BD local.
    6. Si es válido y activo, redirige al menú principal.
- **Excepciones**: Dominio inválido o usuario inactivo/no registrado.

### IS02: Validar Credenciales
- **Actor**: Sistema.
- **Objetivo**: Verificar validez de la cuenta y permisos.
- **Lógica**: Consultar API de Google -> Verificar Email -> Verificar activo en BD.

### IS03: Cerrar Sesión
- **Actor**: Usuario.
- **Objetivo**: Finalizar sesión activa.
- **Flujo**: Click en "Cerrar sesión" -> Invalidar token/sesión local -> Redirigir a Login.

### IS04: Restringir acceso a usuarios activos
- **Actor**: Sistema.
- **Regla**: Solo usuarios con `estado = TRUE` y email `@ucc.edu.ar` pueden ingresar.

## Capas Involucradas
- **Presentation (Angular)**: Botón de Google Sign-In, Rutas protegidas (Guards), Manejo de Token.
- **Application (Backend)**: Servicio de Autenticación (`AuthService`), validación de dominio, generación de JWT interno.
- **Domain**: Entidad `Usuario` (reglas de negocio).
- **Infrastructure**: Implementación de Cliente Google Auth, Repositorio de Usuarios.

## Cambios en Base de Datos
- Modificación de tabla `Usuario` para soportar autenticación externa (ver `README_Epica_BD.md` para detalles técnicos).
