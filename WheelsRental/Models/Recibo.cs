using System.Globalization;
using System.Text;

namespace WheelsRental.Models;

/// <summary>
/// Recibo emitido para o cliente após o pagamento de um aluguel.
/// «include» de Emitir Recibo a partir de Registrar Pagamento.
/// </summary>
public class Recibo
{
    public int Id { get; set; }
    public int AluguelId { get; set; }
    public int PagamentoId { get; set; }
    public DateTime DataEmissao { get; set; } = DateTime.Now;

    /// <summary>Gera o texto do recibo a partir do aluguel, cliente e pagamento.</summary>
    public string GerarTexto(Cliente cliente, Aluguel aluguel, Pagamento pagamento)
    {
        var sb = new StringBuilder();
        sb.AppendLine("====== LOJA WHEELS - RECIBO DE ALUGUEL ======");
        sb.AppendLine($"Recibo nº: {Id}");
        sb.AppendLine($"Data de emissão: {DataEmissao:dd/MM/yyyy HH:mm}");
        sb.AppendLine($"Cliente: {cliente.Nome} (CPF {cliente.Cpf})");
        sb.AppendLine($"Aluguel nº: {aluguel.Id}");
        sb.AppendLine($"Início: {aluguel.DataInicio:dd/MM/yyyy HH:mm}");
        sb.AppendLine($"Devolução: {aluguel.DataDevolucao:dd/MM/yyyy HH:mm}");
        sb.AppendLine($"Bicicletas alugadas: {aluguel.Itens.Count}");
        sb.AppendLine($"Multa por atraso: R$ {aluguel.Multa.ToString("F2", CultureInfo.GetCultureInfo("pt-BR"))}");
        sb.AppendLine($"Valor total: R$ {aluguel.ValorTotal.ToString("F2", CultureInfo.GetCultureInfo("pt-BR"))}");
        sb.AppendLine($"Valor pago: R$ {pagamento.ValorPago.ToString("F2", CultureInfo.GetCultureInfo("pt-BR"))}");
        sb.AppendLine($"Valor pendente: R$ {pagamento.ValorPendente.ToString("F2", CultureInfo.GetCultureInfo("pt-BR"))}");
        sb.AppendLine("=============================================");
        return sb.ToString();
    }

    private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

    public string ToCsv() =>
        string.Join(";", Id, AluguelId, PagamentoId, DataEmissao.ToString("o", Ci));

    public static Recibo FromCsv(string linha)
    {
        var c = linha.Split(';');
        return new Recibo
        {
            Id = int.Parse(c[0], Ci),
            AluguelId = int.Parse(c[1], Ci),
            PagamentoId = int.Parse(c[2], Ci),
            DataEmissao = DateTime.Parse(c[3], Ci)
        };
    }
}
