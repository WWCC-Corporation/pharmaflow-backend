-- ============================================================
-- PHARMAFLOW - SCHEMA LIMPIO MULTI-SUCURSAL PARA NEON
-- ============================================================
-- Responsable DB: Diego Armando Alvarez Ccompi
--
-- Principios:
-- - PostgreSQL limpio en Neon.
-- - Soporte para 2, 3, 5 o mas boticas/sucursales.
-- - Sin triggers operativos.
-- - Backend como duenio de la logica de negocio.
-- - Base como duenio de integridad, constraints, indices y vistas.
-- ============================================================

CREATE SCHEMA IF NOT EXISTS extensions;

CREATE EXTENSION IF NOT EXISTS citext WITH SCHEMA extensions;
CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA extensions;

SET search_path TO public, extensions;

-- ============================================================
-- VALORES CONTROLADOS
-- ============================================================

DO $$
BEGIN
    CREATE TYPE estado_compra AS ENUM ('pendiente', 'recepcionada', 'anulada');
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

DO $$
BEGIN
    CREATE TYPE estado_venta AS ENUM ('completada', 'anulada');
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

DO $$
BEGIN
    CREATE TYPE metodo_pago AS ENUM ('efectivo', 'tarjeta', 'yape', 'plin');
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

DO $$
BEGIN
    CREATE TYPE moneda AS ENUM ('PEN', 'USD');
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

DO $$
BEGIN
    CREATE TYPE tipo_movimiento AS ENUM ('ENTRADA', 'SALIDA', 'AJUSTE', 'DEVOLUCION');
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

DO $$
BEGIN
    CREATE TYPE tipo_movimiento_caja AS ENUM (
        'APERTURA',
        'VENTA_EFECTIVO_INGRESO',
        'VUELTO_SALIDA',
        'INGRESO_MANUAL',
        'EGRESO_MANUAL',
        'ANULACION_INGRESO',
        'ANULACION_EGRESO',
        'CIERRE'
    );
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

DO $$
BEGIN
    CREATE TYPE tipo_alerta AS ENUM ('stock_minimo', 'por_vencer', 'vencido', 'otro');
EXCEPTION
    WHEN duplicate_object THEN NULL;
END $$;

-- ============================================================
-- ORGANIZACION / SUCURSALES
-- ============================================================

