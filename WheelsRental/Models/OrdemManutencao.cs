using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Ordem de manutenção de uma bicicleta, registrada pelo mecânico.
/// Classe criada na mudança de requisitos do TP4 (controle de manutenção
/// passou a fazer parte do escopo).
/// </summary>
public class OrdemManutencao
{
    public int Id { get; set; }
    public int BicicletaId { get; set; }
    public int MecanicoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; } = DateTime.Now;
    public DateTime? DataFechamento { get; set; }
    public StatusManutencao Status { get; set; } = StatusManutencao.Aberta;

    public void Abrir()
    {
        Status = StatusManutencao.Aberta;
        DataAbertura = DateTime.Now;
    }

    public void Fechar()
    {
        Status = StatusManutencao.Concluida;
        DataFechamento = DateTime.Now;
    }

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() => string.Join(";",
        Id, BicicletaId, MecanicoId, Descricao.Replace(";", ","),
        DataAbertura.ToString("o", Ci),
        DataFechamento?.ToString("o", Ci) ?? "",
        (int)Status);

    public static OrdemManutencao FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new OrdemManutencao
        {
            Id = int.Parse(c[0], Ci),
            BicicletaId = int.Parse(c[1], Ci),
            MecanicoId = int.Parse(c[2], Ci),
            Descricao = c[3],
            DataAbertura = DateTime.Parse(c[4], Ci),
            DataFechamento = string.IsNullOrWhiteSpace(c[5]) ? null : DateTime.Parse(c[5], Ci),
            Status = (StatusManutencao)int.Parse(c[6], Ci)
        };
    }
}
