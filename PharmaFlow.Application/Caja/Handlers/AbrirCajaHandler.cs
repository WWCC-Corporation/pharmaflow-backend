using PharmaFlow.Application.Caja.Commands;
using PharmaFlow.Application.Caja.DTOs;
using PharmaFlow.Application.Caja.Mappings;
using PharmaFlow.Application.Caja.Validators;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Caja.Handlers;

public class AbrirCajaHandler
{
    private readonly ICajaRepository _cajaRepository;

    public AbrirCajaHandler(ICajaRepository cajaRepository)
    {
        _cajaRepository = cajaRepository;
    }

    public async Task<AperturaCajaDto> Handle(AbrirCajaCommand command)
    {
        AbrirCajaValidator.Validate(command);

        var turnoExistente = await _cajaRepository.GetTurnoAbiertoPorUsuarioAsync(command.IdUsuario);
        if (turnoExistente != null)
            throw new InvalidOperationException("El usuario ya tiene una caja abierta. Debe cerrarla antes de abrir una nueva.");

        var nuevoTurno = new TurnosCaja
        {
            Id = Guid.NewGuid(),
            IdUsuario = command.IdUsuario,
            MontoApertura = command.MontoApertura,
            Abierto = true,
            CreatedAt = DateTime.UtcNow
        };

        await _cajaRepository.AddTurnoCajaAsync(nuevoTurno);

        return CajaMapper.ToAperturaDto(nuevoTurno);
    }
}
