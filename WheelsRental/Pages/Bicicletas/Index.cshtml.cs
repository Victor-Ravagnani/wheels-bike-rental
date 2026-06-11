using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages.Bicicletas;

public class IndexModel : PageModel
{
    private readonly WheelsContext _ctx;
    public IndexModel(WheelsContext ctx) => _ctx = ctx;

    public List<Bicicleta> Bicicletas { get; private set; } = new();

    public void OnGet() => Bicicletas = _ctx.Bicicletas.Listar();
}
