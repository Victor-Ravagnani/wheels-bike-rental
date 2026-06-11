using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Pagamento associado a um aluguel. Registra valores pagos e pendentes e a
/// forma de pagamento utilizada.
/// </summary>
public class Pagamento
{
    public int Id { get; set; }
    public int AluguelId { get; set; }
    public decimal ValorPago { get; set; }
    public decimal ValorPendente { get; set; }
    public DateTime DataPagamento { get; set; }
    public FormaPagamento Forma { get; set; }

    /// <summary>Registra o pagamento, calculando o valor pendente.</summary>
    public void Registrar(decimal valorTotal, decimal valorPago, FormaPagamento forma)
    {
        ValorPago = valorPago;
        ValorPendente = Math.Max(0m, valorTotal - valorPago);
        Forma = forma;
        DataPagamento = DateTime.Now;
    }

    /// <summary>Indica se o aluguel foi totalmente quitado.</summary>
    public bool EstaQuitado() => ValorPendente <= 0m;

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() => string.Join(";",
        Id, AluguelId, ValorPago.ToString(Ci), ValorPendente.ToString(Ci),
        DataPagamento.ToString("o", Ci), (int)Forma);

    public static Pagamento FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Pagamento
        {
            Id = int.Parse(c[0], Ci),
            AluguelId = int.Parse(c[1], Ci),
            ValorPago = decimal.Parse(c[2], Ci),
            ValorPendente = decimal.Parse(c[3], Ci),
            DataPagamento = DateTime.Parse(c[4], Ci),
            Forma = (FormaPagamento)int.Parse(c[5], Ci)
        };
    }
}
