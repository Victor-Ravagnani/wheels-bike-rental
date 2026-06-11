using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;
using WheelsRental.Services;

namespace WheelsRental.Pages.Reservas;

public class CreateModel : PageModel
{
    private readonly WheelsContext _ctx;
    private readonly ServicoReserva _servico;
    public CreateModel(WheelsContext ctx, ServicoReserva servico)
    {
        _ctx = ctx;
        _servico = servico;
    }

    [BindProperty] public int ClienteId { get; set; }
    [BindProperty] public int BicicletaId { get; set; }
    [BindProperty] public DateTime DataRetirada { get; set; } = DateTime.Now.AddDays(1);

    public List<Cliente> Clientes { get; private set; } = new();
    public List<Bicicleta> Disponiveis { get; private set; } = new();
    public string? Erro { get; private set; }

    public void OnGet() => Carregar();

    public IActionResult OnPost()
    {
        Carregar();
        try
        {
            _servico.RealizarReserva(ClienteId, BicicletaId, DataRetirada);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            Erro = ex.Message;
            return Page();
        }
    }

    private void Carregar()
    {
        Clientes = _ctx.Clientes.Listar();
        Disponiveis = _servico.ConsultarDisponiveis();
    }
}
