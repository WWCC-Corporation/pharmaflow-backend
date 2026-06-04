using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Infrastructure.Context;

public partial class PharmaFlowDbContext : DbContext
{
    public PharmaFlowDbContext(DbContextOptions<PharmaFlowDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alerta> Alertas { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<Lote> Lotes { get; set; }

    public virtual DbSet<MovimientoInventario> MovimientoInventarios { get; set; }

    public virtual DbSet<MovimientosCaja> MovimientosCajas { get; set; }

    public virtual DbSet<Precio> Precios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StockLote> StockLotes { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<TurnosCaja> TurnosCajas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioSucursale> UsuarioSucursales { get; set; }

    public virtual DbSet<VStockFefo> VStockFefos { get; set; }

    public virtual DbSet<VStockPorProducto> VStockPorProductos { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("estado_compra", new[] { "pendiente", "recepcionada", "anulada" })
            .HasPostgresEnum("estado_venta", new[] { "completada", "anulada" })
            .HasPostgresEnum("metodo_pago", new[] { "efectivo", "tarjeta", "yape", "plin" })
            .HasPostgresEnum("moneda", new[] { "PEN", "USD" })
            .HasPostgresEnum("tipo_alerta", new[] { "stock_minimo", "por_vencer", "vencido", "otro" })
            .HasPostgresEnum("tipo_movimiento", new[] { "ENTRADA", "SALIDA", "AJUSTE", "DEVOLUCION" })
            .HasPostgresEnum("tipo_movimiento_caja", new[] { "APERTURA", "VENTA_EFECTIVO_INGRESO", "VUELTO_SALIDA", "INGRESO_MANUAL", "EGRESO_MANUAL", "ANULACION_INGRESO", "ANULACION_EGRESO", "CIERRE" })
            .HasPostgresExtension("extensions", "citext")
            .HasPostgresExtension("extensions", "pgcrypto");

        modelBuilder.Entity<Alerta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("alertas_pkey");

            entity.ToTable("alertas");

            entity.HasIndex(e => new { e.IdSucursal, e.Leida, e.CreatedAt }, "idx_alertas_sucursal_leida").IsDescending(false, false, true);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Leida)
                .HasDefaultValue(false)
                .HasColumnName("leida");
            entity.Property(e => e.Mensaje).HasColumnName("mensaje");
            entity.Property(e => e.Tipo)
                .HasColumnType("tipo_alerta")
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Alerta)
                .HasForeignKey(d => d.IdLote)
                .HasConstraintName("alertas_id_lote_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Alerta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("alertas_id_producto_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Alerta)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alertas_id_sucursal_fkey");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_log_pkey");

            entity.ToTable("audit_log");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Accion).HasColumnName("accion");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Detalle)
                .HasColumnType("jsonb")
                .HasColumnName("detalle");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.RegistroId).HasColumnName("registro_id");
            entity.Property(e => e.Tabla).HasColumnName("tabla");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("audit_log_id_sucursal_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("audit_log_usuario_id_fkey");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Dni, "clientes_dni_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Apellidos).HasColumnName("apellidos");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .HasColumnName("dni");
            entity.Property(e => e.Nombres).HasColumnName("nombres");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("compras_pkey");

            entity.ToTable("compras");

            entity.HasIndex(e => new { e.IdSucursal, e.Fecha }, "idx_compras_sucursal_fecha");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("now()")
                .HasColumnName("fecha");
            entity.Property(e => e.Estado)
                .HasColumnType("estado_compra")
                .HasColumnName("estado");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Moneda)
                .HasColumnType("moneda")
                .HasColumnName("moneda");
            entity.Property(e => e.TipoCambio)
                .HasPrecision(10, 3)
                .HasDefaultValueSql("1")
                .HasColumnName("tipo_cambio");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("compras_id_proveedor_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("compras_id_sucursal_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("compras_id_usuario_fkey");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detalle_compras_pkey");

            entity.ToTable("detalle_compras");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("detalle_compras_id_compra_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_compras_id_producto_fkey");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detalle_ventas_pkey");

            entity.ToTable("detalle_ventas");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdLote)
                .HasConstraintName("detalle_ventas_id_lote_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_ventas_id_producto_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("detalle_ventas_id_venta_fkey");
        });

        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_log_pkey");

            entity.ToTable("login_log");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Correo)
                .HasColumnType("extensions.citext")
                .HasColumnName("correo");
            entity.Property(e => e.Exito).HasColumnName("exito");
            entity.Property(e => e.Ip).HasColumnName("ip");
        });

        modelBuilder.Entity<Lote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lotes_pkey");

            entity.ToTable("lotes");

            entity.HasIndex(e => new { e.IdSucursal, e.IdProducto }, "idx_lotes_sucursal_producto");

            entity.HasIndex(e => new { e.IdSucursal, e.IdProducto, e.NumeroLote }, "lotes_id_sucursal_id_producto_numero_lote_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.NumeroLote).HasColumnName("numero_lote");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("lotes_id_compra_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotes_id_producto_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("lotes_id_sucursal_fkey");
        });

        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movimiento_inventario_pkey");

            entity.ToTable("movimiento_inventario");

            entity.HasIndex(e => new { e.IdSucursal, e.CreatedAt }, "idx_movimiento_inventario_sucursal").IsDescending(false, true);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.IdDetalleVenta).HasColumnName("id_detalle_venta");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Tipo)
                .HasColumnType("tipo_movimiento")
                .HasColumnName("tipo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("movimiento_inventario_id_compra_fkey");

            entity.HasOne(d => d.IdDetalleVentaNavigation).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.IdDetalleVenta)
                .HasConstraintName("movimiento_inventario_id_detalle_venta_fkey");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.IdLote)
                .HasConstraintName("movimiento_inventario_id_lote_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("movimiento_inventario_id_producto_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimiento_inventario_id_sucursal_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("movimiento_inventario_id_venta_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MovimientoInventarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("movimiento_inventario_usuario_id_fkey");
        });

        modelBuilder.Entity<MovimientosCaja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movimientos_caja_pkey");

            entity.ToTable("movimientos_caja");

            entity.HasIndex(e => new { e.IdSucursal, e.CreatedAt }, "idx_movimientos_caja_sucursal").IsDescending(false, true);

            entity.HasIndex(e => new { e.IdTurnoCaja, e.CreatedAt }, "idx_movimientos_caja_turno").IsDescending(false, true);

            entity.HasIndex(e => new { e.IdUsuario, e.CreatedAt }, "idx_movimientos_caja_usuario").IsDescending(false, true);

            entity.HasIndex(e => e.IdVenta, "idx_movimientos_caja_venta");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdTurnoCaja).HasColumnName("id_turno_caja");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Monto)
                .HasPrecision(12, 2)
                .HasColumnName("monto");
            entity.Property(e => e.Tipo)
                .HasColumnType("tipo_movimiento_caja")
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimientos_caja_id_sucursal_fkey");

            entity.HasOne(d => d.IdTurnoCajaNavigation).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.IdTurnoCaja)
                .HasConstraintName("movimientos_caja_id_turno_caja_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("movimientos_caja_id_usuario_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("movimientos_caja_id_venta_fkey");
        });

        modelBuilder.Entity<Precio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("precios_pkey");

            entity.ToTable("precios");

            entity.HasIndex(e => new { e.IdSucursal, e.IdProducto, e.Activo }, "idx_precios_sucursal_producto");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.PrecioCompra)
                .HasPrecision(12, 2)
                .HasColumnName("precio_compra");
            entity.Property(e => e.PrecioVenta)
                .HasPrecision(12, 2)
                .HasColumnName("precio_venta");
            entity.Property(e => e.VigenteDesde)
                .HasDefaultValueSql("now()")
                .HasColumnName("vigente_desde");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Precios)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("precios_id_producto_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Precios)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("precios_id_sucursal_fkey");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.HasIndex(e => e.CodigoBarra, "idx_productos_barra");

            entity.HasIndex(e => e.Nombre, "idx_productos_nombre");

            entity.HasIndex(e => e.CodigoBarra, "productos_codigo_barra_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.CodigoBarra).HasColumnName("codigo_barra");
            entity.Property(e => e.Concentracion).HasColumnName("concentracion");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FormaFarmaceutica).HasColumnName("forma_farmaceutica");
            entity.Property(e => e.Laboratorio).HasColumnName("laboratorio");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.PrincipioActivo).HasColumnName("principio_activo");
            entity.Property(e => e.RequiereReceta)
                .HasDefaultValue(false)
                .HasColumnName("requiere_receta");
            entity.Property(e => e.StockMinimo)
                .HasDefaultValue(5)
                .HasColumnName("stock_minimo");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.HasIndex(e => e.Ruc, "proveedores_ruc_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Ruc)
                .HasMaxLength(11)
                .HasColumnName("ruc");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_token_pkey");

            entity.ToTable("refresh_token");

            entity.HasIndex(e => e.ExpiresAt, "idx_refresh_token_expires_at");

            entity.HasIndex(e => e.RevokedAt, "idx_refresh_token_revoked_at");

            entity.HasIndex(e => e.UsuarioId, "idx_refresh_token_usuario_id");

            entity.HasIndex(e => e.Token, "refresh_token_token_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByIp).HasColumnName("created_by_ip");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.ReplacedByToken).HasColumnName("replaced_by_token");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("refresh_token_usuario_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "roles_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<StockLote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stock_lotes_pkey");

            entity.ToTable("stock_lotes");

            entity.HasIndex(e => e.IdSucursal, "idx_stock_lotes_sucursal");

            entity.HasIndex(e => e.IdLote, "stock_lotes_id_lote_key").IsUnique();

            entity.HasIndex(e => new { e.IdSucursal, e.IdLote }, "stock_lotes_id_sucursal_id_lote_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.StockActual)
                .HasDefaultValue(0)
                .HasColumnName("stock_actual");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");

            entity.HasOne(d => d.IdLoteNavigation).WithOne(p => p.StockLote)
                .HasForeignKey<StockLote>(d => d.IdLote)
                .HasConstraintName("stock_lotes_id_lote_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.StockLotes)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_lotes_id_sucursal_fkey");
        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sucursales_pkey");

            entity.ToTable("sucursales");

            entity.HasIndex(e => e.Codigo, "idx_sucursales_codigo");

            entity.HasIndex(e => e.Codigo, "sucursales_codigo_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Codigo)
                .HasMaxLength(30)
                .HasColumnName("codigo");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<TurnosCaja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("turnos_caja_pkey");

            entity.ToTable("turnos_caja");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Abierto)
                .HasDefaultValue(true)
                .HasColumnName("abierto");
            entity.Property(e => e.ClosedAt).HasColumnName("closed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DiferenciaCaja)
                .HasPrecision(12, 2)
                .HasColumnName("diferencia_caja");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.MontoApertura)
                .HasPrecision(12, 2)
                .HasColumnName("monto_apertura");
            entity.Property(e => e.MontoContado)
                .HasPrecision(12, 2)
                .HasColumnName("monto_contado");
            entity.Property(e => e.MontoVentas)
                .HasPrecision(12, 2)
                .HasColumnName("monto_ventas");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.TurnosCajas)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("turnos_caja_id_sucursal_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TurnosCajas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("turnos_caja_id_usuario_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "usuarios_correo_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Correo)
                .HasColumnType("extensions.citext")
                .HasColumnName("correo");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("usuarios_id_rol_fkey");
        });

        modelBuilder.Entity<UsuarioSucursale>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdSucursal }).HasName("usuario_sucursales_pkey");

            entity.ToTable("usuario_sucursales");

            entity.HasIndex(e => e.IdSucursal, "idx_usuario_sucursales_sucursal");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Principal)
                .HasDefaultValue(false)
                .HasColumnName("principal");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.UsuarioSucursales)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("usuario_sucursales_id_sucursal_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioSucursales)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("usuario_sucursales_id_usuario_fkey");
        });

        modelBuilder.Entity<VStockFefo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_stock_fefo");

            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.NumeroLote).HasColumnName("numero_lote");
            entity.Property(e => e.StockActual).HasColumnName("stock_actual");
            entity.Property(e => e.Sucursal).HasColumnName("sucursal");
        });

        modelBuilder.Entity<VStockPorProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_stock_por_producto");

            entity.Property(e => e.CodigoBarra).HasColumnName("codigo_barra");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");
            entity.Property(e => e.StockTotal).HasColumnName("stock_total");
            entity.Property(e => e.Sucursal).HasColumnName("sucursal");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ventas_pkey");

            entity.ToTable("ventas");

            entity.HasIndex(e => new { e.IdSucursal, e.Fecha }, "idx_ventas_sucursal_fecha");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("now()")
                .HasColumnName("fecha");
            entity.Property(e => e.Estado)
                .HasColumnType("estado_venta")
                .HasColumnName("estado");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdTurnoCaja).HasColumnName("id_turno_caja");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Metodo)
                .HasColumnType("metodo_pago")
                .HasColumnName("metodo");
            entity.Property(e => e.Moneda)
                .HasColumnType("moneda")
                .HasColumnName("moneda");
            entity.Property(e => e.MontoRecibido)
                .HasPrecision(12, 2)
                .HasColumnName("monto_recibido");
            entity.Property(e => e.MontoTotal)
                .HasPrecision(12, 2)
                .HasColumnName("monto_total");
            entity.Property(e => e.TipoCambio)
                .HasPrecision(10, 3)
                .HasDefaultValueSql("1")
                .HasColumnName("tipo_cambio");
            entity.Property(e => e.Vuelto)
                .HasPrecision(12, 2)
                .HasColumnName("vuelto");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("ventas_id_cliente_fkey");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_id_sucursal_fkey");

            entity.HasOne(d => d.IdTurnoCajaNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdTurnoCaja)
                .HasConstraintName("ventas_id_turno_caja_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("ventas_id_usuario_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
