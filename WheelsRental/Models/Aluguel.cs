using System.Globalization;

namespace WheelsRental.Models;

/// <summary>
/// Aluguel de uma ou mais bicicletas por um cliente. Concentra as principais
/// regras de negócio: cálculo do valor (por duração e tipo), multa por atraso
/// e desconto de fidelidade.
/// </summary>
public class Aluguel
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int FuncionarioId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataPrevistaDevolucao { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public StatusAluguel Status { get; set; } = StatusAluguel.Aberto;
    public decimal ValorTotal { get; set; }
    public decimal Multa { get; set; }

    /// <summary>Itens do aluguel (uma linha por bicicleta).</summary>
    public List<ItemAluguel> Itens { get; set; } = new();

    /// <summary>Adiciona uma bicicleta ao aluguel.</summary>
    public void AdicionarItem(Bicicleta bicicleta)
    {
        Itens.Add(new ItemAluguel
        {
            AluguelId = Id,
            BicicletaId = bicicleta.Id,
            Bicicleta = bicicleta,
            ValorUnitario = 0m
        });
    }

    /// <summary>
    /// «include» Calcular Valor do Aluguel: soma o valor de cada bicicleta
    /// (valorHora × horas de uso) e aplica o desconto de fidelidade do cliente.
    /// </summary>
    public decimal CalcularValor(decimal descontoFidelidade = 0m)
    {
        var fim = DataDevolucao ?? DataPrevistaDevolucao;
        int horas = Math.Max(1, (int)Math.Ceiling((fim - DataInicio).TotalHours));

        decimal bruto = 0m;
        foreach (var item in Itens)
        {
            decimal valorHora = item.Bicicleta?.ValorHora ?? 0m;
            item.ValorUnitario = valorHora * horas;
            bruto += item.ValorUnitario;
        }
        ValorTotal = Math.Round(bruto * (1 - descontoFidelidade), 2) + Multa;
        return ValorTotal;
    }

    /// <summary>
    /// «extend» Calcular Multa por Atraso: aplicada somente quando a devolução
    /// ocorre após a data prevista. Cobra 50% a mais sobre as horas em atraso.
    /// </summary>
    public decimal CalcularMulta()
    {
        if (DataDevolucao is null || DataDevolucao <= DataPrevistaDevolucao)
        {
            Multa = 0m;
            return 0m;
        }
        int horasAtraso = (int)Math.Ceiling((DataDevolucao.Value - DataPrevistaDevolucao).TotalHours);
        decimal valorHoraTotal = Itens.Sum(i => i.Bicicleta?.ValorHora ?? 0m);
        Multa = Math.Round(horasAtraso * valorHoraTotal * 1.5m, 2);
        return Multa;
    }

    /// <summary>Finaliza o aluguel registrando a data de devolução.</summary>
    public void Finalizar(DateTime dataDevolucao)
    {
        DataDevolucao = dataDevolucao;
        Status = StatusAluguel.Finalizado;
    }

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() => string.Join(";",
        Id, ClienteId, FuncionarioId,
        DataInicio.ToString("o", Ci),
        DataPrevistaDevolucao.ToString("o", Ci),
        DataDevolucao?.ToString("o", Ci) ?? "",
        (int)Status, ValorTotal.ToString(Ci), Multa.ToString(Ci));

    public static Aluguel FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Aluguel
        {
            Id = int.Parse(c[0], Ci),
            ClienteId = int.Parse(c[1], Ci),
            FuncionarioId = int.Parse(c[2], Ci),
            DataInicio = DateTime.Parse(c[3], Ci),
            DataPrevistaDevolucao = DateTime.Parse(c[4], Ci),
            DataDevolucao = string.IsNullOrWhiteSpace(c[5]) ? null : DateTime.Parse(c[5], Ci),
            Status = (StatusAluguel)int.Parse(c[6], Ci),
            ValorTotal = decimal.Parse(c[7], Ci),
            Multa = decimal.Parse(c[8], Ci)
        };
    }
}
