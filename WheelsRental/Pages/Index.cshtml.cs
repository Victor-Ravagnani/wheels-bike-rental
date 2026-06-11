using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages;

public class IndexModel : PageModel
{
    private readonly WheelsContext _ctx;
    public IndexModel(WheelsContext ctx) => _ctx = ctx;

    public int TotalBicicletas { get; private set; }
    public int Disponiveis { get; private set; }
    public int TotalClientes { get; private set; }
    public int AlugueisAbertos { get; private set; }

    public void OnGet()
    {
        var bikes = _ctx.Bicicletas.Listar();
        TotalBicicletas = bikes.Count;
        Disponiveis = bikes.Count(b => b.EstaDisponivel());
        TotalClientes = _ctx.Clientes.Listar().Count;
        AlugueisAbertos = _ctx.Alugueis.Listar().Count(a => a.Status == StatusAluguel.Aberto);
    }
}
