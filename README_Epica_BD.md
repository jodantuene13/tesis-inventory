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
