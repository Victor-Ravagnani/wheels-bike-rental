using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Reserva de uma bicicleta feita pelo próprio cliente (autoatendimento).
/// Classe criada na mudança de requisitos do TP4.
/// </summary>
public class Reserva
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int BicicletaId { get; set; }
    public DateTime DataReserva { get; set; } = DateTime.Now;
    public DateTime DataRetiradaPrevista { get; set; }
    public StatusReserva Status { get; set; } = StatusReserva.Pendente;

    public void Confirmar() => Status = StatusReserva.Confirmada;
    public void Cancelar() => Status = StatusReserva.Cancelada;

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() => string.Join(";",
        Id, ClienteId, BicicletaId,
        DataReserva.ToString("o", Ci),
        DataRetiradaPrevista.ToString("o", Ci),
        (int)Status);

    public static Reserva FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Reserva
        {
            Id = int.Parse(c[0], Ci),
            ClienteId = int.Parse(c[1], Ci),
            BicicletaId = int.Parse(c[2], Ci),
            DataReserva = DateTime.Parse(c[3], Ci),
            DataRetiradaPrevista = DateTime.Parse(c[4], Ci),
            Status = (StatusReserva)int.Parse(c[5], Ci)
        };
    }
}
