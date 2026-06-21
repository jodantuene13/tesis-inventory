SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE `auditlog`;
TRUNCATE TABLE `historialtransferencia`;
TRUNCATE TABLE `transferenciadetalle`;
TRUNCATE TABLE `transferencia`;
TRUNCATE TABLE `solicitudcompradetalle`;
TRUNCATE TABLE `solicitudcompra`;
TRUNCATE TABLE `movimiento`;
TRUNCATE TABLE `operacionstock`;
TRUNCATE TABLE `stock`;
TRUNCATE TABLE `productoatributovalor`;
TRUNCATE TABLE `producto`;
TRUNCATE TABLE `familiaatributo`;
TRUNCATE TABLE `atributoopcion`;
TRUNCATE TABLE `atributo`;
TRUNCATE TABLE `familia`;
TRUNCATE TABLE `rubro`;
TRUNCATE TABLE `usuario`;
TRUNCATE TABLE `sede`;
TRUNCATE TABLE `rol`;

SET FOREIGN_KEY_CHECKS = 1;

-- 1. Roles
INSERT INTO `rol` (`idRol`, `nombreRol`, `descripcion`) VALUES 
(1, 'Administrador', 'Acceso total al sistema'),
(2, 'Operador', 'Acceso limitado a operaciones diarias'),
(3, 'Encargado de Depósito', 'Gestión de inventario y transferencias');

-- 2. Sedes
INSERT INTO `sede` (`idSede`, `nombreSede`, `Direccion`, `Activo`, `CodigoSede`) VALUES 
(1, 'Sede Central', 'Av. Colón 123', 1, 'SC01'),
(2, 'Sucursal Norte', 'Rafael Núñez 4500', 1, 'SN01'),
(3, 'Depósito Externo', 'Circunvalación 200', 1, 'DE01');

-- 3. Usuarios
INSERT INTO `usuario` (`idUsuario`, `nombreUsuario`, `email`, `googleId`, `password`, `estado`, `fechaRegistro`, `idRol`, `idSede`) VALUES 
(1, 'Admin', 'admin@tesis.edu.ar', '117062495606254048613', 'admin', 1, NOW(), 1, 1),
(2, 'Juan Operador', 'juan@tesis.edu.ar', NULL, 'operador123', 1, NOW(), 2, 2),
(3, 'Maria Encargada', 'maria@tesis.edu.ar', NULL, 'encargada123', 1, NOW(), 3, 1);

-- 4. Rubros
INSERT INTO `rubro` (`IdRubro`, `CodigoRubro`, `Nombre`, `Activo`, `FechaCreacion`, `FechaActualizacion`) VALUES 
(1, 'RUB-ELEC', 'Electrónica', 1, NOW(), NOW()),
(2, 'RUB-LIBR', 'Librería', 1, NOW(), NOW()),
(3, 'RUB-LIMP', 'Limpieza', 1, NOW(), NOW());

-- 5. Familias
INSERT INTO `familia` (`IdFamilia`, `IdRubro`, `CodigoFamilia`, `Nombre`, `Activo`, `FechaCreacion`, `FechaActualizacion`) VALUES 
(1, 1, 'FAM-COMP', 'Computadoras', 1, NOW(), NOW()),
(2, 1, 'FAM-PERI', 'Periféricos', 1, NOW(), NOW()),
(3, 2, 'FAM-CUAD', 'Cuadernos y Anotadores', 1, NOW(), NOW()),
(4, 3, 'FAM-ARTL', 'Artículos de Limpieza', 1, NOW(), NOW());

-- 6. Atributos
INSERT INTO `atributo` (`IdAtributo`, `CodigoAtributo`, `Nombre`, `TipoDato`, `Unidad`, `Descripcion`, `Activo`, `FechaCreacion`, `FechaActualizacion`) VALUES 
(1, 'ATR-MARCA', 'Marca', 'Texto', NULL, 'Marca del producto', 1, NOW(), NOW()),
(2, 'ATR-RAM', 'Memoria RAM', 'Numero', 'GB', 'Capacidad de memoria RAM', 1, NOW(), NOW()),
(3, 'ATR-TAMA', 'Tamaño', 'Texto', NULL, 'Tamaño general', 1, NOW(), NOW());

-- 7. Atributo Opciones (para los que son tipo lista, si aplica, aunque pusimos Texto)
INSERT INTO `atributoopcion` (`IdAtributoOpcion`, `IdAtributo`, `CodigoOpcion`, `Valor`, `Activo`) VALUES 
(1, 1, 'OPC-DELL', 'Dell', 1),
(2, 1, 'OPC-HP', 'HP', 1);

-- 8. Familia-Atributo
INSERT INTO `familiaatributo` (`IdFamiliaAtributo`, `IdFamilia`, `IdAtributo`, `Obligatorio`, `Activo`, `FechaCreacion`, `FechaActualizacion`) VALUES 
(1, 1, 1, 1, 1, NOW(), NOW()), -- Computadoras -> Marca
(2, 1, 2, 1, 1, NOW(), NOW()), -- Computadoras -> RAM
(3, 2, 1, 1, 1, NOW(), NOW()); -- Perifericos -> Marca