CREATE TABLE IF NOT EXISTS sucursales (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    codigo VARCHAR(30) UNIQUE NOT NULL,
    nombre TEXT NOT NULL,
    direccion TEXT,
    telefono TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- ============================================================
-- SEGURIDAD
-- ============================================================

CREATE TABLE IF NOT EXISTS roles (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS usuarios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    correo CITEXT UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    nombres VARCHAR(100),
    apellidos VARCHAR(100),
    id_rol INT REFERENCES roles(id),
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS usuario_sucursales (
    id_usuario UUID NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    id_sucursal UUID NOT NULL REFERENCES sucursales(id) ON DELETE CASCADE,
    principal BOOLEAN NOT NULL DEFAULT FALSE,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    PRIMARY KEY (id_usuario, id_sucursal)
);

CREATE TABLE IF NOT EXISTS login_log (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    correo CITEXT,
    ip TEXT,
    exito BOOLEAN,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS audit_log (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID REFERENCES usuarios(id),
    id_sucursal UUID REFERENCES sucursales(id),
    accion TEXT,
    tabla TEXT,
    registro_id UUID,
    detalle JSONB,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS refresh_token (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID NOT NULL REFERENCES usuarios(id) ON DELETE CASCADE,
    token TEXT NOT NULL UNIQUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    expires_at TIMESTAMPTZ NOT NULL,
    revoked_at TIMESTAMPTZ,
    created_by_ip TEXT,
    replaced_by_token TEXT
);

-- ============================================================
-- MAESTROS GLOBALES
-- ============================================================

CREATE TABLE IF NOT EXISTS productos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre TEXT NOT NULL,
    principio_activo TEXT,
    laboratorio TEXT,
    forma_farmaceutica TEXT,
    concentracion TEXT,
    codigo_barra TEXT UNIQUE,
    stock_minimo INT NOT NULL DEFAULT 5 CHECK (stock_minimo >= 0),
    requiere_receta BOOLEAN NOT NULL DEFAULT FALSE,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS precios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID REFERENCES sucursales(id),
    id_producto UUID NOT NULL REFERENCES productos(id) ON DELETE CASCADE,
    precio_compra NUMERIC(12,2),
    precio_venta NUMERIC(12,2) NOT NULL CHECK (precio_venta >= 0),
    vigente_desde TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS proveedores (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre TEXT NOT NULL,
    ruc VARCHAR(11) UNIQUE,
    telefono TEXT,
    correo TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS clientes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    dni VARCHAR(8) UNIQUE,
    nombres TEXT,
    apellidos TEXT,
    telefono TEXT,
    correo TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

-- ============================================================
-- COMPRAS
-- ============================================================

CREATE TABLE IF NOT EXISTS compras (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_proveedor UUID REFERENCES proveedores(id),
    id_usuario UUID REFERENCES usuarios(id),
    fecha TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    estado estado_compra NOT NULL DEFAULT 'pendiente',
    moneda moneda NOT NULL DEFAULT 'PEN',
    tipo_cambio NUMERIC(10,3) NOT NULL DEFAULT 1 CHECK (tipo_cambio > 0)
);

CREATE TABLE IF NOT EXISTS detalle_compras (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_compra UUID NOT NULL REFERENCES compras(id) ON DELETE CASCADE,
    id_producto UUID NOT NULL REFERENCES productos(id),
    cantidad INT NOT NULL CHECK (cantidad > 0),
    precio_unitario NUMERIC(12,2) CHECK (precio_unitario >= 0)
);

-- ============================================================
-- INVENTARIO POR SUCURSAL
-- ============================================================

CREATE TABLE IF NOT EXISTS lotes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_producto UUID NOT NULL REFERENCES productos(id),
    id_compra UUID REFERENCES compras(id),
    numero_lote TEXT NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE (id_sucursal, id_producto, numero_lote)
);

CREATE TABLE IF NOT EXISTS stock_lotes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_lote UUID NOT NULL UNIQUE REFERENCES lotes(id) ON DELETE CASCADE,
    stock_actual INT NOT NULL DEFAULT 0 CHECK (stock_actual >= 0),
    version INT NOT NULL DEFAULT 1,
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    UNIQUE (id_sucursal, id_lote)
);

-- ============================================================
-- VENTAS / CAJA POR SUCURSAL
-- ============================================================

CREATE TABLE IF NOT EXISTS turnos_caja (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_usuario UUID REFERENCES usuarios(id),
    monto_apertura NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (monto_apertura >= 0),
    monto_ventas NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (monto_ventas >= 0),
    monto_contado NUMERIC(12,2),
    diferencia_caja NUMERIC(12,2),
    abierto BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    closed_at TIMESTAMPTZ
);

CREATE TABLE IF NOT EXISTS ventas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_cliente UUID REFERENCES clientes(id),
    id_usuario UUID REFERENCES usuarios(id),
    id_turno_caja UUID REFERENCES turnos_caja(id),
    fecha TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    metodo metodo_pago,
    estado estado_venta NOT NULL DEFAULT 'completada',
    moneda moneda NOT NULL DEFAULT 'PEN',
    tipo_cambio NUMERIC(10,3) NOT NULL DEFAULT 1 CHECK (tipo_cambio > 0),
    monto_total NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (monto_total >= 0),
    monto_recibido NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (monto_recibido >= 0),
    vuelto NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (vuelto >= 0)
);

CREATE TABLE IF NOT EXISTS detalle_ventas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_venta UUID NOT NULL REFERENCES ventas(id) ON DELETE CASCADE,
    id_lote UUID REFERENCES lotes(id),
    id_producto UUID NOT NULL REFERENCES productos(id),
    cantidad INT NOT NULL CHECK (cantidad > 0),
    precio_unitario NUMERIC(12,2) NOT NULL CHECK (precio_unitario >= 0)
);

CREATE TABLE IF NOT EXISTS movimiento_inventario (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    tipo tipo_movimiento NOT NULL,
    id_producto UUID REFERENCES productos(id),
    id_lote UUID REFERENCES lotes(id),
    id_compra UUID REFERENCES compras(id),
    id_venta UUID REFERENCES ventas(id),
    id_detalle_venta UUID REFERENCES detalle_ventas(id),
    cantidad INT NOT NULL CHECK (cantidad > 0),
    usuario_id UUID REFERENCES usuarios(id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS movimientos_caja (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_turno_caja UUID REFERENCES turnos_caja(id),
    id_venta UUID REFERENCES ventas(id),
    id_usuario UUID REFERENCES usuarios(id),
    tipo tipo_movimiento_caja NOT NULL,
    monto NUMERIC(12,2) NOT NULL CHECK (monto >= 0),
    descripcion TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- ============================================================
-- ALERTAS POR SUCURSAL
-- ============================================================

CREATE TABLE IF NOT EXISTS alertas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    id_sucursal UUID NOT NULL REFERENCES sucursales(id),
    id_producto UUID REFERENCES productos(id),
    id_lote UUID REFERENCES lotes(id),
    tipo tipo_alerta,
    mensaje TEXT,
    leida BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- ============================================================
-- VISTAS
-- ============================================================

CREATE OR REPLACE VIEW v_stock_por_producto AS
SELECT
    s.id AS id_sucursal,
    s.nombre AS sucursal,
    p.id,
    p.nombre,
    p.codigo_barra,
    p.stock_minimo,
    COALESCE(SUM(sl.stock_actual), 0) AS stock_total,
    CASE
        WHEN COALESCE(SUM(sl.stock_actual), 0) = 0 THEN 'sin_stock'
        WHEN COALESCE(SUM(sl.stock_actual), 0) <= p.stock_minimo THEN 'stock_bajo'
        ELSE 'ok'
    END AS estado
FROM sucursales s
CROSS JOIN productos p
LEFT JOIN lotes l ON l.id_sucursal = s.id AND l.id_producto = p.id
LEFT JOIN stock_lotes sl ON sl.id_lote = l.id
WHERE s.activo = TRUE
  AND p.activo = TRUE
GROUP BY s.id, s.nombre, p.id;

CREATE OR REPLACE VIEW v_stock_fefo AS
SELECT
    s.id AS id_sucursal,
    s.nombre AS sucursal,
    l.id,
    p.nombre,
    l.numero_lote,
    l.fecha_vencimiento,
    sl.stock_actual,
    CASE
        WHEN l.fecha_vencimiento < CURRENT_DATE THEN 'vencido'
        WHEN l.fecha_vencimiento < CURRENT_DATE + INTERVAL '30 days' THEN 'por_vencer'
        ELSE 'vigente'
    END AS estado
FROM stock_lotes sl
JOIN lotes l ON l.id = sl.id_lote
JOIN productos p ON p.id = l.id_producto
JOIN sucursales s ON s.id = sl.id_sucursal
WHERE sl.stock_actual > 0;

-- ============================================================
-- INDICES
-- ============================================================

CREATE INDEX IF NOT EXISTS idx_sucursales_codigo ON sucursales(codigo);
CREATE INDEX IF NOT EXISTS idx_usuario_sucursales_sucursal ON usuario_sucursales(id_sucursal);
CREATE INDEX IF NOT EXISTS idx_productos_nombre ON productos(nombre);
CREATE INDEX IF NOT EXISTS idx_productos_barra ON productos(codigo_barra);
CREATE INDEX IF NOT EXISTS idx_precios_sucursal_producto ON precios(id_sucursal, id_producto, activo);
CREATE INDEX IF NOT EXISTS idx_compras_sucursal_fecha ON compras(id_sucursal, fecha);
CREATE INDEX IF NOT EXISTS idx_ventas_sucursal_fecha ON ventas(id_sucursal, fecha);
CREATE INDEX IF NOT EXISTS idx_lotes_sucursal_producto ON lotes(id_sucursal, id_producto);
CREATE INDEX IF NOT EXISTS idx_stock_lotes_sucursal ON stock_lotes(id_sucursal);
CREATE INDEX IF NOT EXISTS idx_alertas_sucursal_leida ON alertas(id_sucursal, leida, created_at DESC);
CREATE INDEX IF NOT EXISTS idx_refresh_token_usuario_id ON refresh_token(usuario_id);
CREATE INDEX IF NOT EXISTS idx_refresh_token_expires_at ON refresh_token(expires_at);
CREATE INDEX IF NOT EXISTS idx_refresh_token_revoked_at ON refresh_token(revoked_at);
CREATE INDEX IF NOT EXISTS idx_movimientos_caja_turno ON movimientos_caja(id_turno_caja, created_at DESC);
CREATE INDEX IF NOT EXISTS idx_movimientos_caja_venta ON movimientos_caja(id_venta);
CREATE INDEX IF NOT EXISTS idx_movimientos_caja_usuario ON movimientos_caja(id_usuario, created_at DESC);
CREATE INDEX IF NOT EXISTS idx_movimientos_caja_sucursal ON movimientos_caja(id_sucursal, created_at DESC);
CREATE INDEX IF NOT EXISTS idx_movimiento_inventario_sucursal ON movimiento_inventario(id_sucursal, created_at DESC);

-- ============================================================
-- DATOS BASE
-- ============================================================

INSERT INTO roles (nombre) VALUES
('admin'),
('farmaceutico'),
('cajero'),
('almacenero')
ON CONFLICT (nombre) DO NOTHING;

INSERT INTO sucursales (codigo, nombre, direccion, telefono)
VALUES
('BOTICA-01', 'Botica Principal', NULL, NULL),
('BOTICA-02', 'Botica Secundaria', NULL, NULL)
ON CONFLICT (codigo) DO NOTHING;
