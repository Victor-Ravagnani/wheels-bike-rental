using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Funcionário da loja. O cargo "Mecânico" é o responsável pelas ordens de
/// manutenção (mudança de requisitos do TP4).
/// </summary>
public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;

    public bool EhMecanico() =>
        Cargo.Equals("Mecânico", StringComparison.OrdinalIgnoreCase) ||
        Cargo.Equals("Mecanico", StringComparison.OrdinalIgnoreCase);

    public string ToCsv() =>
        string.Join(";", Id, Nome.Replace(";", ","), Cargo.Replace(";", ","));

    public static Funcionario FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Funcionario
        {
            Id = int.Parse(c[0], CultureInfo.InvariantCulture),
            Nome = c[1],
            Cargo = c[2]
        };
    }
}
