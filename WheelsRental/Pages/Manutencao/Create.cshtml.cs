using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;
using WheelsRental.Services;

namespace WheelsRental.Pages.Manutencao;

public class CreateModel : PageModel
{
    private readonly WheelsContext _ctx;
    private readonly ServicoManutencao _servico;
    public CreateModel(WheelsContext ctx, ServicoManutencao servico)
    {
        _ctx = ctx;
        _servico = servico;
    }

    [BindProperty] public int BicicletaId { get; set; }
    [BindProperty] public int MecanicoId { get; set; }
    [BindProperty] public string Descricao { get; set; } = string.Empty;

    public List<Bicicleta> Bicicletas { get; private set; } = new();
    public List<Funcionario> Mecanicos { get; private set; } = new();
    public string? Erro { get; private set; }

    public void OnGet() => Carregar();

    public IActionResult OnPost()
    {
        Carregar();
        try
        {
            _servico.RegistrarOrdem(BicicletaId, MecanicoId, Descricao);
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
        Bicicletas = _ctx.Bicicletas.Listar();
        Mecanicos = _ctx.Funcionarios.Listar().Where(f => f.EhMecanico()).ToList();
    }
}
