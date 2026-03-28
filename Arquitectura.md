# Arquitectura de Datos

## Motor de Base de Datos
- **MySQL / MariaDB** (Compatible con entorno XAMPP).

## Diseño del Esquema
El esquema sigue una arquitectura relacional normalizada (3NF) para asegurar la integridad de los datos.

### Componentes por Capa (Infrastructure - DB)

#### Tablas Maestras
- `Roles`: Definición de perfiles de acceso.
- `Sedes`: Ubicaciones físicas (incluye `Direccion`).
- `Rubros` y `Familias`: Estructura jerárquica de agrupación de productos.
- `Atributos`: Diccionario de características dinámicas para productos.

#### Tablas Principales
- `Usuarios`: Gestión de acceso y perfiles.
- `Productos`: Catálogo de ítems.
- `Stock`: Relación N:M entre Productos y Sedes (cantidad actual).

#### Tablas Transaccionales
- `Movimientos`: Registro de entradas/salidas (consumos, ajustes preventivos) manteniendo el saldo de cantidad restante histórico.
- `Transferencias`: Logística e intercambio de stock entre sedes (con estado de aprobación).
- `SolicitudesCompra`: Flujo de aprobaciones.
- `AjustesStock`: Correcciones manuales.
- `Informes`: Registro de reportes generados.

#### Tablas de Auditoría
- `HistorialModificacionesUsuarios`: Cambios en perfil de usuario.
- `HistorialTransferencias`: Cambios de estado en transferencias.
- `HistorialSolicitudes`: Cambios de estado en solicitudes.

## Relaciones Clave
- **Producto - Stock - Sede**: Control de inventario distribuido.
- **Usuario - Rol - Sede**: Control de acceso y contexto operativo.
- **Auditoría**: Trazabilidad completa por ID de usuario responsable.
