# PharmaFlow - Base de Datos Neon

Este paquete deja la base de datos multi-sucursal preparada para un rebuild limpio en Neon.

Responsable unico de base de datos: Diego Armando Alvarez Ccompi.

## Orden de ejecucion

Para limpiar una base existente y volver a crearla:

1. Ejecutar `00_reset_public_schema.sql`
2. Ejecutar `schema.sql`
3. Ejecutar `02_verify_database.sql`
4. Opcional: ejecutar un seed demo cuando sea necesario

`schema.sql` es el schema limpio principal para Neon. El archivo `01_schema_neon_clean.sql` se conserva como version descriptiva del mismo schema.

## Multi-sucursal

El schema soporta 2, 3, 5 o mas boticas mediante la tabla `sucursales`.

- `productos` funciona como catalogo global.
- `precios`, `compras`, `lotes`, `stock_lotes`, `ventas`, `turnos_caja`, `movimientos_caja`, `movimiento_inventario` y `alertas` trabajan por `id_sucursal`.
- `usuario_sucursales` permite asignar usuarios a una o varias boticas.

## Regla principal

El equipo no modifica la base de datos directamente. Cualquier cambio de tablas, enums, vistas, indices, constraints o triggers debe pasar por Diego.

## Decision sobre triggers

La base limpia no usa triggers operativos. La logica de compras, ventas, inventario, FEFO, alertas y caja vive en el backend.

Se permiten constraints, foreign keys, checks, defaults, indices, vistas y extensiones.

## Scaffold con Entity Framework

La base actual usa `citext` para correos y enums PostgreSQL para estados, monedas, metodos y tipos.

EF scaffold genera las entidades principales, pero esas columnas especiales se revisan manualmente:

- `citext` se representa como `string` en C#.
- Los enums PostgreSQL se representan en `PharmaFlow.Domain/Enums`.
- El mapeo manual queda en `PharmaFlow.Infrastructure/Context/PharmaFlowDbContext.cs`.
