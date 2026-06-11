using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Representa uma bicicleta do acervo da loja Wheels. O valor por hora varia
/// conforme o tipo e o estado controla a disponibilidade para aluguel.
/// </summary>
public class Bicicleta
{
    public int Id { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public TipoBicicleta Tipo { get; set; }
    public EstadoBicicleta Estado { get; set; } = EstadoBicicleta.Disponivel;
    public decimal ValorHora { get; set; }
    public decimal ValorDeposito { get; set; }
    public bool Especial { get; set; }
    public string Restricoes { get; set; } = string.Empty;

    /// <summary>Indica se a bicicleta pode ser alugada no momento.</summary>
    public bool EstaDisponivel() => Estado == EstadoBicicleta.Disponivel;

    /// <summary>Altera o estado da bicicleta (disponível, alugada, manutenção...).</summary>
    public void AlterarEstado(EstadoBicicleta novoEstado) => Estado = novoEstado;

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() => string.Join(";",
        Id, Modelo.Replace(";", ","), (int)Tipo, (int)Estado,
        ValorHora.ToString(Ci), ValorDeposito.ToString(Ci),
        Especial, Restricoes.Replace(";", ","));

    public static Bicicleta FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Bicicleta
        {
            Id = int.Parse(c[0], Ci),
            Modelo = c[1],
            Tipo = (TipoBicicleta)int.Parse(c[2], Ci),
            Estado = (EstadoBicicleta)int.Parse(c[3], Ci),
            ValorHora = decimal.Parse(c[4], Ci),
            ValorDeposito = decimal.Parse(c[5], Ci),
            Especial = bool.Parse(c[6]),
            Restricoes = c.Length > 7 ? c[7] : string.Empty
        };
    }
}
