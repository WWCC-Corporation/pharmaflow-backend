# Responsabilidades De Modulos

Este documento resume que debe hacer cada modulo del backend y donde debe ir su logica dentro de la arquitectura.

## Regla General

La logica no debe vivir en los controllers.

El flujo esperado es:

```text
Controller
  -> Command o Query
  -> Handler
  -> Port / Repository
  -> Adapter en Infrastructure
  -> DbContext o servicio externo
```

Los controllers solo deben:

- recibir el request,
- llamar al handler correspondiente,
- devolver la respuesta HTTP.

Los handlers deben:

- ejecutar el caso de uso,
- validar reglas del proceso,
- coordinar consultas y guardados,
- llamar repositorios o servicios externos mediante puertos,
- devolver DTOs de respuesta.

Los repositorios/adapters deben:

- consultar o guardar datos,
- usar `PharmaFlowDbContext`,
- implementar interfaces definidas como ports.

## Estructura Por Modulo

Cada modulo en `PharmaFlow.Application` debe seguir esta estructura:

```text
Modulo
  Commands
  Queries
  Handlers
  DTOs
  Validators
  Mappings
```

Ejemplo:

```text
PharmaFlow.Application
  Compras
    Commands
    Queries
    Handlers
    DTOs
    Validators
    Mappings
```

## Reparto De Trabajo

### Alex - Compras Y Proveedores

Modulo:

```text
PharmaFlow.Application/Compras
PharmaFlow.Application/Proveedores
PharmaFlow.Persistence/Controllers/Compras
PharmaFlow.Persistence/Controllers/Proveedores
PharmaFlow.Infrastructure/Adapters/Repositories/Compras
PharmaFlow.Infrastructure/Adapters/Repositories/Proveedores
```

Debe implementar:

- registrar compras,
- listar compras,
- obtener compra por id,
- registrar proveedores,
- actualizar proveedores,
- listar proveedores,
- obtener proveedor por id,
- desactivar proveedor.

Logica esperada en compras:

- una compra debe tener proveedor si aplica,
- una compra debe tener sucursal,
- una compra debe tener detalle de productos,
- al recepcionar mercaderia debe afectar inventario de la sucursal,
- debe registrar lotes si corresponde,
- debe generar movimientos de inventario de entrada.

Archivos esperados:

```text
Compras
  Commands
    CrearCompraCommand.cs
    RecepcionarCompraCommand.cs
  Queries
    ListarComprasQuery.cs
    ObtenerCompraPorIdQuery.cs
  Handlers
    CrearCompraHandler.cs
    RecepcionarCompraHandler.cs
    ListarComprasHandler.cs
    ObtenerCompraPorIdHandler.cs
  DTOs
    CrearCompraRequestDto.cs
    CompraResponseDto.cs
    DetalleCompraDto.cs
  Validators
    CrearCompraValidator.cs
  Mappings
    CompraMapper.cs
```

### Mariel - Inventario

Modulo:

```text
PharmaFlow.Application/Inventario
PharmaFlow.Persistence/Controllers/Inventario
PharmaFlow.Infrastructure/Adapters/Repositories/Inventario
```

Debe implementar:

- consultar stock por producto,
- consultar stock por lote,
- consultar stock por sucursal,
- registrar ajustes de inventario,
- registrar movimientos de inventario,
- controlar lotes y vencimientos.

Logica esperada en inventario:

- el stock debe manejarse por sucursal,
- el stock debe manejarse por lote cuando aplique,
- cada entrada o salida debe generar un movimiento de inventario,
- no se debe permitir stock negativo salvo que el caso de uso lo defina explicitamente,
- las ventas deben disminuir stock,
- las compras recepcionadas deben aumentar stock.

Archivos esperados:

```text
Inventario
  Commands
    AjustarStockCommand.cs
    RegistrarMovimientoInventarioCommand.cs
  Queries
    ObtenerStockPorProductoQuery.cs
    ObtenerStockPorSucursalQuery.cs
    ObtenerStockFefoQuery.cs
  Handlers
    AjustarStockHandler.cs
    RegistrarMovimientoInventarioHandler.cs
    ObtenerStockPorProductoHandler.cs
    ObtenerStockPorSucursalHandler.cs
  DTOs
    StockLoteDto.cs
    MovimientoInventarioDto.cs
  Validators
    AjustarStockValidator.cs
  Mappings
    InventarioMapper.cs
```

