# Documentación Funcional del Proyecto TesisInventory (Estado Actual)

## 1. Resumen Ejecutivo
El proyecto se encuentra en una fase inicial de **Cimientos Sólidos**. Se ha completado el diseño total de la base de datos relacional para el sistema de inventario y se ha implementado un sistema de autenticación robusto basado en **Google Login** y **JWT** (JSON Web Tokens), con una arquitectura de backend limpia (Onion Architecture) lista para escalar.

Actualmente, el sistema permite:
1. Crear la estructura completa de base de datos.
2. Iniciar sesión mediante Google (restringido al dominio `@ucc.edu.ar`).
3. Verificar existencia y estado del usuario en base de datos.
4. Acceder al Frontend (Angular) y navegar entre Login y Home (protegido).

---

## 2. Funcionalidades Implementadas

### A. Autenticación y Seguridad (Backend & Frontend)
El módulo de autenticación está **completamente funcional** y conecta Frontend, Backend y Google.

*   **Google Sign-In**: Integración en el Frontend para obtener token de identidad.
*   **Validación de Dominio**: El backend rechaza cualquier correo que no pertenezca a `@ucc.edu.ar`.
*   **Verificación de Usuario**:
    *   El sistema verifica si el email existe en la tabla `Usuario`.
    *   Verifica si el campo `Estado` es activo (`true`).
    *   **Auto-Link**: Si el usuario existe por carga manual (email/rol) pero es su primer login con Google, el sistema vincula automáticamente su `GoogleId`.
*   **Generación de JWT**: Se emite un token firmado que contiene `NameId` (ID usuario), `Email` y `Role` (Rol del usuario).
*   **Protección de Rutas (Guards)**: El Frontend impide el acceso a `/home` si no hay un token válido almacenado.

### B. Base de Datos (Infraestructura)
El esquema de base de datos MySQL está **diseñado y desplegado al 100%** de la especificación inicial, preparado para soportar todas las funcionalidades futuras.

**Tablas Maestras:**
*   `Rol`: Roles de usuario (Admin, Depósito, etc.).
*   `Sede`: Ubicaciones físicas.
*   `Categoria`: Categorización de productos.

**Usuarios:**
*   `Usuario`: Manejo de credenciales, estado y relaciones con Sede/Rol. Soporta campos opcionales (`Password`/`GoogleId`) para flexibilidad en el login.

**Inventario (Esqueleto listo):**
*   `Producto`: Catálogo maestro.
*   `Stock`: Cantidad actual por Producto/Sede.
*   `Movimiento`: Registro histórico de entradas/salidas.
*   `Transferencia`: Lógica de envíos entre sedes.
*   `SolicitudCompra`: Pedidos de reposición.
*   `AjusteStock`: Correcciones manuales auditadas.
*   `Informe`: Almacenamiento de reportes generados.

**Auditoría:**
*   Tablas espejo para registrar cambios en `Usuario`, `Transferencia` y `Solicitud`.

### C. Arquitectura Técnica
*   **Backend**: .NET 9.0 con Arquitectura Onion (Capas: Domain, Application, Infrastructure, API).
*   **ORM**: Entity Framework Core con `Pomelo.EntityFrameworkCore.MySql`.
*   **Frontend**: Angular con Standalone Components.
*   **Base de Datos**: MySQL / MariaDB.

---

## 3. Funcionalidades Pendientes (To-Do List)
Aunque la base de datos soporta estas funciones, **aún no existen los Endpoints en el Backend ni las Pantallas en el Frontend** para ellas.

#### Gestión de Inventario
- [ ] **CRUD Productos**: Alta, baja y modificación de productos.
- [ ] **Visualización de Stock**: Pantalla para ver cantidades por sede.

#### Operaciones
- [ ] **Registrar Movimiento**: API y UI para ingresar/egresar mercadería.
- [ ] **Transferencias**: Flujo de solicitud -> aprobación -> recepción entre sedes.
- [ ] **Solicitudes de Compra**: Flujo de pedido y autorización.

#### Reportes
- [ ] **Generación de Informes**: El backend debe procesar los datos y guardarlos en la tabla `Informe`.

---

## 4. Estructura de Archivos Clave
*   `backend/TesisInventory.API/Controllers/AuthController.cs`: Controlador principal de autenticación.
*   `backend/TesisInventory.Domain/Entities/*.cs`: Definición de todas las entidades del negocio.
*   `frontend/src/app/pages/login`: Lógica de UI para login.
*   `database.sql`: Script fuente de la base de datos.
