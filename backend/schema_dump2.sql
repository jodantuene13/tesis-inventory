-- MariaDB dump 10.19  Distrib 10.4.32-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: tesis_inventory
-- ------------------------------------------------------
-- Server version	10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `atributo`
--

DROP TABLE IF EXISTS `atributo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `atributo` (
  `IdAtributo` int(11) NOT NULL AUTO_INCREMENT,
  `CodigoAtributo` varchar(255) NOT NULL,
  `Nombre` longtext NOT NULL,
  `TipoDato` longtext NOT NULL,
  `Unidad` longtext DEFAULT NULL,
  `Descripcion` longtext DEFAULT NULL,
  `Activo` tinyint(1) NOT NULL,
  `FechaCreacion` datetime(6) NOT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdAtributo`),
  UNIQUE KEY `IX_Atributo_CodigoAtributo` (`CodigoAtributo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `atributoopcion`
--

DROP TABLE IF EXISTS `atributoopcion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `atributoopcion` (
  `IdAtributoOpcion` int(11) NOT NULL AUTO_INCREMENT,
  `IdAtributo` int(11) NOT NULL,
  `CodigoOpcion` longtext NOT NULL,
  `Valor` longtext NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdAtributoOpcion`),
  KEY `IX_AtributoOpcion_IdAtributo` (`IdAtributo`),
  CONSTRAINT `FK_AtributoOpcion_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `atributo` (`IdAtributo`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `auditlog`
--

DROP TABLE IF EXISTS `auditlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `auditlog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EntityId` int(11) NOT NULL,
  `EntityType` longtext NOT NULL,
  `Action` longtext NOT NULL,
  `Timestamp` datetime(6) NOT NULL,
  `Details` longtext NOT NULL,
  `ExecutorId` int(11) DEFAULT NULL,
  `ExecutorName` longtext NOT NULL,
  `ExecutorEmail` longtext NOT NULL,
  `ExecutorRole` longtext NOT NULL,
  `ExecutorSede` longtext NOT NULL,
  `TargetUserSnapshot` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `familia`
--

DROP TABLE IF EXISTS `familia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `familia` (
  `IdFamilia` int(11) NOT NULL AUTO_INCREMENT,
  `IdRubro` int(11) NOT NULL,
  `CodigoFamilia` varchar(255) NOT NULL,
  `Nombre` longtext NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  `FechaCreacion` datetime(6) NOT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdFamilia`),
  UNIQUE KEY `IX_Familia_IdRubro_CodigoFamilia` (`IdRubro`,`CodigoFamilia`),
  CONSTRAINT `FK_Familia_Rubro_IdRubro` FOREIGN KEY (`IdRubro`) REFERENCES `rubro` (`IdRubro`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `familiaatributo`
--

DROP TABLE IF EXISTS `familiaatributo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `familiaatributo` (
  `IdFamiliaAtributo` int(11) NOT NULL AUTO_INCREMENT,
  `IdFamilia` int(11) NOT NULL,
  `IdAtributo` int(11) NOT NULL,
  `Obligatorio` tinyint(1) NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  `FechaCreacion` datetime(6) NOT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdFamiliaAtributo`),
  UNIQUE KEY `IX_FamiliaAtributo_IdFamilia_IdAtributo` (`IdFamilia`,`IdAtributo`),
  KEY `IX_FamiliaAtributo_IdAtributo` (`IdAtributo`),
  CONSTRAINT `FK_FamiliaAtributo_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `atributo` (`IdAtributo`),
  CONSTRAINT `FK_FamiliaAtributo_Familia_IdFamilia` FOREIGN KEY (`IdFamilia`) REFERENCES `familia` (`IdFamilia`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `historialtransferencia`
--

DROP TABLE IF EXISTS `historialtransferencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `historialtransferencia` (
  `idHistorialTransferencia` int(11) NOT NULL,
  `idTransferencia` int(11) NOT NULL,
  `estadoAnterior` int(11) NOT NULL,
  `estadoNuevo` int(11) NOT NULL,
  `fecha` datetime(6) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `observaciones` longtext DEFAULT NULL,
  PRIMARY KEY (`idHistorialTransferencia`),
  KEY `IX_HistorialTransferencia_idUsuario` (`idUsuario`),
  KEY `IX_HistorialTransferencia_idTransferencia` (`idTransferencia`),
  CONSTRAINT `FK_HistorialTransferencia_Transferencia_idTransferencia` FOREIGN KEY (`idTransferencia`) REFERENCES `transferencia` (`idTransferencia`) ON DELETE CASCADE,
  CONSTRAINT `FK_HistorialTransferencia_Usuario_idUsuario` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`idUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `movimiento`
--

DROP TABLE IF EXISTS `movimiento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movimiento` (
  `IdMovimiento` int(11) NOT NULL AUTO_INCREMENT,
  `IdProducto` int(11) NOT NULL,
  `IdSede` int(11) NOT NULL,
  `TipoMovimiento` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `Fecha` datetime(6) NOT NULL,
  `Motivo` int(11) NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `Observaciones` longtext DEFAULT NULL,
  `CantidadRestante` int(11) NOT NULL DEFAULT 0,
  `IdOperacion` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdMovimiento`),
  KEY `IX_Movimiento_IdProducto` (`IdProducto`),
  KEY `IX_Movimiento_IdSede` (`IdSede`),
  KEY `IX_Movimiento_IdUsuario` (`IdUsuario`),
  KEY `IX_Movimiento_IdOperacion` (`IdOperacion`),
  CONSTRAINT `FK_Movimiento_OperacionStock_IdOperacion` FOREIGN KEY (`IdOperacion`) REFERENCES `operacionstock` (`idOperacion`) ON DELETE CASCADE,
  CONSTRAINT `FK_Movimiento_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `producto` (`IdProducto`),
  CONSTRAINT `FK_Movimiento_Sede_IdSede` FOREIGN KEY (`IdSede`) REFERENCES `sede` (`idSede`),
  CONSTRAINT `FK_Movimiento_Usuario_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`idUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `operacionstock`
--

DROP TABLE IF EXISTS `operacionstock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `operacionstock` (
  `idOperacion` int(11) NOT NULL AUTO_INCREMENT,
  `idSede` int(11) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `tipoOperacion` int(11) NOT NULL,
  `fecha` datetime(6) NOT NULL,
  `motivo` int(11) NOT NULL,
  `ordenTrabajo` longtext DEFAULT NULL,
  `ordenCompra` longtext DEFAULT NULL,
  `ticketSolicitud` longtext DEFAULT NULL,
  `observaciones` longtext DEFAULT NULL,
  PRIMARY KEY (`idOperacion`),
  KEY `IX_OperacionStock_idSede` (`idSede`),
  KEY `IX_OperacionStock_idUsuario` (`idUsuario`),
  CONSTRAINT `FK_OperacionStock_Sede_idSede` FOREIGN KEY (`idSede`) REFERENCES `sede` (`idSede`),
  CONSTRAINT `FK_OperacionStock_Usuario_idUsuario` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`idUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `producto`
--

DROP TABLE IF EXISTS `producto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `producto` (
  `IdProducto` int(11) NOT NULL AUTO_INCREMENT,
  `IdFamilia` int(11) NOT NULL,
  `Sku` varchar(255) NOT NULL,
  `Nombre` longtext NOT NULL,
  `UnidadMedida` longtext NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  `FechaCreacion` datetime(6) NOT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdProducto`),
  UNIQUE KEY `IX_Producto_Sku` (`Sku`),
  KEY `IX_Producto_IdFamilia` (`IdFamilia`),
  CONSTRAINT `FK_Producto_Familia_IdFamilia` FOREIGN KEY (`IdFamilia`) REFERENCES `familia` (`IdFamilia`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `productoatributovalor`
--

DROP TABLE IF EXISTS `productoatributovalor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `productoatributovalor` (
  `IdProductoAtributoValor` int(11) NOT NULL AUTO_INCREMENT,
  `IdProducto` int(11) NOT NULL,
  `IdAtributo` int(11) NOT NULL,
  `ValorTexto` longtext DEFAULT NULL,
  `ValorNumero` int(11) DEFAULT NULL,
  `ValorDecimal` decimal(65,30) DEFAULT NULL,
  `ValorBool` tinyint(1) DEFAULT NULL,
  `ValorLista` longtext DEFAULT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdProductoAtributoValor`),
  UNIQUE KEY `IX_ProductoAtributoValor_IdProducto_IdAtributo` (`IdProducto`,`IdAtributo`),
  KEY `IX_ProductoAtributoValor_IdAtributo` (`IdAtributo`),
  CONSTRAINT `FK_ProductoAtributoValor_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `atributo` (`IdAtributo`),
  CONSTRAINT `FK_ProductoAtributoValor_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `producto` (`IdProducto`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rol`
--

DROP TABLE IF EXISTS `rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rol` (
  `idRol` int(11) NOT NULL AUTO_INCREMENT,
  `nombreRol` longtext NOT NULL,
  `descripcion` longtext NOT NULL,
  PRIMARY KEY (`idRol`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rubro`
--

DROP TABLE IF EXISTS `rubro`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rubro` (
  `IdRubro` int(11) NOT NULL AUTO_INCREMENT,
  `CodigoRubro` varchar(255) NOT NULL,
  `Nombre` longtext NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  `FechaCreacion` datetime(6) NOT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdRubro`),
  UNIQUE KEY `IX_Rubro_CodigoRubro` (`CodigoRubro`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sede`
--

DROP TABLE IF EXISTS `sede`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sede` (
  `idSede` int(11) NOT NULL AUTO_INCREMENT,
  `nombreSede` longtext NOT NULL,
  `Direccion` longtext NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 0,
  `CodigoSede` longtext NOT NULL,
  PRIMARY KEY (`idSede`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `solicitudcompra`
--

DROP TABLE IF EXISTS `solicitudcompra`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `solicitudcompra` (
  `IdSolicitudCompra` int(11) NOT NULL AUTO_INCREMENT,
  `IdSede` int(11) NOT NULL,
  `IdUsuarioSolicitante` int(11) NOT NULL,
  `IdUsuarioAprobador` int(11) DEFAULT NULL,
  `Estado` int(11) NOT NULL,
  `FechaSolicitud` datetime(6) NOT NULL,
  `FechaDecision` datetime(6) DEFAULT NULL,
  `Observaciones` longtext DEFAULT NULL,
  `MotivoRechazo` longtext DEFAULT NULL,
  `MotivoSolicitud` longtext DEFAULT NULL,
  `OrdenTrabajo` longtext DEFAULT NULL,
  `TareaARealizar` longtext DEFAULT NULL,
  `TicketSolicitud` longtext DEFAULT NULL,
  PRIMARY KEY (`IdSolicitudCompra`),
  KEY `IX_SolicitudCompra_IdSede` (`IdSede`),
  KEY `IX_SolicitudCompra_IdUsuarioAprobador` (`IdUsuarioAprobador`),
  KEY `IX_SolicitudCompra_IdUsuarioSolicitante` (`IdUsuarioSolicitante`),
  CONSTRAINT `FK_SolicitudCompra_Sede_IdSede` FOREIGN KEY (`IdSede`) REFERENCES `sede` (`idSede`),
  CONSTRAINT `FK_SolicitudCompra_Usuario_IdUsuarioAprobador` FOREIGN KEY (`IdUsuarioAprobador`) REFERENCES `usuario` (`idUsuario`),
  CONSTRAINT `FK_SolicitudCompra_Usuario_IdUsuarioSolicitante` FOREIGN KEY (`IdUsuarioSolicitante`) REFERENCES `usuario` (`idUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `solicitudcompradetalle`
--

DROP TABLE IF EXISTS `solicitudcompradetalle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `solicitudcompradetalle` (
  `IdSolicitudCompraDetalle` int(11) NOT NULL AUTO_INCREMENT,
  `IdSolicitudCompra` int(11) NOT NULL,
  `IdProducto` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  PRIMARY KEY (`IdSolicitudCompraDetalle`),
  KEY `IX_SolicitudCompraDetalle_IdProducto` (`IdProducto`),
  KEY `IX_SolicitudCompraDetalle_IdSolicitudCompra` (`IdSolicitudCompra`),
  CONSTRAINT `FK_SolicitudCompraDetalle_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `producto` (`IdProducto`),
  CONSTRAINT `FK_SolicitudCompraDetalle_SolicitudCompra_IdSolicitudCompra` FOREIGN KEY (`IdSolicitudCompra`) REFERENCES `solicitudcompra` (`IdSolicitudCompra`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `stock`
--

DROP TABLE IF EXISTS `stock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `stock` (
  `IdStock` int(11) NOT NULL AUTO_INCREMENT,
  `IdProducto` int(11) NOT NULL,
  `IdSede` int(11) NOT NULL,
  `CantidadActual` int(11) NOT NULL,
  `PuntoReposicion` int(11) NOT NULL,
  `FechaActualizacion` datetime(6) NOT NULL,
  PRIMARY KEY (`IdStock`),
  UNIQUE KEY `IX_Stock_IdProducto_IdSede` (`IdProducto`,`IdSede`),
  KEY `IX_Stock_IdSede` (`IdSede`),
  CONSTRAINT `FK_Stock_Producto_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `producto` (`IdProducto`),
  CONSTRAINT `FK_Stock_Sede_IdSede` FOREIGN KEY (`IdSede`) REFERENCES `sede` (`idSede`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transferencia`
--

DROP TABLE IF EXISTS `transferencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `transferencia` (
  `idTransferencia` int(11) NOT NULL,
  `idSedeOrigen` int(11) NOT NULL,
  `idSedeDestino` int(11) NOT NULL,
  `fechaSolicitud` datetime(6) NOT NULL,
  `estado` int(11) NOT NULL,
  `idUsuarioSolicita` int(11) NOT NULL,
  `observaciones` longtext DEFAULT NULL,
  `motivo` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`idTransferencia`),
  KEY `IX_Transferencia_idUsuarioSolicita` (`idUsuarioSolicita`),
  KEY `IX_Transferencia_idSedeOrigen` (`idSedeOrigen`),
  KEY `IX_Transferencia_idSedeDestino` (`idSedeDestino`),
  CONSTRAINT `FK_Transferencia_Sede_idSedeDestino` FOREIGN KEY (`idSedeDestino`) REFERENCES `sede` (`idSede`),
  CONSTRAINT `FK_Transferencia_Sede_idSedeOrigen` FOREIGN KEY (`idSedeOrigen`) REFERENCES `sede` (`idSede`),
  CONSTRAINT `FK_Transferencia_Usuario_idUsuarioSolicita` FOREIGN KEY (`idUsuarioSolicita`) REFERENCES `usuario` (`idUsuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transferenciadetalle`
--

DROP TABLE IF EXISTS `transferenciadetalle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `transferenciadetalle` (
  `idTransferenciaDetalle` int(11) NOT NULL AUTO_INCREMENT,
  `idTransferencia` int(11) NOT NULL,
  `idProducto` int(11) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `stockOrigenSnapshot` int(11) DEFAULT NULL,
  PRIMARY KEY (`idTransferenciaDetalle`),
  KEY `IX_TransferenciaDetalle_idProducto` (`idProducto`),
  KEY `IX_TransferenciaDetalle_idTransferencia` (`idTransferencia`),
  CONSTRAINT `FK_TransferenciaDetalle_Producto_idProducto` FOREIGN KEY (`idProducto`) REFERENCES `producto` (`IdProducto`),
  CONSTRAINT `FK_TransferenciaDetalle_Transferencia_idTransferencia` FOREIGN KEY (`idTransferencia`) REFERENCES `transferencia` (`idTransferencia`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombreUsuario` longtext NOT NULL,
  `email` longtext NOT NULL,
  `googleId` longtext DEFAULT NULL,
  `password` longtext DEFAULT NULL,
  `estado` tinyint(1) NOT NULL,
  `fechaRegistro` datetime(6) NOT NULL,
  `idRol` int(11) NOT NULL,
  `idSede` int(11) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  KEY `IX_Usuario_idRol` (`idRol`),
  KEY `IX_Usuario_idSede` (`idSede`),
  CONSTRAINT `FK_Usuario_Rol_idRol` FOREIGN KEY (`idRol`) REFERENCES `rol` (`idRol`) ON DELETE CASCADE,
  CONSTRAINT `FK_Usuario_Sede_idSede` FOREIGN KEY (`idSede`) REFERENCES `sede` (`idSede`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-09 17:47:04
