using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Services;

/// <summary>
/// Regras de negócio de manutenção, executadas pelo mecânico. Introduzidas na
/// mudança de requisitos do TP4 (controle de manutenção passou ao escopo).
/// «include» Atualizar Estado da Bicicleta.
/// </summary>
public class ServicoManutencao
{
    private readonly WheelsContext _ctx;

    public ServicoManutencao(WheelsContext ctx) => _ctx = ctx;

    /// <summary>Abre uma ordem de manutenção e coloca a bicicleta em manutenção.</summary>
    public OrdemManutencao RegistrarOrdem(int bicicletaId, int mecanicoId, string descricao)
    {
        var bicicletas = _ctx.Bicicletas.Listar();
        var bike = bicicletas.First(b => b.Id == bicicletaId);

        var ordem = new OrdemManutencao
        {
            Id = WheelsContext.ProximoId(_ctx.OrdensManutencao.Listar(), o => o.Id),
            BicicletaId = bicicletaId,
            MecanicoId = mecanicoId,
            Descricao = descricao
        };
        ordem.Abrir();
        bike.AlterarEstado(EstadoBicicleta.Manutencao);       // «include» Atualizar Estado

        _ctx.OrdensManutencao.Adicionar(ordem);
        _ctx.Bicicletas.SalvarTodos(bicicletas);
        return ordem;
    }

    /// <summary>Conclui a ordem e devolve a bicicleta ao estado disponível.</summary>
    public void ConcluirOrdem(int ordemId)
    {
        var ordens = _ctx.OrdensManutencao.Listar();
        var ordem = ordens.First(o => o.Id == ordemId);
        ordem.Fechar();

        var bicicletas = _ctx.Bicicletas.Listar();
        var bike = bicicletas.First(b => b.Id == ordem.BicicletaId);
        bike.AlterarEstado(EstadoBicicleta.Disponivel);

        _ctx.OrdensManutencao.SalvarTodos(ordens);
        _ctx.Bicicletas.SalvarTodos(bicicletas);
    }
}
