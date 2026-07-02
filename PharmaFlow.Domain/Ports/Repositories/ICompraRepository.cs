using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface ICompraRepository
{
    Task AgregarAsync(Compra compra, CancellationToken cancellationToken);

    Task<IReadOnlyList<Compra>> ListarAsync(Guid? idSucursal, DateTime? desde, DateTime? hasta, CancellationToken cancellationToken);

    Task<Compra?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Compra> RecepcionarAsync(
        Guid idCompra,
        Guid? idUsuario,
        IReadOnlyList<RecepcionCompraDetalle> detalles,
        CancellationToken cancellationToken);
}

public class RecepcionCompraDetalle
{
    public Guid IdDetalleCompra { get; set; }

    public string? NumeroLote { get; set; }

    public DateOnly? FechaVencimiento { get; set; }
}
