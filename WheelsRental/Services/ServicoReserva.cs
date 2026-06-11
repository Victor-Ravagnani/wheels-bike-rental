using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Services;

/// <summary>
/// Regras de negócio de reserva (autoatendimento do cliente) introduzidas na
/// mudança de requisitos do TP4. Implementa o fluxo do diagrama de sequência
/// "Realizar Reserva".
/// </summary>
public class ServicoReserva
{
    private readonly WheelsContext _ctx;

    public ServicoReserva(WheelsContext ctx) => _ctx = ctx;

    /// <summary>«include» Consultar Bicicletas Disponíveis.</summary>
    public List<Bicicleta> ConsultarDisponiveis()
        => _ctx.Bicicletas.Listar().Where(b => b.EstaDisponivel()).ToList();

    /// <summary>Realiza a reserva de uma bicicleta disponível para um cliente.</summary>
    public Reserva RealizarReserva(int clienteId, int bicicletaId, DateTime dataRetirada)
    {
        var bicicletas = _ctx.Bicicletas.Listar();
        var bike = bicicletas.First(b => b.Id == bicicletaId);
        if (!bike.EstaDisponivel())
            throw new InvalidOperationException("Bicicleta não está disponível para reserva.");

        var reserva = new Reserva
        {
            Id = WheelsContext.ProximoId(_ctx.Reservas.Listar(), r => r.Id),
            ClienteId = clienteId,
            BicicletaId = bicicletaId,
            DataRetiradaPrevista = dataRetirada,
            Status = StatusReserva.Confirmada
        };
        bike.AlterarEstado(EstadoBicicleta.Reservada);

        _ctx.Reservas.Adicionar(reserva);
        _ctx.Bicicletas.SalvarTodos(bicicletas);
        return reserva;
    }

    public void Cancelar(int reservaId)
    {
        var reservas = _ctx.Reservas.Listar();
        var reserva = reservas.First(r => r.Id == reservaId);
        reserva.Cancelar();

        var bicicletas = _ctx.Bicicletas.Listar();
        var bike = bicicletas.First(b => b.Id == reserva.BicicletaId);
        if (bike.Estado == EstadoBicicleta.Reservada)
            bike.AlterarEstado(EstadoBicicleta.Disponivel);

        _ctx.Reservas.SalvarTodos(reservas);
        _ctx.Bicicletas.SalvarTodos(bicicletas);
    }
}
