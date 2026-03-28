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
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20260307010844_InitialCreate','9.0.0'),('20260307011803_AddDireccionToSede','9.0.0'),('20260307015528_InventorySchemaInitialization','9.0.0');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `atributo`
--

LOCK TABLES `atributo` WRITE;
/*!40000 ALTER TABLE `atributo` DISABLE KEYS */;
/*!40000 ALTER TABLE `atributo` ENABLE KEYS */;
UNLOCK TABLES;

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
  `Orden` int(11) NOT NULL,
  `Activo` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdAtributoOpcion`),
  KEY `IX_AtributoOpcion_IdAtributo` (`IdAtributo`),
  CONSTRAINT `FK_AtributoOpcion_Atributo_IdAtributo` FOREIGN KEY (`IdAtributo`) REFERENCES `atributo` (`IdAtributo`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `atributoopcion`
--

LOCK TABLES `atributoopcion` WRITE;
/*!40000 ALTER TABLE `atributoopcion` DISABLE KEYS */;
/*!40000 ALTER TABLE `atributoopcion` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `auditlog`
--

LOCK TABLES `auditlog` WRITE;
/*!40000 ALTER TABLE `auditlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `auditlog` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `familia`
--

LOCK TABLES `familia` WRITE;
/*!40000 ALTER TABLE `familia` DISABLE KEYS */;
/*!40000 ALTER TABLE `familia` ENABLE KEYS */;
UNLOCK TABLES;

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
  `Orden` int(11) NOT NULL,
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
-- Dumping data for table `familiaatributo`
--

LOCK TABLES `familiaatributo` WRITE;
/*!40000 ALTER TABLE `familiaatributo` DISABLE KEYS */;
/*!40000 ALTER TABLE `familiaatributo` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `producto`
--

LOCK TABLES `producto` WRITE;
/*!40000 ALTER TABLE `producto` DISABLE KEYS */;
/*!40000 ALTER TABLE `producto` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `productoatributovalor`
--

LOCK TABLES `productoatributovalor` WRITE;
/*!40000 ALTER TABLE `productoatributovalor` DISABLE KEYS */;
/*!40000 ALTER TABLE `productoatributovalor` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol`
--

LOCK TABLES `rol` WRITE;
/*!40000 ALTER TABLE `rol` DISABLE KEYS */;
/*!40000 ALTER TABLE `rol` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `rubro`
--

LOCK TABLES `rubro` WRITE;
/*!40000 ALTER TABLE `rubro` DISABLE KEYS */;
/*!40000 ALTER TABLE `rubro` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sede`
--

LOCK TABLES `sede` WRITE;
/*!40000 ALTER TABLE `sede` DISABLE KEYS */;
/*!40000 ALTER TABLE `sede` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `stock`
--

LOCK TABLES `stock` WRITE;
/*!40000 ALTER TABLE `stock` DISABLE KEYS */;
/*!40000 ALTER TABLE `stock` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-07 19:51:34
