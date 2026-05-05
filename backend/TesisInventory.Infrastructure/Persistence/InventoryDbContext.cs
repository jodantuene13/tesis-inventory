using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;

namespace TesisInventory.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; } // Singular naming as per DB refactor
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Sede> Sede { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Inventory DbSets
        public DbSet<Rubro> Rubro { get; set; }
        public DbSet<Familia> Familia { get; set; }
        public DbSet<Atributo> Atributo { get; set; }
        public DbSet<FamiliaAtributo> FamiliaAtributo { get; set; }
        public DbSet<AtributoOpcion> AtributoOpcion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ProductoAtributoValor> ProductoAtributoValor { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Movimiento> Movimiento { get; set; }
        public DbSet<OperacionStock> OperacionStock { get; set; }
        public DbSet<Transferencia> Transferencia { get; set; }
        public DbSet<TransferenciaDetalle> TransferenciaDetalle { get; set; }
        public DbSet<HistorialTransferencia> HistorialTransferencia { get; set; }
        public DbSet<SolicitudCompra> SolicitudCompra { get; set; }
        public DbSet<SolicitudCompraDetalle> SolicitudCompraDetalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map entities to singular table names if EF defaults to plural
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Rol>().ToTable("Rol");

            // Key configurations
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);

            // Column Mappings (Case sensitive / Special characters fix)
            modelBuilder.Entity<Usuario>().Property(u => u.IdUsuario).HasColumnName("idUsuario");
            modelBuilder.Entity<Usuario>().Property(u => u.NombreUsuario).HasColumnName("nombreUsuario");
            modelBuilder.Entity<Usuario>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<Usuario>().Property(u => u.GoogleId).HasColumnName("googleId");
            modelBuilder.Entity<Usuario>().Property(u => u.Password).HasColumnName("password");
            modelBuilder.Entity<Usuario>().Property(u => u.Estado).HasColumnName("estado");
            modelBuilder.Entity<Usuario>().Property(u => u.FechaRegistro).HasColumnName("fechaRegistro");
            modelBuilder.Entity<Usuario>().Property(u => u.IdRol).HasColumnName("idRol");
            modelBuilder.Entity<Usuario>().Property(u => u.IdSede).HasColumnName("idSede");

            modelBuilder.Entity<Rol>().Property(r => r.IdRol).HasColumnName("idRol");
            modelBuilder.Entity<Rol>().Property(r => r.NombreRol).HasColumnName("nombreRol");
            modelBuilder.Entity<Rol>().Property(r => r.Descripcion).HasColumnName("descripcion");

            // Relationships
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Sede)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(u => u.IdSede);

            modelBuilder.Entity<Sede>().ToTable("Sede");
            modelBuilder.Entity<Sede>().HasKey(s => s.IdSede);
            modelBuilder.Entity<Sede>().Property(s => s.IdSede).HasColumnName("idSede");
            modelBuilder.Entity<Sede>().Property(s => s.NombreSede).HasColumnName("nombreSede");

            // Audit
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog");
            modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);

            // Inventory Module Configurations
            modelBuilder.Entity<Rubro>().ToTable("Rubro");
            modelBuilder.Entity<Rubro>().HasKey(r => r.IdRubro);
            modelBuilder.Entity<Rubro>().HasIndex(r => r.CodigoRubro).IsUnique();

            modelBuilder.Entity<Familia>().ToTable("Familia");
            modelBuilder.Entity<Familia>().HasKey(f => f.IdFamilia);
            modelBuilder.Entity<Familia>().HasIndex(f => new { f.IdRubro, f.CodigoFamilia }).IsUnique();
            modelBuilder.Entity<Familia>()
                .HasOne(f => f.Rubro)
                .WithMany(r => r.Familias)
                .HasForeignKey(f => f.IdRubro)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Atributo>().ToTable("Atributo");
            modelBuilder.Entity<Atributo>().HasKey(a => a.IdAtributo);
            modelBuilder.Entity<Atributo>().HasIndex(a => a.CodigoAtributo).IsUnique();

            modelBuilder.Entity<FamiliaAtributo>().ToTable("FamiliaAtributo");
            modelBuilder.Entity<FamiliaAtributo>().HasKey(fa => fa.IdFamiliaAtributo);
            modelBuilder.Entity<FamiliaAtributo>().HasIndex(fa => new { fa.IdFamilia, fa.IdAtributo }).IsUnique();
            modelBuilder.Entity<FamiliaAtributo>()
                .HasOne(fa => fa.Familia)
                .WithMany(f => f.FamiliaAtributos)
                .HasForeignKey(fa => fa.IdFamilia)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FamiliaAtributo>()
                .HasOne(fa => fa.Atributo)
                .WithMany(a => a.FamiliaAtributos)
                .HasForeignKey(fa => fa.IdAtributo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AtributoOpcion>().ToTable("AtributoOpcion");
            modelBuilder.Entity<AtributoOpcion>().HasKey(ao => ao.IdAtributoOpcion);
            modelBuilder.Entity<AtributoOpcion>()
                .HasOne(ao => ao.Atributo)
                .WithMany(a => a.Opciones)
                .HasForeignKey(ao => ao.IdAtributo)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Producto>().HasKey(p => p.IdProducto);
            modelBuilder.Entity<Producto>().HasIndex(p => p.Sku).IsUnique();
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Familia)
                .WithMany(f => f.Productos)
                .HasForeignKey(p => p.IdFamilia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductoAtributoValor>().ToTable("ProductoAtributoValor");
            modelBuilder.Entity<ProductoAtributoValor>().HasKey(pav => pav.IdProductoAtributoValor);
            modelBuilder.Entity<ProductoAtributoValor>().HasIndex(pav => new { pav.IdProducto, pav.IdAtributo }).IsUnique();
            modelBuilder.Entity<ProductoAtributoValor>()
                .HasOne(pav => pav.Producto)
                .WithMany(p => p.ProductoAtributoValores)
                .HasForeignKey(pav => pav.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductoAtributoValor>()
                .HasOne(pav => pav.Atributo)
                .WithMany()
                .HasForeignKey(pav => pav.IdAtributo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<Stock>().HasKey(s => s.IdStock);
            modelBuilder.Entity<Stock>().HasIndex(s => new { s.IdProducto, s.IdSede }).IsUnique();
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Producto)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Sede)
                .WithMany(s => s.Stocks)
                .HasForeignKey(s => s.IdSede)
                .OnDelete(DeleteBehavior.Restrict);

            // Movimiento
            modelBuilder.Entity<Movimiento>().ToTable("Movimiento");
            modelBuilder.Entity<Movimiento>().HasKey(m => m.IdMovimiento);
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Producto)
                .WithMany(p => p.Movimientos)
                .HasForeignKey(m => m.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Sede)
                .WithMany(s => s.Movimientos)
                .HasForeignKey(m => m.IdSede)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Movimientos)
                .HasForeignKey(m => m.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.OperacionStock)
                .WithMany(o => o.Movimientos)
                .HasForeignKey(m => m.IdOperacion)
                .OnDelete(DeleteBehavior.Cascade);

            // OperacionStock
            modelBuilder.Entity<OperacionStock>().ToTable("OperacionStock");
            modelBuilder.Entity<OperacionStock>().HasKey(o => o.IdOperacion);
            modelBuilder.Entity<OperacionStock>().Property(o => o.IdOperacion).HasColumnName("idOperacion");
            modelBuilder.Entity<OperacionStock>().Property(o => o.IdSede).HasColumnName("idSede");
            modelBuilder.Entity<OperacionStock>().Property(o => o.IdUsuario).HasColumnName("idUsuario");
            modelBuilder.Entity<OperacionStock>().Property(o => o.TipoOperacion).HasColumnName("tipoOperacion");
            modelBuilder.Entity<OperacionStock>().Property(o => o.Fecha).HasColumnName("fecha");
            modelBuilder.Entity<OperacionStock>().Property(o => o.Motivo).HasColumnName("motivo");
            modelBuilder.Entity<OperacionStock>().Property(o => o.OrdenTrabajo).HasColumnName("ordenTrabajo");
            modelBuilder.Entity<OperacionStock>().Property(o => o.OrdenCompra).HasColumnName("ordenCompra");
            modelBuilder.Entity<OperacionStock>().Property(o => o.TicketSolicitud).HasColumnName("ticketSolicitud");
            modelBuilder.Entity<OperacionStock>().Property(o => o.Observaciones).HasColumnName("observaciones");

            modelBuilder.Entity<OperacionStock>()
                .HasOne(o => o.Sede)
                .WithMany(s => s.OperacionesStock)
                .HasForeignKey(o => o.IdSede)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OperacionStock>()
                .HasOne(o => o.Usuario)
                .WithMany(u => u.OperacionesStock)
                .HasForeignKey(o => o.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Transferencia
            modelBuilder.Entity<Transferencia>().ToTable("Transferencia");
            modelBuilder.Entity<Transferencia>().HasKey(t => t.IdTransferencia);
            modelBuilder.Entity<Transferencia>().Property(t => t.IdTransferencia).HasColumnName("idTransferencia");
            modelBuilder.Entity<Transferencia>().Property(t => t.IdSedeOrigen).HasColumnName("idSedeOrigen");
            modelBuilder.Entity<Transferencia>().Property(t => t.IdSedeDestino).HasColumnName("idSedeDestino");
            modelBuilder.Entity<Transferencia>().Property(t => t.FechaSolicitud).HasColumnName("fechaSolicitud");
            modelBuilder.Entity<Transferencia>().Property(t => t.Estado).HasColumnName("estado");
            modelBuilder.Entity<Transferencia>().Property(t => t.Motivo).HasColumnName("motivo");
            modelBuilder.Entity<Transferencia>().Property(t => t.IdUsuarioSolicita).HasColumnName("idUsuarioSolicita");
            modelBuilder.Entity<Transferencia>().Property(t => t.Observaciones).HasColumnName("observaciones");

            modelBuilder.Entity<Transferencia>()
                .HasOne(t => t.SedeOrigen)
                .WithMany(s => s.TransferenciasOrigen)
                .HasForeignKey(t => t.IdSedeOrigen)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transferencia>()
                .HasOne(t => t.SedeDestino)
                .WithMany(s => s.TransferenciasDestino)
                .HasForeignKey(t => t.IdSedeDestino)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transferencia>()
                .HasOne(t => t.UsuarioSolicita)
                .WithMany(u => u.TransferenciasSolicitadas)
                .HasForeignKey(t => t.IdUsuarioSolicita)
                .OnDelete(DeleteBehavior.Restrict);

            // TransferenciaDetalle
            modelBuilder.Entity<TransferenciaDetalle>().ToTable("TransferenciaDetalle");
            modelBuilder.Entity<TransferenciaDetalle>().HasKey(td => td.IdTransferenciaDetalle);
            modelBuilder.Entity<TransferenciaDetalle>().Property(td => td.IdTransferenciaDetalle).HasColumnName("idTransferenciaDetalle");
            modelBuilder.Entity<TransferenciaDetalle>().Property(td => td.IdTransferencia).HasColumnName("idTransferencia");
            modelBuilder.Entity<TransferenciaDetalle>().Property(td => td.IdProducto).HasColumnName("idProducto");
            modelBuilder.Entity<TransferenciaDetalle>().Property(td => td.Cantidad).HasColumnName("cantidad");
            modelBuilder.Entity<TransferenciaDetalle>().Property(td => td.StockOrigenSnapshot).HasColumnName("stockOrigenSnapshot");

            modelBuilder.Entity<TransferenciaDetalle>()
                .HasOne(td => td.Transferencia)
                .WithMany(t => t.Detalles)
                .HasForeignKey(td => td.IdTransferencia)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<TransferenciaDetalle>()
                .HasOne(td => td.Producto)
                .WithMany(p => p.TransferenciaDetalles)
                .HasForeignKey(td => td.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            // HistorialTransferencia
            modelBuilder.Entity<HistorialTransferencia>().ToTable("HistorialTransferencia");
            modelBuilder.Entity<HistorialTransferencia>().HasKey(h => h.IdHistorialTransferencia);
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.IdHistorialTransferencia).HasColumnName("idHistorialTransferencia");
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.IdTransferencia).HasColumnName("idTransferencia");
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.EstadoAnterior).HasColumnName("estadoAnterior");
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.EstadoNuevo).HasColumnName("estadoNuevo");
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.Fecha).HasColumnName("fecha");
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.IdUsuario).HasColumnName("idUsuario");
            modelBuilder.Entity<HistorialTransferencia>().Property(h => h.Observaciones).HasColumnName("observaciones");

            modelBuilder.Entity<HistorialTransferencia>()
                .HasOne(h => h.Transferencia)
                .WithMany(t => t.HistorialTransferencias)
                .HasForeignKey(h => h.IdTransferencia)
                .OnDelete(DeleteBehavior.Cascade);
            // HistorialTransferencia
            modelBuilder.Entity<HistorialTransferencia>()
                .HasOne(h => h.Usuario)
                .WithMany(u => u.HistorialTransferencias)
                .HasForeignKey(h => h.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // SolicitudCompra
            modelBuilder.Entity<SolicitudCompra>().ToTable("SolicitudCompra");
            modelBuilder.Entity<SolicitudCompra>().HasKey(s => s.IdSolicitudCompra);
            modelBuilder.Entity<SolicitudCompra>()
                .HasOne(s => s.Sede)
                .WithMany()
                .HasForeignKey(s => s.IdSede)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SolicitudCompra>()
                .HasOne(s => s.UsuarioSolicitante)
                .WithMany()
                .HasForeignKey(s => s.IdUsuarioSolicitante)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SolicitudCompra>()
                .HasOne(s => s.UsuarioAprobador)
                .WithMany()
                .HasForeignKey(s => s.IdUsuarioAprobador)
                .OnDelete(DeleteBehavior.Restrict);

            // SolicitudCompraDetalle
            modelBuilder.Entity<SolicitudCompraDetalle>().ToTable("SolicitudCompraDetalle");
            modelBuilder.Entity<SolicitudCompraDetalle>().HasKey(sd => sd.IdSolicitudCompraDetalle);
            modelBuilder.Entity<SolicitudCompraDetalle>()
                .HasOne(sd => sd.SolicitudCompra)
                .WithMany(s => s.Detalles)
                .HasForeignKey(sd => sd.IdSolicitudCompra)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SolicitudCompraDetalle>()
                .HasOne(sd => sd.Producto)
                .WithMany()
                .HasForeignKey(sd => sd.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
