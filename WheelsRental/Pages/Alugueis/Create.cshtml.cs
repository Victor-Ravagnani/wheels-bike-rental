using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;
using WheelsRental.Services;

namespace WheelsRental.Pages.Alugueis;

public class CreateModel : PageModel
{
    private readonly WheelsContext _ctx;
    private readonly ServicoAluguel _servico;
    public CreateModel(WheelsContext ctx, ServicoAluguel servico)
    {
        _ctx = ctx;
        _servico = servico;
    }

    [BindProperty] public int ClienteId { get; set; }
    [BindProperty] public int FuncionarioId { get; set; }
    [BindProperty] public List<int> BicicletaIds { get; set; } = new();
    [BindProperty] public DateTime DataPrevista { get; set; } = DateTime.Now.AddHours(2);

    public List<Cliente> Clientes { get; private set; } = new();
    public List<Funcionario> Funcionarios { get; private set; } = new();
    public List<Bicicleta> Disponiveis { get; private set; } = new();
    public string? Erro { get; private set; }

    public void OnGet() => Carregar();

    public IActionResult OnPost()
    {
        Carregar();
        if (BicicletaIds.Count == 0)
        {
            Erro = "Selecione ao menos uma bicicleta.";
            return Page();
        }
        try
        {
            _servico.RegistrarAluguel(ClienteId, FuncionarioId, BicicletaIds, DataPrevista);
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
        Funcionarios = _ctx.Funcionarios.Listar();
        Disponiveis = _ctx.Bicicletas.Listar().Where(b => b.EstaDisponivel()).ToList();
    }
}
