using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages.Bicicletas;

public class CreateModel : PageModel
{
    private readonly WheelsContext _ctx;
    public CreateModel(WheelsContext ctx) => _ctx = ctx;

    [BindProperty] public Bicicleta Entrada { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        Entrada.Id = WheelsContext.ProximoId(_ctx.Bicicletas.Listar(), b => b.Id);
        Entrada.Estado = EstadoBicicleta.Disponivel;
        _ctx.Bicicletas.Adicionar(Entrada);
        return RedirectToPage("Index");
    }
}