### Kevin - Caja

Modulo:

```text
PharmaFlow.Application/Caja
PharmaFlow.Persistence/Controllers/Caja
PharmaFlow.Infrastructure/Adapters/Repositories/Caja
```

Debe implementar:

- apertura de caja,
- cierre de caja,
- movimientos de caja,
- ingresos manuales,
- egresos manuales,
- resumen por turno,
- validacion de caja abierta por usuario/sucursal.

Logica esperada en caja:

- no debe existir mas de una caja abierta para el mismo usuario/sucursal si la regla lo prohibe,
- toda venta en efectivo debe generar movimiento de caja,
- el cierre debe calcular total esperado,
- el cierre debe registrar diferencia si existe,
- los movimientos deben estar asociados a sucursal y turno.

Archivos esperados:

```text
Caja
  Commands
    AbrirCajaCommand.cs
    CerrarCajaCommand.cs
    RegistrarMovimientoCajaCommand.cs
  Queries
    ObtenerTurnoCajaActualQuery.cs
    ObtenerResumenCajaQuery.cs
  Handlers
    AbrirCajaHandler.cs
    CerrarCajaHandler.cs
    RegistrarMovimientoCajaHandler.cs
    ObtenerTurnoCajaActualHandler.cs
  DTOs
    AperturaCajaDto.cs
    CierreCajaDto.cs
    MovimientoCajaDto.cs
  Validators
    AbrirCajaValidator.cs
    CerrarCajaValidator.cs
  Mappings
    CajaMapper.cs
```

### Fernando - Auth Y Usuarios

Modulo:

```text
PharmaFlow.Application/Auth
PharmaFlow.Application/Usuarios
PharmaFlow.Persistence/Controllers/Auth
PharmaFlow.Persistence/Controllers/Usuarios
PharmaFlow.Infrastructure/Adapters/Services
PharmaFlow.Infrastructure/Adapters/Repositories/Usuarios
```

Debe implementar:

- login,
- refresh token,
- logout,
- registro o administracion de usuarios,
- asignacion de roles,
- asignacion de sucursales,
- hash de password,
- generacion y validacion de JWT.

Logica esperada en auth:

- el password nunca debe guardarse plano,
- el login debe validar credenciales y usuario activo,
- el refresh token debe poder revocarse,
- el usuario debe estar asociado a rol,
- el usuario puede estar asociado a una o mas sucursales.

Archivos esperados:

```text
Auth
  Commands
    LoginCommand.cs
    RefreshTokenCommand.cs
    LogoutCommand.cs
  Queries
    ObtenerPerfilActualQuery.cs
  Handlers
    LoginHandler.cs
    RefreshTokenHandler.cs
    LogoutHandler.cs
    ObtenerPerfilActualHandler.cs
  DTOs
    LoginRequestDto.cs
    AuthResponseDto.cs
  Validators
    LoginValidator.cs

Usuarios
  Commands
    CrearUsuarioCommand.cs
    ActualizarUsuarioCommand.cs
  Queries
    ListarUsuariosQuery.cs
    ObtenerUsuarioPorIdQuery.cs
  Handlers
    CrearUsuarioHandler.cs
    ActualizarUsuarioHandler.cs
    ListarUsuariosHandler.cs
    ObtenerUsuarioPorIdHandler.cs
```

### Eds - Productos Y Clientes

Modulo:

```text
PharmaFlow.Application/Productos
PharmaFlow.Application/Clientes
PharmaFlow.Persistence/Controllers/Productos
PharmaFlow.Persistence/Controllers/Clientes
PharmaFlow.Infrastructure/Adapters/Repositories/Productos
PharmaFlow.Infrastructure/Adapters/Repositories/Clientes
```

Debe implementar:

- registrar productos,
- actualizar productos,
- listar productos,
- buscar producto por codigo de barras o nombre,
- desactivar productos,
- registrar clientes,
- actualizar clientes,
- listar clientes,
- buscar cliente por DNI.

Logica esperada en productos:

- el codigo de barra debe ser unico,
- el producto debe tener nombre,
- el stock minimo debe ser valido,
- un producto inactivo no deberia usarse en nuevas ventas o compras salvo regla contraria.

Logica esperada en clientes:

- el DNI debe ser unico si se registra,
- los datos de contacto son opcionales segun regla de negocio,
- un cliente puede estar asociado a ventas.

