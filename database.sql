-- Script de Creación de Base de Datos - Tesis Inventory
-- Generado por Antigravity AI
-- Fecha: 2026-01-20
-- Actualización: 2026-01-20 (Refactorización a Singular)

DROP DATABASE IF EXISTS tesis_inventory;
CREATE DATABASE IF NOT EXISTS tesis_inventory CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE tesis_inventory;

-- =============================================
-- 1. Tablas Maestras
-- =============================================

CREATE TABLE IF NOT EXISTS Rol (
    idRol INT AUTO_INCREMENT PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL UNIQUE,
    descripcion TEXT
);

CREATE TABLE IF NOT EXISTS Sede (
    idSede INT AUTO_INCREMENT PRIMARY KEY,
    nombreSede VARCHAR(100) NOT NULL,
    direccion VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS Categoria (
    idCategoria INT AUTO_INCREMENT PRIMARY KEY,
    nombreCategoria VARCHAR(100) NOT NULL,
    descripcion TEXT
);

-- =============================================
-- 2. Usuarios y Productos
-- =============================================

CREATE TABLE IF NOT EXISTS Usuario (
    idUsuario INT AUTO_INCREMENT PRIMARY KEY,
    nombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    googleId VARCHAR(100) UNIQUE NULL,
    password VARCHAR(255) NULL,
    estado BOOLEAN DEFAULT TRUE,
    fechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP,
    idRol INT NOT NULL,
    idSede INT NOT NULL,
    FOREIGN KEY (idRol) REFERENCES Rol(idRol),
    FOREIGN KEY (idSede) REFERENCES Sede(idSede)
);

CREATE TABLE IF NOT EXISTS Producto (
    idProducto INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    unidadMedida VARCHAR(20),
    idCategoria INT NOT NULL,
    stockMinimo INT DEFAULT 0,
    fechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (idCategoria) REFERENCES Categoria(idCategoria)
);

-- =============================================
-- 3. Stock e Inventario
-- =============================================

CREATE TABLE IF NOT EXISTS Stock (
    idStock INT AUTO_INCREMENT PRIMARY KEY,
    idProducto INT NOT NULL,
    idSede INT NOT NULL,
    cantidadActual INT DEFAULT 0,
    fechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
    FOREIGN KEY (idSede) REFERENCES Sede(idSede),
    UNIQUE KEY unique_stock_sede (idProducto, idSede)
);

-- =============================================
-- 4. Transaccionales: Movimientos, Transferencias, Solicitudes
-- =============================================

CREATE TABLE IF NOT EXISTS Movimiento (
    idMovimiento INT AUTO_INCREMENT PRIMARY KEY,
    idProducto INT NOT NULL,
    idSede INT NOT NULL,
    tipoMovimiento ENUM('ENTRADA', 'SALIDA', 'TRANSFERENCIA', 'CONSUMO') NOT NULL,
    cantidad INT NOT NULL,
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    motivo VARCHAR(255),
    idUsuario INT NOT NULL,
    area VARCHAR(100),
    FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
    FOREIGN KEY (idSede) REFERENCES Sede(idSede),
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);

CREATE TABLE IF NOT EXISTS Transferencia (
    idTransferencia INT AUTO_INCREMENT PRIMARY KEY,
    idProducto INT NOT NULL,
    sedeOrigen INT NOT NULL,
    sedeDestino INT NOT NULL,
    cantidad INT NOT NULL,
    fechaSolicitud DATETIME DEFAULT CURRENT_TIMESTAMP,
    fechaAceptacion DATETIME,
    fechaRecepcion DATETIME,
    estado ENUM('PENDIENTE', 'ACEPTADA', 'EN_TRANSITO', 'RECIBIDA', 'RECHAZADA') DEFAULT 'PENDIENTE',
    idUsuarioSolicita INT NOT NULL,
    idUsuarioRecibe INT,
    FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
    FOREIGN KEY (sedeOrigen) REFERENCES Sede(idSede),
    FOREIGN KEY (sedeDestino) REFERENCES Sede(idSede),
    FOREIGN KEY (idUsuarioSolicita) REFERENCES Usuario(idUsuario),
    FOREIGN KEY (idUsuarioRecibe) REFERENCES Usuario(idUsuario)
);

CREATE TABLE IF NOT EXISTS SolicitudCompra (
    idSolicitud INT AUTO_INCREMENT PRIMARY KEY,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    motivo TEXT,
    prioridad ENUM('BAJA', 'MEDIA', 'ALTA', 'URGENTE') DEFAULT 'MEDIA',
    estado ENUM('PENDIENTE', 'APROBADA', 'RECHAZADA', 'COMPLETADA') DEFAULT 'PENDIENTE',
    fechaSolicitud DATETIME DEFAULT CURRENT_TIMESTAMP,
    fechaDecision DATETIME,
    idUsuarioSolicita INT NOT NULL,
    idUsuarioAutoriza INT,
    observaciones TEXT,
    FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
    FOREIGN KEY (idUsuarioSolicita) REFERENCES Usuario(idUsuario),
    FOREIGN KEY (idUsuarioAutoriza) REFERENCES Usuario(idUsuario)
);

CREATE TABLE IF NOT EXISTS AjusteStock (
    idAjuste INT AUTO_INCREMENT PRIMARY KEY,
    idProducto INT NOT NULL,
    idSede INT NOT NULL,
    cantidadAnterior INT NOT NULL,
    cantidadNueva INT NOT NULL,
    justificacion TEXT NOT NULL,
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    idUsuario INT NOT NULL,
    FOREIGN KEY (idProducto) REFERENCES Producto(idProducto),
    FOREIGN KEY (idSede) REFERENCES Sede(idSede),
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);

CREATE TABLE IF NOT EXISTS Informe (
    idInforme INT AUTO_INCREMENT PRIMARY KEY,
    tipoInforme ENUM('STOCK', 'MOVIMIENTOS', 'SALIDAS', 'TRANSFERENCIAS') NOT NULL,
    fechaGeneracion DATETIME DEFAULT CURRENT_TIMESTAMP,
    idUsuario INT NOT NULL,
    contenido JSON NULL COMMENT 'Almacena filtros o resumen del informe generado',
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);

-- =============================================
-- 5. Tablas de Auditoría
-- =============================================

CREATE TABLE IF NOT EXISTS AuditLog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    EntityId INT NOT NULL,
    EntityType VARCHAR(50) DEFAULT 'Usuario',
    Action VARCHAR(50) NOT NULL,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    Details TEXT,
    ExecutorId INT,
    ExecutorName VARCHAR(100),
    ExecutorEmail VARCHAR(100),
    ExecutorRole VARCHAR(50),
    ExecutorSede VARCHAR(50),
    TargetUserSnapshot TEXT
);

-- Deprecated/Unused legacy table (kept for reference if needed, or remove)
-- CREATE TABLE IF NOT EXISTS HistorialModificacionUsuario ...

CREATE TABLE IF NOT EXISTS HistorialTransferencia (
    idHistorialTransferencia INT AUTO_INCREMENT PRIMARY KEY,
    idTransferencia INT NOT NULL,
    estadoAnterior VARCHAR(50),
    estadoNuevo VARCHAR(50),
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    idUsuario INT NOT NULL,
    observaciones TEXT,
    FOREIGN KEY (idTransferencia) REFERENCES Transferencia(idTransferencia),
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);

CREATE TABLE IF NOT EXISTS HistorialSolicitud (
    idHistorialSolicitud INT AUTO_INCREMENT PRIMARY KEY,
    idSolicitud INT NOT NULL,
    estadoAnterior VARCHAR(50),
    estadoNuevo VARCHAR(50),
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    idUsuario INT NOT NULL,
    observaciones TEXT,
    FOREIGN KEY (idSolicitud) REFERENCES SolicitudCompra(idSolicitud),
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
);
