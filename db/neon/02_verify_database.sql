-- ============================================================
-- PHARMAFLOW - VERIFICACION DE BASE NEON MULTI-SUCURSAL
-- ============================================================
-- Ejecutar despues de schema.sql.
-- Debe devolver tablas, enums e indices esperados.
-- La lista de triggers debe quedar vacia.
-- ============================================================

-- Tablas y vistas principales
SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'public'
  AND table_name IN (
    'sucursales',
    'roles',
    'usuarios',
    'usuario_sucursales',
    'login_log',
    'audit_log',
    'refresh_token',
    'productos',
    'precios',
    'proveedores',
    'clientes',
    'compras',
    'detalle_compras',
    'lotes',
    'stock_lotes',
    'turnos_caja',
    'ventas',
    'detalle_ventas',
    'movimiento_inventario',
    'movimientos_caja',
    'alertas',
    'v_stock_por_producto',
    'v_stock_fefo'
  )
ORDER BY table_name;

-- Enums PostgreSQL principales
SELECT
    t.typname AS enum_name,
    e.enumlabel AS enum_value
FROM pg_type t
JOIN pg_enum e ON t.oid = e.enumtypid
JOIN pg_namespace n ON n.oid = t.typnamespace
WHERE n.nspname = 'public'
  AND t.typname IN (
    'estado_compra',
    'estado_venta',
    'metodo_pago',
    'moneda',
    'tipo_alerta',
    'tipo_movimiento',
    'tipo_movimiento_caja'
  )
ORDER BY t.typname, e.enumsortorder;

-- Triggers: debe quedar vacio en la base limpia.
SELECT
    event_object_table AS table_name,
    trigger_name
FROM information_schema.triggers
WHERE event_object_schema = 'public'
ORDER BY event_object_table, trigger_name;

-- Indices principales
SELECT indexname, tablename
FROM pg_indexes
WHERE schemaname = 'public'
  AND indexname IN (
    'idx_sucursales_codigo',
    'idx_usuario_sucursales_sucursal',
    'idx_productos_nombre',
    'idx_productos_barra',
    'idx_precios_sucursal_producto',
    'idx_compras_sucursal_fecha',
    'idx_ventas_sucursal_fecha',
    'idx_lotes_sucursal_producto',
    'idx_stock_lotes_sucursal',
    'idx_alertas_sucursal_leida',
    'idx_refresh_token_usuario_id',
    'idx_refresh_token_expires_at',
    'idx_refresh_token_revoked_at',
    'idx_movimientos_caja_turno',
    'idx_movimientos_caja_venta',
    'idx_movimientos_caja_usuario',
    'idx_movimientos_caja_sucursal',
    'idx_movimiento_inventario_sucursal'
  )
ORDER BY tablename, indexname;

-- Sucursales base
SELECT codigo, nombre, activo
FROM sucursales
ORDER BY codigo;
