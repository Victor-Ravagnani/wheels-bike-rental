using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages.Clientes;

public class IndexModel : PageModel
{
    private readonly WheelsContext _ctx;
    public IndexModel(WheelsContext ctx) => _ctx = ctx;

    public List<Cliente> Clientes { get; private set; } = new();

    public void OnGet() => Clientes = _ctx.Clientes.Listar();
}
