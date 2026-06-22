-- Migración: Grupos de Atributos
-- Ejecutar sobre la base de datos existente

CREATE TABLE `GrupoAtributo` (
    `IdGrupoAtributo` INT NOT NULL AUTO_INCREMENT,
    `CodigoGrupo`     VARCHAR(255) NOT NULL,
    `Nombre`          LONGTEXT NOT NULL,
    `Separador`       VARCHAR(10) NOT NULL DEFAULT '*',
    `UnidadSufijo`    VARCHAR(20) NULL,
    `Activo`          TINYINT(1) NOT NULL DEFAULT 1,
    `FechaCreacion`   DATETIME(6) NOT NULL,
    `FechaActualizacion` DATETIME(6) NOT NULL,
    CONSTRAINT `PK_GrupoAtributo` PRIMARY KEY (`IdGrupoAtributo`),
    CONSTRAINT `UQ_GrupoAtributo_Codigo` UNIQUE (`CodigoGrupo`)
) CHARACTER SET utf8mb4;

CREATE TABLE `GrupoAtributoItem` (
    `IdGrupoAtributoItem` INT NOT NULL AUTO_INCREMENT,
    `IdGrupoAtributo`     INT NOT NULL,
    `IdAtributo`          INT NOT NULL,
    `Orden`               INT NOT NULL DEFAULT 0,
    `Activo`              TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT `PK_GrupoAtributoItem` PRIMARY KEY (`IdGrupoAtributoItem`),
    CONSTRAINT `UQ_GrupoAtributoItem` UNIQUE (`IdGrupoAtributo`, `IdAtributo`),
    CONSTRAINT `FK_GrupoAtributoItem_Grupo`
        FOREIGN KEY (`IdGrupoAtributo`) REFERENCES `GrupoAtributo` (`IdGrupoAtributo`) ON DELETE CASCADE,
    CONSTRAINT `FK_GrupoAtributoItem_Atributo`
        FOREIGN KEY (`IdAtributo`) REFERENCES `Atributo` (`IdAtributo`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `FamiliaGrupoAtributo` (
    `IdFamiliaGrupoAtributo` INT NOT NULL AUTO_INCREMENT,
    `IdFamilia`              INT NOT NULL,
    `IdGrupoAtributo`        INT NOT NULL,
    `Obligatorio`            TINYINT(1) NOT NULL DEFAULT 0,
    `Activo`                 TINYINT(1) NOT NULL DEFAULT 1,
    `FechaCreacion`          DATETIME(6) NOT NULL,
    `FechaActualizacion`     DATETIME(6) NOT NULL,
    CONSTRAINT `PK_FamiliaGrupoAtributo` PRIMARY KEY (`IdFamiliaGrupoAtributo`),
    CONSTRAINT `UQ_FamiliaGrupoAtributo` UNIQUE (`IdFamilia`, `IdGrupoAtributo`),
    CONSTRAINT `FK_FamiliaGrupoAtributo_Familia`
        FOREIGN KEY (`IdFamilia`) REFERENCES `Familia` (`IdFamilia`) ON DELETE CASCADE,
    CONSTRAINT `FK_FamiliaGrupoAtributo_Grupo`
        FOREIGN KEY (`IdGrupoAtributo`) REFERENCES `GrupoAtributo` (`IdGrupoAtributo`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;
