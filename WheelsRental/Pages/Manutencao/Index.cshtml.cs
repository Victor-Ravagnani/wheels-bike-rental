using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WheelsRental.Data;
using WheelsRental.Models;
using WheelsRental.Services;

namespace WheelsRental.Pages.Manutencao;

public class IndexModel : PageModel
{
    private readonly WheelsContext _ctx;
    private readonly ServicoManutencao _servico;
    public IndexModel(WheelsContext ctx, ServicoManutencao servico)
    {
        _ctx = ctx;
        _servico = servico;
    }

    public List<OrdemManutencao> Ordens { get; private set; } = new();
    private List<Bicicleta> _bikes = new();
    private List<Funcionario> _funcs = new();

    public void OnGet() => Carregar();

    public IActionResult OnPostConcluir(int id)
    {
        _servico.ConcluirOrdem(id);
        return RedirectToPage("Index");
    }

    private void Carregar()
    {
        Ordens = _ctx.OrdensManutencao.Listar().OrderByDescending(o => o.Id).ToList();
        _bikes = _ctx.Bicicletas.Listar();
        _funcs = _ctx.Funcionarios.Listar();
    }

    public string NomeBike(int id) => _bikes.FirstOrDefault(b => b.Id == id)?.Modelo ?? $"#{id}";
    public string NomeMecanico(int id) => _funcs.FirstOrDefault(f => f.Id == id)?.Nome ?? $"#{id}";
}
