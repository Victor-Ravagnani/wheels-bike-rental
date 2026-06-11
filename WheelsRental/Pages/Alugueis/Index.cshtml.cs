using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages.Alugueis;

public class IndexModel : PageModel
{
    private readonly WheelsContext _ctx;
    public IndexModel(WheelsContext ctx) => _ctx = ctx;

    public List<Aluguel> Alugueis { get; private set; } = new();
    private List<Cliente> _clientes = new();

    public void OnGet()
    {
        Alugueis = _ctx.Alugueis.Listar().OrderByDescending(a => a.Id).ToList();
        _clientes = _ctx.Clientes.Listar();
    }

    public string NomeCliente(int id) =>
        _clientes.FirstOrDefault(c => c.Id == id)?.Nome ?? $"#{id}";
}
