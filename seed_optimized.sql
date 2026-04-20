USE tesis_inventory;

-- Seed Roles
INSERT INTO rol (idRol, nombreRol, descripcion) VALUES 
(1, 'Administrador', 'Acceso total al sistema'),
(2, 'Operador', 'Acceso limitado a operaciones diarias');

-- Seed Sedes
INSERT INTO sede (idSede, nombreSede, Direccion, Activo, CodigoSede) VALUES 
(1, 'Sede Central', 'Av. Siempre Viva 742', 1, 'SC01');

-- Seed Usuarios (password 'admin', email corresponding to some tests)
INSERT INTO usuario (idUsuario, nombreUsuario, email, googleId, password, estado, fechaRegistro, idRol, idSede) VALUES 
(1, 'Admin', 'admin@ucc.edu.ar', '117062495606254048613', 'admin', 1, NOW(), 1, 1);
