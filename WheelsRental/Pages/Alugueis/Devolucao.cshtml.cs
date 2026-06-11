using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;
using WheelsRental.Services;

namespace WheelsRental.Pages.Alugueis;

public class DevolucaoModel : PageModel
{
    private readonly WheelsContext _ctx;
    private readonly ServicoAluguel _servico;
    public DevolucaoModel(WheelsContext ctx, ServicoAluguel servico)
    {
        _ctx = ctx;
        _servico = servico;
    }

    [BindProperty(SupportsGet = true)] public int AluguelId { get; set; }
    [BindProperty] public DateTime DataDevolucao { get; set; } = DateTime.Now;
    [BindProperty] public FormaPagamento Forma { get; set; }
    [BindProperty] public decimal? ValorPago { get; set; }

    public string? ReciboTexto { get; private set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        var aluguel = _servico.RegistrarDevolucao(AluguelId, DataDevolucao);
        decimal valor = ValorPago ?? aluguel.ValorTotal;
        var recibo = _servico.RegistrarPagamento(AluguelId, valor, Forma);

        var cliente = _ctx.Clientes.Listar().First(c => c.Id == aluguel.ClienteId);
        var pagamento = _ctx.Pagamentos.Listar().First(p => p.Id == recibo.PagamentoId);
        ReciboTexto = recibo.GerarTexto(cliente, aluguel, pagamento);
        return Page();
    }
}
