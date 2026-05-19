# README - Épica: Fundamentos de Base de Datos

## Objetivo
Establecer la estructura física de almacenamiento de datos del sistema "Tesis Inventory", implementando el esquema relacional en MySQL basado en el modelo de clases definido.

## Historias de Usuario (HU)
- **HU-DB-01**: Crear tablas maestras (Roles, Sedes, Categorías).
- **HU-DB-02**: Crear tablas principales (Usuarios, Productos).
- **HU-DB-03**: Crear tablas transaccionales (Movimientos, Transferencias, Solicitudes, Ajustes).
- **HU-DB-04**: Crear tablas de auditoría (Historiales).

## Alcance Funcional
- Script DDL para la creación de la base de datos completa.
- Definición de claves primarias, foráneas e índices.

## Capas Involucradas
- **Infrastructure**: Persistencia de datos (Base de Datos).

## Decisiones de Implementación y Ajustes (Logico vs Físico)
Se realizaron las siguientes adaptaciones al modelo de clases original para su implementación efectiva en MySQL:


200126 - cambios a impactar en TP

### 1. Tipos de Datos y Restricciones
- **Autenticación Híbrida**: Se modificó la tabla `Usuario` para permitir `contraseña` NULA y se agregó `googleId` para soportar inicio de sesión único (SSO).
- **Strings Genéricos**: Se definieron longitudes específicas (`VARCHAR(50)`, `VARCHAR(100)`, `VARCHAR(255)`) en lugar de `string` genérico para optimizar almacenamiento.
- **Tipos ENUM**: Se reemplazaron los campos `string` de estado/tipo por `ENUM` para garantizar integridad de datos:
  - `Movimientos.tipoMovimiento`: 'ENTRADA', 'SALIDA', 'TRANSFERENCIA', 'CONSUMO'.
  - `Transferencias.estado`: 'PENDIENTE', 'ACEPTADA', 'EN_TRANSITO', 'RECIBIDA', 'RECHAZADA'.
  - `SolicitudesCompra.prioridad` y `estado`.
  - `Informes.tipoInforme`.
- **Fechas**: Se utilizó `DATETIME DEFAULT CURRENT_TIMESTAMP` para automatizar el registro de fechas (`fechaRegistro`, `fecha`).
- **Booleans**: Se implementaron como `TINYINT(1)` (estándar MySQL para `BOOLEAN`).

### 2. Normalización y Nombres
- **Eliminación de Acentos en Identificadores**: Se renombró la columna `código` a `codigo` en la tabla `Productos` para evitar conflictos de codificación en consultas SQL.
- **Manejo de Caracteres Especiales**: Se aplicó quoting (backticks) al campo `contraseña` en la tabla `Usuarios` para permitir el uso de la letra 'ñ' en el nombre de la columna sin errores de sintaxis DDL.
- **Campos Calculados/Automáticos**: Se agregó `ON UPDATE CURRENT_TIMESTAMP` en `Stocks.fechaActualizacion` para reflejar cambios automáticamente.

### 3. Campos Adicionales
- **Informes**: Se agregó una columna `contenido JSON` para permitir el almacenamiento estructurado de los datos del reporte o los filtros aplicados.

### 4. Convención de Nombres (Refactor)
- **Tablas en Singular**: Se adoptó la convención de nombres en singular para todas las tablas (`Usuario`, `Rol`, `Sede`, `Movimiento`, etc.) para adherirse a las mejores prácticas de diseño y consistencia con los nombres de las clases del dominio.

### [2026-05-09]
- **Descripción del cambio:** Recreación de Base de Datos, Semilla Inicial y Seed Completo. Se aplicaron todas las migraciones de EF Core, se recreó el esquema y se inyectaron datos base (Roles, Sede, Usuario). Posteriormente, se creó y ejecutó un archivo `seed_full_entities.sql` con datos de prueba (mock) para TODAS las entidades del sistema (Rubros, Familias, Atributos, Productos, Stock, Transferencias, Solicitudes de Compra, etc.) respetando la integridad referencial.
- **Motivo técnico:** Reconstrucción del entorno local debido a la pérdida de la base de datos y necesidad de contar con datos de prueba realistas para agilizar el desarrollo y testing del frontend.
- **Impacto funcional:** Se restableció el esquema de base de datos, el acceso al sistema, y se dispone de un entorno de inventario funcionalmente completo (con stock y operaciones).

### [2026-05-19]
- **Descripción del cambio:** Se deshabilitó temporalmente el seeder (`DbInitializer.Initialize(context)`) en `Program.cs` y se recreó la base de datos aplicando las migraciones.
- **Motivo técnico:** Solicitud del usuario debido a rotura de base de datos. El seeder se modificará antes de ejecutarlo.
- **Impacto funcional:** Ninguno sobre la aplicación final. La base de datos queda vacía y estructurada con las tablas, lista para el nuevo seeder.

### [2026-05-19] (Continuación)
- **Descripción del cambio:** Se expandió `DbInitializer.cs` para incluir 15 registros por cada entidad principal (Roles, Sedes, Usuarios, Rubros, Familias, Productos, Stock). Se descomentó la ejecución del seeder en `Program.cs`.
- **Motivo técnico:** Solicitud del usuario para poder autocompletar la base de datos con suficientes datos de ejemplo cada vez que se inicie o recree en entornos de desarrollo.
- **Impacto funcional:** Al ejecutar el backend, el sistema generará automáticamente un catálogo de 15 productos con su respectivo stock, familias, rubros y usuarios en caso de estar la base vacía.
