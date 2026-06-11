using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Pages.Clientes;

public class CreateModel : PageModel
{
    private readonly WheelsContext _ctx;
    public CreateModel(WheelsContext ctx) => _ctx = ctx;

    [BindProperty] public Cliente Entrada { get; set; } = new();
    public string? Erro { get; private set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!Entrada.ValidarCpf())
        {
            Erro = "CPF inválido: informe 11 dígitos.";
            return Page();
        }
        Entrada.Id = WheelsContext.ProximoId(_ctx.Clientes.Listar(), c => c.Id);
        _ctx.Clientes.Adicionar(Entrada);
        return RedirectToPage("Index");
    }
}
