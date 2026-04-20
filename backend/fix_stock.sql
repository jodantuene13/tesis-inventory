CREATE TABLE `Stock` (
    `IdStock` int NOT NULL AUTO_INCREMENT,
    `IdProducto` int NOT NULL,
    `IdSede` int NOT NULL,
    `CantidadActual` int NOT NULL DEFAULT 0,
    `PuntoReposicion` int NOT NULL DEFAULT 0,
    `FechaActualizacion` datetime(6) NOT NULL,
    PRIMARY KEY (`IdStock`),
    UNIQUE KEY `IX_Stock_IdProducto_IdSede` (`IdProducto`, `IdSede`),
    KEY `IX_Stock_IdSede` (`IdSede`),
    CONSTRAINT `FK_Stock_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `Producto` (`IdProducto`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Stock_Sede_IdSede` FOREIGN KEY (`IdSede`) REFERENCES `Sede` (`idSede`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;
