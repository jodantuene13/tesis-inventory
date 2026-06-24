using System;
using System.Linq;
using System.Collections.Generic;
using TesisInventory.Domain.Entities;
using TesisInventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TesisInventory.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InventoryDbContext context)
        {
            // Seed Roles
            if (!context.Rol.Any())
            {
                var roles = new Rol[]
                {
                    new Rol { IdRol = 1, NombreRol = "Administrador", Descripcion = "Acceso total al sistema", TodasLasSedes = true, LimitarOperacionSedePrimaria = false },
                    new Rol { IdRol = 2, NombreRol = "Operador", Descripcion = "Acceso limitado a operaciones diarias", TodasLasSedes = false, LimitarOperacionSedePrimaria = false },
                    new Rol { IdRol = 3, NombreRol = "Auditor", Descripcion = "Acceso de solo lectura para control", TodasLasSedes = true, LimitarOperacionSedePrimaria = true }
                };
                context.Rol.AddRange(roles);
                context.SaveChanges();
            }

            // Seed Permisos (inicial para BD vacía)
            if (!context.Permiso.Any())
            {
                var permisosData = new List<(string Modulo, string Nombre, string Descripcion)>
                {
                    ("Inicio", "Inicio_Ver", "Ver pantalla de Inicio"),
                    ("Inventario", "Inventario_Ver", "Ver módulo de Inventario"),
                    ("Inventario", "Inventario_StockLocal_Ver", "Ver Stock Local"),
                    ("Inventario", "Inventario_StockLocal_Historial_Ver", "Ver Historial de Stock Local"),
                    ("Inventario", "Inventario_StockLocal_Historial_VerMovimientos", "Ver Movimientos de Historial de Stock Local"),
                    ("Inventario", "Inventario_StockLocal_OpMultiples_VerCrear", "Ver y crear Operaciones Múltiples de Stock Local"),
                    ("Inventario", "Inventario_Remitos_Ver", "Ver Remitos"),
                    ("Inventario", "Inventario_Historial_Ver", "Ver Historial de Movimientos de Inventario"),
                    ("Solicitudes", "Solicitudes_VerBuscarImprimir", "Ver, buscar e imprimir Solicitudes de Compra"),
                    ("Solicitudes", "Solicitudes_Crear", "Crear Solicitudes de Compra"),
                    ("Solicitudes", "Solicitudes_Aprobar", "Aprobar o rechazar Solicitudes de Compra"),
                    ("Transferencias", "Transferencias_Ver", "Ver módulo de Transferencias"),
                    ("Transferencias", "Transferencias_Crear", "Crear nueva solicitud de Transferencia"),
                    ("Transferencias", "Transferencias_Aprobar", "Aprobar o rechazar solicitudes de Transferencia"),
                    ("Transferencias", "Transferencias_PedidosAMiSede_Ver", "Ver Pedidos a mi Sede en Transferencias"),
                    ("Transferencias", "Transferencias_MisPedidos_Ver", "Ver Mis Pedidos en Transferencias"),
                    ("Transferencias", "Transferencias_Historico_Ver", "Ver Histórico de Transferencias"),
                    ("Parametricas", "Parametricas_Ver", "Ver módulo de Paramétricas del Gestor"),
                    ("Parametricas", "Parametricas_RubrosFamilias_ABM", "Ver, crear y modificar Rubros y Familias"),
                    ("Parametricas", "Parametricas_Atributos_ABM", "Ver, crear, asociar y modificar Atributos"),
                    ("Parametricas", "Parametricas_GruposAtributos_ABM", "Ver, crear y modificar Grupos de Atributos"),
                    ("Parametricas", "Parametricas_UnidadesMedida_ABM", "Ver, crear y modificar Unidades de Medida"),
                    ("Parametricas", "Parametricas_Productos_ABM", "Crear y modificar Productos"),
                    ("Informes", "Informes_Ver", "Ver módulo de Informes"),
                    ("Informes", "Informes_AlertasStock_Ver", "Ver Informe de Stock bajo y alertas"),
                    ("Informes", "Informes_RotacionProductos_Ver", "Ver Informe de Rotación de Productos"),
                    ("Informes", "Informes_Transferencias_Ver", "Ver Informe de Transferencias y Préstamos"),
                    ("Informes", "Informes_SolicitudesCompra_Ver", "Ver Informe de Solicitudes de Compra"),
                    ("Configuracion", "ConfiguracionAdmin_Ver", "Ver Configuración Admin")
                };

                var permisos = permisosData.Select((p, index) => new Permiso
                {
                    IdPermiso = index + 1,
                    Modulo = p.Modulo,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion
                }).ToList();

                context.Permiso.AddRange(permisos);
                context.SaveChanges();

                // Asignar todos los permisos al Administrador
                if (!context.RolPermiso.Any())
                {
                    var adminRolPermisos = permisos.Select(p => new RolPermiso
                    {
                        IdRol = 1, // Administrador
                        IdPermiso = p.IdPermiso
                    }).ToList();
                    context.RolPermiso.AddRange(adminRolPermisos);
                    context.SaveChanges();
                }
            }
            else
            {
                // Seed incremental: agrega permisos nuevos a una BD ya inicializada
                SeedPermisosIncrementales(context);
            }

            // Seed Sedes
            if (!context.Sede.Any())
            {
                var sedes = new List<Sede>();
                for (int i = 1; i <= 15; i++)
                {
                    sedes.Add(new Sede { IdSede = i, NombreSede = i == 1 ? "Sede Central" : $"Sucursal {i}" });
                }
                context.Sede.AddRange(sedes);
                context.SaveChanges();
            }

            // Seed Users
            if (!context.Usuario.Any())
            {
                var usuarios = new List<Usuario>();
                usuarios.Add(new Usuario
                {
                    NombreUsuario = "Admin", Email = "admin@ucc.edu.ar", Password = "admin", Estado = true, FechaRegistro = DateTime.Now, IdRol = 1, IdSede = 1
                });
                for (int i = 2; i <= 15; i++)
                {
                    usuarios.Add(new Usuario
                    {
                        NombreUsuario = $"Operador {i}", Email = $"operador{i}@ucc.edu.ar", Password = "password", Estado = true, FechaRegistro = DateTime.Now, IdRol = 2, IdSede = i
                    });
                }
                context.Usuario.AddRange(usuarios);
                context.SaveChanges();
            }

            // Seed Rubros
            if (!context.Rubro.Any())
            {
                var rubros = new List<Rubro>
                {
                    new Rubro { CodigoRubro = "ELEC", Nombre = "Electricidad", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "ILUM", Nombre = "Iluminación", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "SAN", Nombre = "Sanitario / Plomería", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "CLIM", Nombre = "Climatización", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "EQEM", Nombre = "Equipos Electromecánicos", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "SEG", Nombre = "Seguridad Edilicia", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "CERR", Nombre = "Cerrajería", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now },
                    new Rubro { CodigoRubro = "PINT", Nombre = "Pintura", Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now }
                };
                context.Rubro.AddRange(rubros);
                context.SaveChanges();
            }

            // Seed Familias
            if (!context.Familia.Any())
            {
                var rubrosFromDb = context.Rubro.ToDictionary(r => r.CodigoRubro, r => r.IdRubro);
                var familias = new List<Familia>();

                void AddFamilia(string rubroCodigo, string familiaCodigo, string nombre)
                {
                    if (rubrosFromDb.TryGetValue(rubroCodigo, out var rubroId))
                    {
                        familias.Add(new Familia 
                        { 
                            IdRubro = rubroId, 
                            CodigoFamilia = familiaCodigo, 
                            Nombre = nombre, 
                            Activo = true, 
                            FechaCreacion = DateTime.Now, 
                            FechaActualizacion = DateTime.Now 
                        });
                    }
                }

                AddFamilia("ELEC", "TF", "Tomas y fichas");
                AddFamilia("ELEC", "PROT", "Protección");
                AddFamilia("ELEC", "CONT", "Control");
                AddFamilia("ELEC", "COND", "Conductores");
                AddFamilia("ELEC", "CAN", "Canalización");
                AddFamilia("ELEC", "CG", "Cajas y gabinetes");
                AddFamilia("ILUM", "LAM", "Lámparas y tubos");
                AddFamilia("ILUM", "LUMN", "Luminarias");
                AddFamilia("ILUM", "EMRG", "Emergencia y señalización");
                AddFamilia("ILUM", "SLED", "Sistemas LED");
                AddFamilia("SAN", "CC", "Conexiones y cañerías");
                AddFamilia("SAN", "GRIF", "Grifería");
                AddFamilia("SAN", "REPS", "Repuestos sanitarios");
                AddFamilia("SAN", "TAP", "Tapas, rejillas y accesorios sanitarios");
                AddFamilia("CLIM", "REF", "Refrigerantes");
                AddFamilia("CLIM", "VENT", "Ventilación");
                AddFamilia("EQEM", "BOMB", "Bombas y control hídrico");
                AddFamilia("SEG", "HID", "Hydrantes"); // Wait, original output had "Hidrantes", let's use "Hidrantes"
                AddFamilia("CERR", "CER", "Cerraduras");
                AddFamilia("CERR", "CIE", "Cierrapuertas");
                AddFamilia("PINT", "HER", "Herramientas de aplicación");
                AddFamilia("PINT", "PR", "Pinturas y recubrimientos");
                AddFamilia("PINT", "S", "Selladores y adhesivos");

                context.Familia.AddRange(familias);
                context.SaveChanges();
            }

            // Seed Productos
            // (No product seeds for the new database)

            // Seed Stock
            // (No stock seeds for the new database)
        }

        private static void SeedPermisosIncrementales(InventoryDbContext context)
        {
            var nuevosPermisos = new List<(string Modulo, string Nombre, string Descripcion)>
            {
                ("Solicitudes", "Solicitudes_Aprobar", "Aprobar o rechazar Solicitudes de Compra"),
                ("Transferencias", "Transferencias_Aprobar", "Aprobar o rechazar solicitudes de Transferencia"),
                ("Parametricas", "Parametricas_GruposAtributos_ABM", "Ver, crear y modificar Grupos de Atributos"),
                ("Parametricas", "Parametricas_UnidadesMedida_ABM", "Ver, crear y modificar Unidades de Medida"),
                ("Informes", "Informes_Ver", "Ver módulo de Informes"),
                ("Informes", "Informes_AlertasStock_Ver", "Ver Informe de Stock bajo y alertas"),
                ("Informes", "Informes_RotacionProductos_Ver", "Ver Informe de Rotación de Productos"),
                ("Informes", "Informes_Transferencias_Ver", "Ver Informe de Transferencias y Préstamos"),
                ("Informes", "Informes_SolicitudesCompra_Ver", "Ver Informe de Solicitudes de Compra"),
            };

            var existentes = context.Permiso.Select(p => p.Nombre).ToHashSet();
            var faltantes = nuevosPermisos.Where(p => !existentes.Contains(p.Nombre)).ToList();

            if (!faltantes.Any()) return;

            var maxId = context.Permiso.Max(p => p.IdPermiso);
            var permisosToAdd = faltantes.Select((p, i) => new Permiso
            {
                IdPermiso = maxId + i + 1,
                Modulo = p.Modulo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion
            }).ToList();

            context.Permiso.AddRange(permisosToAdd);
            context.SaveChanges();

            // Asignar los nuevos permisos al Administrador
            var adminPermisoIds = context.RolPermiso
                .Where(rp => rp.IdRol == 1)
                .Select(rp => rp.IdPermiso)
                .ToHashSet();

            var nuevosRolPermisos = permisosToAdd
                .Where(p => !adminPermisoIds.Contains(p.IdPermiso))
                .Select(p => new RolPermiso { IdRol = 1, IdPermiso = p.IdPermiso })
                .ToList();

            if (nuevosRolPermisos.Any())
            {
                context.RolPermiso.AddRange(nuevosRolPermisos);
                context.SaveChanges();
            }
        }
    }
}
