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

            // Seed Permisos
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
                    ("Transferencias", "Transferencias_Ver", "Ver módulo de Transferencias"),
                    ("Transferencias", "Transferencias_Crear", "Crear nueva solicitud de Transferencia"),
                    ("Transferencias", "Transferencias_PedidosAMiSede_Ver", "Ver Pedidos a mi Sede en Transferencias"),
                    ("Transferencias", "Transferencias_MisPedidos_Ver", "Ver Mis Pedidos en Transferencias"),
                    ("Transferencias", "Transferencias_Historico_Ver", "Ver Histórico de Transferencias"),
                    ("Parametricas", "Parametricas_Ver", "Ver módulo de Paramétricas del Gestor"),
                    ("Parametricas", "Parametricas_RubrosFamilias_ABM", "Ver, crear y modificar Rubros y Familias"),
                    ("Parametricas", "Parametricas_Atributos_ABM", "Ver, crear, asociar y modificar Atributos"),
                    ("Parametricas", "Parametricas_Productos_ABM", "Crear y modificar Productos"),
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
                var rubros = new List<Rubro>();
                string[] nombresRubros = { "Electrónica", "Computación", "Oficina", "Limpieza", "Ferretería", "Iluminación", "Muebles", "Seguridad", "Librería", "Indumentaria", "Herramientas", "Alimentos", "Bebidas", "Juguetes", "Deportes" };
                for (int i = 0; i < 15; i++)
                {
                    rubros.Add(new Rubro { CodigoRubro = $"RUB-{(i + 1):D3}", Nombre = nombresRubros[i], Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now });
                }
                context.Rubro.AddRange(rubros);
                context.SaveChanges();
            }

            // Seed Familias
            if (!context.Familia.Any())
            {
                var familias = new List<Familia>();
                string[] nombresFamilias = { "Televisores", "Notebooks", "Escritorios", "Detergentes", "Clavos", "Lámparas LED", "Sillas", "Cámaras", "Cuadernos", "Remeras", "Taladros", "Galletas", "Gaseosas", "Juegos de Mesa", "Pelotas" };
                for (int i = 0; i < 15; i++)
                {
                    familias.Add(new Familia { IdRubro = i + 1, CodigoFamilia = $"FAM-{(i + 1):D3}", Nombre = nombresFamilias[i], Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now });
                }
                context.Familia.AddRange(familias);
                context.SaveChanges();
            }

            // Seed Productos
            if (!context.Producto.Any())
            {
                var productos = new List<Producto>();
                string[] nombresProductos = { "Smart TV 50 4K", "Notebook Dell i7 16GB", "Escritorio Madera Premium", "Detergente Ala 1L", "Clavos 2 pulgadas x100", "Lámpara LED 9W Philips", "Silla Ergonómica Pro", "Cámara IP Ezviz WiFi", "Cuaderno A4 Rayado", "Remera Algodón Básica", "Taladro Bosch 500W", "Galletas Oreo 117g", "Coca Cola 2L Retornable", "Juego Monopoly Clásico", "Pelota de Fútbol N5" };
                string[] unidades = { "Unidad", "Unidad", "Unidad", "Litro", "Paquete", "Unidad", "Unidad", "Unidad", "Unidad", "Unidad", "Unidad", "Paquete", "Botella", "Unidad", "Unidad" };
                for (int i = 0; i < 15; i++)
                {
                    productos.Add(new Producto { IdFamilia = i + 1, Sku = $"PRD-{(i + 1):D4}", Nombre = nombresProductos[i], UnidadMedida = unidades[i], Activo = true, FechaCreacion = DateTime.Now, FechaActualizacion = DateTime.Now });
                }
                context.Producto.AddRange(productos);
                context.SaveChanges();
            }

            // Seed Stock
            if (!context.Stock.Any())
            {
                var stocks = new List<Stock>();
                for (int i = 1; i <= 15; i++)
                {
                    stocks.Add(new Stock { IdSede = 1, IdProducto = i, CantidadActual = i * 15, PuntoReposicion = 10, FechaActualizacion = DateTime.Now });
                }
                context.Stock.AddRange(stocks);
                context.SaveChanges();
            }
        }
    }
}
