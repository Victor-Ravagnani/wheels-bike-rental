using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages.Reservas;

public class IndexModel : PageModel
{
    private readonly WheelsContext _ctx;
    public IndexModel(WheelsContext ctx) => _ctx = ctx;

    public List<Reserva> Reservas { get; private set; } = new();
    private List<Cliente> _clientes = new();
    private List<Bicicleta> _bikes = new();

    public void OnGet()
    {
        Reservas = _ctx.Reservas.Listar().OrderByDescending(r => r.Id).ToList();
        _clientes = _ctx.Clientes.Listar();
        _bikes = _ctx.Bicicletas.Listar();
    }

    public string NomeCliente(int id) => _clientes.FirstOrDefault(c => c.Id == id)?.Nome ?? $"#{id}";
    public string NomeBike(int id) => _bikes.FirstOrDefault(b => b.Id == id)?.Modelo ?? $"#{id}";
}