Archivos esperados:

```text
Productos
  Commands
    CrearProductoCommand.cs
    ActualizarProductoCommand.cs
    DesactivarProductoCommand.cs
  Queries
    ListarProductosQuery.cs
    ObtenerProductoPorIdQuery.cs
    BuscarProductoPorCodigoQuery.cs
  Handlers
  DTOs
  Validators
  Mappings

Clientes
  Commands
    CrearClienteCommand.cs
    ActualizarClienteCommand.cs
  Queries
    ListarClientesQuery.cs
    ObtenerClientePorDniQuery.cs
  Handlers
  DTOs
  Validators
  Mappings
```

### Ventas

Modulo:

```text
PharmaFlow.Application/Ventas
PharmaFlow.Persistence/Controllers/Ventas
PharmaFlow.Infrastructure/Adapters/Repositories/Ventas
```

Debe implementar:

- registrar venta,
- listar ventas,
- obtener venta por id,
- anular venta,
- calcular total,
- descontar stock,
- generar movimiento de inventario de salida,
- generar movimiento de caja si aplica.

Logica esperada en ventas:

- una venta debe pertenecer a una sucursal,
- debe tener usuario vendedor,
- debe tener uno o mas detalles,
- cada detalle debe tener producto, cantidad y precio,
- debe validar stock disponible,
- debe usar lotes segun FEFO si el flujo lo requiere,
- al completar venta debe afectar inventario,
- al anular venta debe revertir inventario si corresponde.

Archivos esperados:

```text
Ventas
  Commands
    CrearVentaCommand.cs
    AnularVentaCommand.cs
  Queries
    ListarVentasQuery.cs
    ObtenerVentaPorIdQuery.cs
  Handlers
    CrearVentaHandler.cs
    AnularVentaHandler.cs
    ListarVentasHandler.cs
    ObtenerVentaPorIdHandler.cs
  DTOs
    CrearVentaRequestDto.cs
    VentaResponseDto.cs
    DetalleVentaDto.cs
  Validators
    CrearVentaValidator.cs
  Mappings
    VentaMapper.cs
```

### Reportes Y Dashboard

Modulo:

```text
PharmaFlow.Application/Reportes
PharmaFlow.Application/Dashboard
PharmaFlow.Persistence/Controllers/Reportes
PharmaFlow.Persistence/Controllers/Dashboard
PharmaFlow.Infrastructure/Adapters/Repositories/Reportes
```

Debe implementar:

- resumen de ventas,
- ventas por fecha,
- productos mas vendidos,
- stock bajo,
- productos por vencer,
- indicadores principales para dashboard.

Logica esperada:

- reportes solo consultan informacion,
- no deben modificar datos,
- deben usar queries y handlers,
- las consultas pesadas deben estar en Infrastructure como readers o repositories especializados.

Ejemplo actual:

```text
ReportesController
  -> ObtenerResumenVentasQuery
  -> ObtenerResumenVentasHandler
  -> IReporteVentasReader
  -> ReporteVentasReader
  -> PharmaFlowDbContext
```

## Tabla Resumen

| Integrante | Modulo principal | Responsabilidad |
| --- | --- | --- |
| Alex | Compras / Proveedores | compras, proveedores, recepcion de mercaderia |
| Mariel | Inventario | stock, lotes, movimientos de inventario |
| Kevin | Caja | turnos de caja, movimientos, apertura y cierre |
| Fernando | Auth / Usuarios | login, JWT, usuarios, roles, sucursales |
| Eds | Productos / Clientes | catalogo de productos y clientes |
| Equipo | Ventas / Reportes / Dashboard | ventas, indicadores y consultas generales |

## Checklist Para Cada Modulo

Antes de subir cambios, cada integrante debe verificar:

- el modulo esta directamente dentro de `PharmaFlow.Application`, sin carpeta `Features`,
- los controllers estan en `PharmaFlow.Persistence`,
- los repositorios concretos estan en `PharmaFlow.Infrastructure/Adapters/Repositories`,
- las interfaces/ports estan en `PharmaFlow.Domain/Ports` o en Application si son solo consultas especificas,
- no hay logica de negocio dentro del controller,
- el proyecto compila con `dotnet build PharmaFlow.sln`,
- los namespaces coinciden con la nueva estructura.

## Comando Para Validar

```bash
dotnet build PharmaFlow.sln
```
