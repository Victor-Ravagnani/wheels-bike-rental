using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Item de um aluguel: associa uma bicicleta a um aluguel e guarda o valor
/// cobrado por ela. Um aluguel possui de 1 a N itens (cliente pode alugar
/// mais de uma bicicleta de uma vez).
/// </summary>
public class ItemAluguel
{
    public int AluguelId { get; set; }
    public int BicicletaId { get; set; }
    public decimal ValorUnitario { get; set; }

    public Bicicleta? Bicicleta { get; set; }

    /// <summary>Valor cobrado por este item do aluguel.</summary>
    public decimal Subtotal() => ValorUnitario;

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() =>
        string.Join(";", AluguelId, BicicletaId, ValorUnitario.ToString(Ci));

    public static ItemAluguel FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new ItemAluguel
        {
            AluguelId = int.Parse(c[0], Ci),
            BicicletaId = int.Parse(c[1], Ci),
            ValorUnitario = decimal.Parse(c[2], Ci)
        };
    }
}