-- 9. Productos
INSERT INTO `producto` (`IdProducto`, `IdFamilia`, `Sku`, `Nombre`, `UnidadMedida`, `Activo`, `FechaCreacion`, `FechaActualizacion`) VALUES 
(1, 1, 'SKU-COMP-001', 'Notebook Dell Inspiron 15', 'Unidad', 1, NOW(), NOW()),
(2, 2, 'SKU-PERI-001', 'Mouse Inalámbrico HP', 'Unidad', 1, NOW(), NOW()),
(3, 3, 'SKU-CUAD-001', 'Cuaderno Universitario A4 120 hojas', 'Unidad', 1, NOW(), NOW()),
(4, 4, 'SKU-LIMP-001', 'Lavandina Concentrada 1L', 'Litro', 1, NOW(), NOW());

-- 10. Producto Atributo Valor
INSERT INTO `productoatributovalor` (`IdProductoAtributoValor`, `IdProducto`, `IdAtributo`, `ValorTexto`, `ValorNumero`, `ValorDecimal`, `ValorBool`, `ValorLista`, `FechaActualizacion`) VALUES 
(1, 1, 1, 'Dell', NULL, NULL, NULL, NULL, NOW()),
(2, 1, 2, NULL, 16, NULL, NULL, NULL, NOW()),
(3, 2, 1, 'HP', NULL, NULL, NULL, NULL, NOW());

-- 11. Stock
INSERT INTO `stock` (`IdStock`, `IdProducto`, `IdSede`, `CantidadActual`, `PuntoReposicion`, `FechaActualizacion`) VALUES 
(1, 1, 1, 15, 5, NOW()),
(2, 2, 1, 50, 10, NOW()),
(3, 3, 1, 100, 20, NOW()),
(4, 4, 1, 25, 10, NOW()),
(5, 1, 2, 3, 2, NOW()),
(6, 2, 2, 10, 5, NOW());

-- 12. OperacionStock (Ingreso inicial de inventario)
INSERT INTO `operacionstock` (`idOperacion`, `idSede`, `idUsuario`, `tipoOperacion`, `fecha`, `motivo`, `ordenTrabajo`, `ordenCompra`, `ticketSolicitud`, `observaciones`) VALUES 
(1, 1, 1, 0, NOW(), 0, NULL, 'OC-1001', NULL, 'Ingreso inicial de stock al sistema');

-- 13. Movimiento
INSERT INTO `movimiento` (`IdMovimiento`, `IdProducto`, `IdSede`, `TipoMovimiento`, `Cantidad`, `Fecha`, `Motivo`, `IdUsuario`, `Observaciones`, `CantidadRestante`, `IdOperacion`) VALUES 
(1, 1, 1, 0, 15, NOW(), 0, 1, 'Stock inicial', 15, 1),
(2, 2, 1, 0, 50, NOW(), 0, 1, 'Stock inicial', 50, 1),
(3, 3, 1, 0, 100, NOW(), 0, 1, 'Stock inicial', 100, 1),
(4, 4, 1, 0, 25, NOW(), 0, 1, 'Stock inicial', 25, 1);

-- 14. SolicitudCompra
INSERT INTO `solicitudcompra` (`IdSolicitudCompra`, `IdSede`, `IdUsuarioSolicitante`, `IdUsuarioAprobador`, `Estado`, `FechaSolicitud`, `FechaDecision`, `Observaciones`, `MotivoRechazo`, `MotivoSolicitud`, `OrdenTrabajo`, `TareaARealizar`, `TicketSolicitud`) VALUES 
(1, 2, 2, NULL, 0, NOW(), NULL, 'Se necesita reposición urgente', NULL, 'Falta de stock para operativo', NULL, 'Renovación de equipos', 'TKT-992');

-- 15. SolicitudCompraDetalle
INSERT INTO `solicitudcompradetalle` (`IdSolicitudCompraDetalle`, `IdSolicitudCompra`, `IdProducto`, `Cantidad`) VALUES 
(1, 1, 1, 5),
(2, 1, 2, 10);

-- 16. Transferencia (idTransferencia no es autoincrement en DDL, let's set it to 1001)
INSERT INTO `transferencia` (`idTransferencia`, `idSedeOrigen`, `idSedeDestino`, `fechaSolicitud`, `estado`, `idUsuarioSolicita`, `observaciones`, `motivo`) VALUES 
(1001, 1, 2, NOW(), 0, 2, 'Transferencia para cubrir faltante', 0);

-- 17. TransferenciaDetalle
INSERT INTO `transferenciadetalle` (`idTransferenciaDetalle`, `idTransferencia`, `idProducto`, `cantidad`, `stockOrigenSnapshot`) VALUES 
(1, 1001, 1, 2, 15);

-- 18. HistorialTransferencia (idHistorialTransferencia tampoco es autoincrement en DDL, set 1)
INSERT INTO `historialtransferencia` (`idHistorialTransferencia`, `idTransferencia`, `estadoAnterior`, `estadoNuevo`, `fecha`, `idUsuario`, `observaciones`) VALUES 
(1, 1001, 0, 0, NOW(), 2, 'Creación de transferencia');

-- 19. AuditLog
INSERT INTO `auditlog` (`Id`, `EntityId`, `EntityType`, `Action`, `Timestamp`, `Details`, `ExecutorId`, `ExecutorName`, `ExecutorEmail`, `ExecutorRole`, `ExecutorSede`, `TargetUserSnapshot`) VALUES 
(1, 1, 'OperacionStock', 'Create', NOW(), 'Ingreso inicial registrado', 1, 'Admin', 'admin@tesis.edu.ar', 'Administrador', 'Sede Central', '{}');
