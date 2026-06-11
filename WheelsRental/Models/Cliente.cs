using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Representa um cliente da loja Wheels. Um cliente pode realizar vários
/// aluguéis e reservas e acumula pontos no programa de fidelidade (TP4).
/// </summary>
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int PontosFidelidade { get; set; }

    /// <summary>Valida o CPF de forma simples (11 dígitos numéricos).</summary>
    public bool ValidarCpf()
    {
        var digitos = new string(Cpf.Where(char.IsDigit).ToArray());
        return digitos.Length == 11;
    }

    /// <summary>Adiciona pontos de fidelidade ao cliente.</summary>
    public void AdicionarPontos(int quantidade)
    {
        if (quantidade > 0) PontosFidelidade += quantidade;
    }

    /// <summary>
    /// Retorna o percentual de desconto de fidelidade (0 a 0,15) conforme
    /// a faixa de pontos acumulados. Regra criada na mudança de requisitos (TP4).
    /// </summary>
    public decimal ObterDesconto()
    {
        if (PontosFidelidade >= 500) return 0.15m;
        if (PontosFidelidade >= 200) return 0.10m;
        if (PontosFidelidade >= 100) return 0.05m;
        return 0m;
    }

    // ---- Persistência CSV ----
    public string ToCsv() =>
        string.Join(";", Id, Escape(Nome), Cpf, Telefone, Email, PontosFidelidade);

    public static Cliente FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Cliente
        {
            Id = int.Parse(c[0], CultureInfo.InvariantCulture),
            Nome = c[1],
            Cpf = c[2],
            Telefone = c[3],
            Email = c[4],
            PontosFidelidade = int.Parse(c[5], CultureInfo.InvariantCulture)
        };
    }

    private static string Escape(string s) => s.Replace(";", ",");
}
