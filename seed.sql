-- Archivo Seed de Inicialización - Tesis Inventory
-- Fecha: 2026-03-05
-- Este archivo contiene la data base inicial requerida para el funcionamiento.

-- SEDES INICIALES
INSERT IGNORE INTO Sede (IdSede, CodigoSede, NombreSede, Direccion, Activo) VALUES
(1, 'CEN', 'Casa Central', 'Av. Corrientes 1234, CABA', 1),
(2, 'NOR', 'Sucursal Norte', 'Av. Cabildo 4567, CABA', 1);

-- CARGOS
INSERT IGNORE INTO Cargo (IdCargo, NombreCargo, Activo) VALUES
(1, 'Administrador del Sistema', 1),
(2, 'Gerente de Sucursal', 1),
(3, 'Vendedor', 1);

-- ROLES DE SEGURIDAD
INSERT IGNORE INTO Rol (IdRol, NombreRol, Permisos, Activo) VALUES
(1, 'SuperAdmin', '["all"]', 1),
(2, 'Gerente', '["read_inventory","write_inventory","read_users"]', 1),
(3, 'Cajero', '["read_inventory","create_sale"]', 1);

-- USUARIO ADMINISTRADOR DEFECTO (Password: admin123 -> Hash de BCrypt estándar ej)
INSERT IGNORE INTO Usuario (IdUsuario, IdCargo, SedeId, NombreComponente, Apellido, NumDoc, Email, Telefono, Activo, FechaAltaUsuario, PasswordHash) VALUES
(1, 1, 1, 'Admin', 'Super', '11111111', 'admin@inventory.local', '11-0000-0000', 1, NOW(), '$2b$10$w6z/Z.X5xI1QZ4QO7.0eGuqZ..IuexV/hA88A4O1xQe8RRx4.Z.lK'); -- Hash de ejemplo

-- INVENTARIO: RUBROS
INSERT IGNORE INTO Rubro (IdRubro, CodigoRubro, Nombre, Activo) VALUES
(1, 'ELEC', 'Electrónica', 1),
(2, 'FERR', 'Ferretería', 1),
(3, 'HOG', 'Hogar y Bazar', 1);

-- INVENTARIO: FAMILIAS
INSERT IGNORE INTO Familia (IdFamilia, IdRubro, CodigoFamilia, Nombre, Activo) VALUES
(1, 1, 'CABL', 'Cables y Conductores', 1),
(2, 1, 'COMP', 'Computación', 1),
(3, 2, 'HERR', 'Herramientas Manuales', 1),
(4, 3, 'TAZA', 'Tazas y Vasos', 1);

-- INVENTARIO: ATRIBUTOS MAESTROS
INSERT IGNORE INTO Atributo (IdAtributo, CodigoAtributo, Nombre, TipoDato, Unidad, Descripcion, Activo) VALUES
(1, 'LARGO', 'Largo', 'DECIMAL', 'MTS', 'Longitud del producto', 1),
(2, 'COLOR', 'Color', 'LIST', NULL, 'Color principal', 1),
(3, 'CAPAC', 'Capacidad', 'NUMBER', 'CC', 'Capacidad volumétrica', 1),
(4, 'BLIND', 'Blindado', 'BOOLEAN', NULL, 'Es o no blindado', 1);

-- INVENTARIO: OPCIONES DE LISTAS (Para el atributo COLOR, ID 2)
INSERT IGNORE INTO AtributoOpcion (IdAtributoOpcion, IdAtributo, CodigoOpcion, Valor, Orden, Activo) VALUES
(1, 2, 'ROJ', 'Rojo', 1, 1),
(2, 2, 'NEG', 'Negro', 2, 1),
(3, 2, 'BLA', 'Blanco', 3, 1),
(4, 2, 'AZU', 'Azul', 4, 1);

-- INVENTARIO: ASIGNACION A FAMILIAS
-- Familiar 1 (Cables) -> Requiere Largo(obligatorio), Color y Blindado
INSERT IGNORE INTO FamiliaAtributo (IdFamiliaAtributo, IdFamilia, IdAtributo, Obligatorio, Orden, Activo) VALUES
(1, 1, 1, 1, 1, 1), -- Largo
(2, 1, 2, 1, 2, 1), -- Color
(3, 1, 4, 0, 3, 1); -- Blindado

-- Familiar 4 (Tazas) -> Requiere Color y Capacidad
INSERT IGNORE INTO FamiliaAtributo (IdFamiliaAtributo, IdFamilia, IdAtributo, Obligatorio, Orden, Activo) VALUES
(4, 4, 3, 1, 1, 1), -- Capacidad
(5, 4, 2, 1, 2, 1); -- Color

-- INVENTARIO: PRODUCTO DE PRUEBA
INSERT IGNORE INTO Producto (IdProducto, IdFamilia, Sku, Nombre, UnidadMedida, Activo) VALUES
(1, 1, 'ELEC-CABL-0001', 'Cable Coaxial RG6', 'MTS', 1);

-- INVENTARIO: VALORES DE ATRIBUTO PARA PRODUCTO DE PRUEBA
INSERT IGNORE INTO ProductoAtributoValor (IdProductoAtributoValor, IdProducto, IdAtributo, ValorTexto, ValorNumero, ValorDecimal, ValorBool, ValorLista) VALUES
(1, 1, 1, NULL, NULL, 100.00, NULL, NULL), -- Largo 100mts
(2, 1, 2, NULL, NULL, NULL, NULL, 'NEG'), -- Color Negro
(3, 1, 4, NULL, NULL, NULL, 1, NULL); -- Blindado SI

-- FIN.
