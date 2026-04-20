CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    CREATE TABLE `AuditLog` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `EntityId` int NOT NULL,
        `EntityType` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Action` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Timestamp` datetime(6) NOT NULL,
        `Details` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ExecutorId` int NULL,
        `ExecutorName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ExecutorEmail` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ExecutorRole` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ExecutorSede` longtext CHARACTER SET utf8mb4 NOT NULL,
        `TargetUserSnapshot` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_AuditLog` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    CREATE TABLE `Rol` (
        `idRol` int NOT NULL AUTO_INCREMENT,
        `nombreRol` longtext CHARACTER SET utf8mb4 NOT NULL,
        `descripcion` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Rol` PRIMARY KEY (`idRol`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    CREATE TABLE `Sede` (
        `idSede` int NOT NULL AUTO_INCREMENT,
        `nombreSede` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Sede` PRIMARY KEY (`idSede`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    CREATE TABLE `Usuario` (
        `idUsuario` int NOT NULL AUTO_INCREMENT,
        `nombreUsuario` longtext CHARACTER SET utf8mb4 NOT NULL,
        `email` longtext CHARACTER SET utf8mb4 NOT NULL,
        `googleId` longtext CHARACTER SET utf8mb4 NULL,
        `password` longtext CHARACTER SET utf8mb4 NULL,
        `estado` tinyint(1) NOT NULL,
        `fechaRegistro` datetime(6) NOT NULL,
        `idRol` int NOT NULL,
        `idSede` int NOT NULL,
        CONSTRAINT `PK_Usuario` PRIMARY KEY (`idUsuario`),
        CONSTRAINT `FK_Usuario_Rol_idRol` FOREIGN KEY (`idRol`) REFERENCES `Rol` (`idRol`) ON DELETE CASCADE,
        CONSTRAINT `FK_Usuario_Sede_idSede` FOREIGN KEY (`idSede`) REFERENCES `Sede` (`idSede`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    CREATE INDEX `IX_Usuario_idRol` ON `Usuario` (`idRol`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    CREATE INDEX `IX_Usuario_idSede` ON `Usuario` (`idSede`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307010844_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260307010844_InitialCreate', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307011803_AddDireccionToSede') THEN

    ALTER TABLE `Usuario` ADD `SedeIdSede` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307011803_AddDireccionToSede') THEN

    ALTER TABLE `Sede` ADD `Direccion` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307011803_AddDireccionToSede') THEN

    CREATE INDEX `IX_Usuario_SedeIdSede` ON `Usuario` (`SedeIdSede`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307011803_AddDireccionToSede') THEN

    ALTER TABLE `Usuario` ADD CONSTRAINT `FK_Usuario_Sede_SedeIdSede` FOREIGN KEY (`SedeIdSede`) REFERENCES `Sede` (`idSede`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307011803_AddDireccionToSede') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260307011803_AddDireccionToSede', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    ALTER TABLE `Usuario` DROP FOREIGN KEY `FK_Usuario_Sede_SedeIdSede`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    ALTER TABLE `Usuario` DROP INDEX `IX_Usuario_SedeIdSede`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    ALTER TABLE `Usuario` DROP COLUMN `SedeIdSede`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    ALTER TABLE `Sede` ADD `Activo` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    ALTER TABLE `Sede` ADD `CodigoSede` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `Atributo` (
        `IdAtributo` int NOT NULL AUTO_INCREMENT,
        `CodigoAtributo` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `TipoDato` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Unidad` longtext CHARACTER SET utf8mb4 NULL,
        `Descripcion` longtext CHARACTER SET utf8mb4 NULL,
        `Activo` tinyint(1) NOT NULL,
        `FechaCreacion` datetime(6) NOT NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Atributo` PRIMARY KEY (`IdAtributo`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `Rubro` (
        `IdRubro` int NOT NULL AUTO_INCREMENT,
        `CodigoRubro` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        `FechaCreacion` datetime(6) NOT NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Rubro` PRIMARY KEY (`IdRubro`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `AtributoOpcion` (
        `IdAtributoOpcion` int NOT NULL AUTO_INCREMENT,
        `IdAtributo` int NOT NULL,
        `CodigoOpcion` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Valor` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Orden` int NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        CONSTRAINT `PK_AtributoOpcion` PRIMARY KEY (`IdAtributoOpcion`),
        CONSTRAINT `FK_AtributoOpcion_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `Atributo` (`IdAtributo`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `Familia` (
        `IdFamilia` int NOT NULL AUTO_INCREMENT,
        `IdRubro` int NOT NULL,
        `CodigoFamilia` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        `FechaCreacion` datetime(6) NOT NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Familia` PRIMARY KEY (`IdFamilia`),
        CONSTRAINT `FK_Familia_Rubro_IdRubro` FOREIGN KEY (`IdRubro`) REFERENCES `Rubro` (`IdRubro`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `FamiliaAtributo` (
        `IdFamiliaAtributo` int NOT NULL AUTO_INCREMENT,
        `IdFamilia` int NOT NULL,
        `IdAtributo` int NOT NULL,
        `Obligatorio` tinyint(1) NOT NULL,
        `Orden` int NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        `FechaCreacion` datetime(6) NOT NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_FamiliaAtributo` PRIMARY KEY (`IdFamiliaAtributo`),
        CONSTRAINT `FK_FamiliaAtributo_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `Atributo` (`IdAtributo`) ON DELETE RESTRICT,
        CONSTRAINT `FK_FamiliaAtributo_Familia_IdFamilia` FOREIGN KEY (`IdFamilia`) REFERENCES `Familia` (`IdFamilia`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `Producto` (
        `IdProducto` int NOT NULL AUTO_INCREMENT,
        `IdFamilia` int NOT NULL,
        `Sku` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UnidadMedida` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Activo` tinyint(1) NOT NULL,
        `FechaCreacion` datetime(6) NOT NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Producto` PRIMARY KEY (`IdProducto`),
        CONSTRAINT `FK_Producto_Familia_IdFamilia` FOREIGN KEY (`IdFamilia`) REFERENCES `Familia` (`IdFamilia`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `ProductoAtributoValor` (
        `IdProductoAtributoValor` int NOT NULL AUTO_INCREMENT,
        `IdProducto` int NOT NULL,
        `IdAtributo` int NOT NULL,
        `ValorTexto` longtext CHARACTER SET utf8mb4 NULL,
        `ValorNumero` int NULL,
        `ValorDecimal` decimal(65,30) NULL,
        `ValorBool` tinyint(1) NULL,
        `ValorLista` longtext CHARACTER SET utf8mb4 NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_ProductoAtributoValor` PRIMARY KEY (`IdProductoAtributoValor`),
        CONSTRAINT `FK_ProductoAtributoValor_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `Atributo` (`IdAtributo`) ON DELETE RESTRICT,
        CONSTRAINT `FK_ProductoAtributoValor_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `Producto` (`IdProducto`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE TABLE `Stock` (
        `IdStock` int NOT NULL AUTO_INCREMENT,
        `IdProducto` int NOT NULL,
        `IdSede` int NOT NULL,
        `CantidadActual` int NOT NULL,
        `PuntoReposicion` int NOT NULL,
        `FechaActualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_Stock` PRIMARY KEY (`IdStock`),
        CONSTRAINT `FK_Stock_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `Producto` (`IdProducto`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Stock_Sede_IdSede` FOREIGN KEY (`IdSede`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_Atributo_CodigoAtributo` ON `Atributo` (`CodigoAtributo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE INDEX `IX_AtributoOpcion_IdAtributo` ON `AtributoOpcion` (`IdAtributo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_Familia_IdRubro_CodigoFamilia` ON `Familia` (`IdRubro`, `CodigoFamilia`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE INDEX `IX_FamiliaAtributo_IdAtributo` ON `FamiliaAtributo` (`IdAtributo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_FamiliaAtributo_IdFamilia_IdAtributo` ON `FamiliaAtributo` (`IdFamilia`, `IdAtributo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE INDEX `IX_Producto_IdFamilia` ON `Producto` (`IdFamilia`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_Producto_Sku` ON `Producto` (`Sku`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE INDEX `IX_ProductoAtributoValor_IdAtributo` ON `ProductoAtributoValor` (`IdAtributo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_ProductoAtributoValor_IdProducto_IdAtributo` ON `ProductoAtributoValor` (`IdProducto`, `IdAtributo`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_Rubro_CodigoRubro` ON `Rubro` (`CodigoRubro`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE UNIQUE INDEX `IX_Stock_IdProducto_IdSede` ON `Stock` (`IdProducto`, `IdSede`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    CREATE INDEX `IX_Stock_IdSede` ON `Stock` (`IdSede`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260307015528_InventorySchemaInitialization') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260307015528_InventorySchemaInitialization', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE TABLE `Movimiento` (
        `IdMovimiento` int NOT NULL AUTO_INCREMENT,
        `IdProducto` int NOT NULL,
        `IdSede` int NOT NULL,
        `TipoMovimiento` int NOT NULL,
        `Cantidad` int NOT NULL,
        `Fecha` datetime(6) NOT NULL,
        `Motivo` int NOT NULL,
        `IdUsuario` int NOT NULL,
        `Observaciones` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Movimiento` PRIMARY KEY (`IdMovimiento`),
        CONSTRAINT `FK_Movimiento_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `Producto` (`IdProducto`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Movimiento_Sede_IdSede` FOREIGN KEY (`IdSede`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Movimiento_Usuario_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuario` (`idUsuario`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE TABLE `Transferencia` (
        `IdTransferencia` int NOT NULL AUTO_INCREMENT,
        `IdProducto` int NOT NULL,
        `IdSedeOrigen` int NOT NULL,
        `IdSedeDestino` int NOT NULL,
        `Cantidad` int NOT NULL,
        `FechaSolicitud` datetime(6) NOT NULL,
        `Estado` int NOT NULL,
        `IdUsuarioSolicita` int NOT NULL,
        `Observaciones` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Transferencia` PRIMARY KEY (`IdTransferencia`),
        CONSTRAINT `FK_Transferencia_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `Producto` (`IdProducto`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Transferencia_Sede_IdSedeDestino` FOREIGN KEY (`IdSedeDestino`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Transferencia_Sede_IdSedeOrigen` FOREIGN KEY (`IdSedeOrigen`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Transferencia_Usuario_IdUsuarioSolicita` FOREIGN KEY (`IdUsuarioSolicita`) REFERENCES `Usuario` (`idUsuario`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE TABLE `HistorialTransferencia` (
        `IdHistorialTransferencia` int NOT NULL AUTO_INCREMENT,
        `IdTransferencia` int NOT NULL,
        `EstadoAnterior` int NOT NULL,
        `EstadoNuevo` int NOT NULL,
        `Fecha` datetime(6) NOT NULL,
        `IdUsuario` int NOT NULL,
        `Observaciones` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_HistorialTransferencia` PRIMARY KEY (`IdHistorialTransferencia`),
        CONSTRAINT `FK_HistorialTransferencia_Transferencia_IdTransferencia` FOREIGN KEY (`IdTransferencia`) REFERENCES `Transferencia` (`IdTransferencia`) ON DELETE CASCADE,
        CONSTRAINT `FK_HistorialTransferencia_Usuario_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuario` (`idUsuario`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_HistorialTransferencia_IdTransferencia` ON `HistorialTransferencia` (`IdTransferencia`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_HistorialTransferencia_IdUsuario` ON `HistorialTransferencia` (`IdUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Movimiento_IdProducto` ON `Movimiento` (`IdProducto`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Movimiento_IdSede` ON `Movimiento` (`IdSede`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Movimiento_IdUsuario` ON `Movimiento` (`IdUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Transferencia_IdProducto` ON `Transferencia` (`IdProducto`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Transferencia_IdSedeDestino` ON `Transferencia` (`IdSedeDestino`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Transferencia_IdSedeOrigen` ON `Transferencia` (`IdSedeOrigen`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    CREATE INDEX `IX_Transferencia_IdUsuarioSolicita` ON `Transferencia` (`IdUsuarioSolicita`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260308074531_AddStockModule') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260308074531_AddStockModule', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260326202423_AddCantidadRestanteToMovimiento') THEN

    ALTER TABLE `Movimiento` ADD `CantidadRestante` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260326202423_AddCantidadRestanteToMovimiento') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260326202423_AddCantidadRestanteToMovimiento', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` DROP FOREIGN KEY `FK_HistorialTransferencia_Transferencia_IdTransferencia`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` DROP FOREIGN KEY `FK_HistorialTransferencia_Usuario_IdUsuario`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP FOREIGN KEY `FK_Transferencia_Producto_IdProducto`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP FOREIGN KEY `FK_Transferencia_Sede_IdSedeDestino`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP FOREIGN KEY `FK_Transferencia_Sede_IdSedeOrigen`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP FOREIGN KEY `FK_Transferencia_Usuario_IdUsuarioSolicita`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `Observaciones` `observaciones` longtext NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `IdUsuarioSolicita` `idUsuarioSolicita` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `IdSedeOrigen` `idSedeOrigen` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `IdSedeDestino` `idSedeDestino` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `IdProducto` `idProducto` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `FechaSolicitud` `fechaSolicitud` datetime(6) NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `Estado` `estado` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `Cantidad` `cantidad` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` CHANGE `IdTransferencia` `idTransferencia` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP INDEX `IX_Transferencia_IdUsuarioSolicita`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    CREATE INDEX `IX_Transferencia_idUsuarioSolicita` ON `Transferencia` (`idUsuarioSolicita`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP INDEX `IX_Transferencia_IdSedeOrigen`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    CREATE INDEX `IX_Transferencia_idSedeOrigen` ON `Transferencia` (`idSedeOrigen`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP INDEX `IX_Transferencia_IdSedeDestino`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    CREATE INDEX `IX_Transferencia_idSedeDestino` ON `Transferencia` (`idSedeDestino`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` DROP INDEX `IX_Transferencia_IdProducto`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    CREATE INDEX `IX_Transferencia_idProducto` ON `Transferencia` (`idProducto`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `Observaciones` `observaciones` longtext NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `IdUsuario` `idUsuario` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `IdTransferencia` `idTransferencia` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `Fecha` `fecha` datetime(6) NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `EstadoNuevo` `estadoNuevo` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `EstadoAnterior` `estadoAnterior` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` CHANGE `IdHistorialTransferencia` `idHistorialTransferencia` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` DROP INDEX `IX_HistorialTransferencia_IdUsuario`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    CREATE INDEX `IX_HistorialTransferencia_idUsuario` ON `HistorialTransferencia` (`idUsuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` DROP INDEX `IX_HistorialTransferencia_IdTransferencia`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    CREATE INDEX `IX_HistorialTransferencia_idTransferencia` ON `HistorialTransferencia` (`idTransferencia`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` ADD `motivo` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` ADD CONSTRAINT `FK_HistorialTransferencia_Transferencia_idTransferencia` FOREIGN KEY (`idTransferencia`) REFERENCES `Transferencia` (`idTransferencia`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `HistorialTransferencia` ADD CONSTRAINT `FK_HistorialTransferencia_Usuario_idUsuario` FOREIGN KEY (`idUsuario`) REFERENCES `Usuario` (`idUsuario`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` ADD CONSTRAINT `FK_Transferencia_Producto_idProducto` FOREIGN KEY (`idProducto`) REFERENCES `Producto` (`IdProducto`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` ADD CONSTRAINT `FK_Transferencia_Sede_idSedeDestino` FOREIGN KEY (`idSedeDestino`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` ADD CONSTRAINT `FK_Transferencia_Sede_idSedeOrigen` FOREIGN KEY (`idSedeOrigen`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    ALTER TABLE `Transferencia` ADD CONSTRAINT `FK_Transferencia_Usuario_idUsuarioSolicita` FOREIGN KEY (`idUsuarioSolicita`) REFERENCES `Usuario` (`idUsuario`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260328153544_FixTransferenciaCols') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260328153544_FixTransferenciaCols', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

