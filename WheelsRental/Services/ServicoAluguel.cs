using WheelsRental.Data;
using WheelsRental.Models;

namespace WheelsRental.Services;

/// <summary>
/// Coordena as regras de negócio do aluguel: registro, cálculo de valor,
/// devolução (com multa), pagamento e emissão de recibo. Implementa os
/// fluxos descritos nos diagramas de sequência do TP4.
/// </summary>
public class ServicoAluguel
{
    private readonly WheelsContext _ctx;

    public ServicoAluguel(WheelsContext ctx) => _ctx = ctx;

    /// <summary>
    /// Registra um novo aluguel para um cliente com uma ou mais bicicletas.
    /// «include» Calcular Valor do Aluguel; «extend» Aplicar Desconto Fidelidade.
    /// </summary>
    public Aluguel RegistrarAluguel(int clienteId, int funcionarioId,
                                    IEnumerable<int> bicicletaIds, DateTime dataPrevista)
    {
        var cliente = _ctx.Clientes.Listar().FirstOrDefault(c => c.Id == clienteId)
                      ?? throw new InvalidOperationException("Cliente não encontrado.");
        var bicicletas = _ctx.Bicicletas.Listar();

        var aluguel = new Aluguel
        {
            Id = WheelsContext.ProximoId(_ctx.Alugueis.Listar(), a => a.Id),
            ClienteId = clienteId,
            FuncionarioId = funcionarioId,
            DataInicio = DateTime.Now,
            DataPrevistaDevolucao = dataPrevista,
            Status = StatusAluguel.Aberto
        };

        foreach (var id in bicicletaIds)
        {
            var bike = bicicletas.First(b => b.Id == id);
            if (!bike.EstaDisponivel())
                throw new InvalidOperationException($"Bicicleta {bike.Modelo} indisponível.");
            aluguel.AdicionarItem(bike);
            bike.AlterarEstado(EstadoBicicleta.Alugada);
        }

        aluguel.CalcularValor(cliente.ObterDesconto());      // «include»

        _ctx.Alugueis.Adicionar(aluguel);
        foreach (var item in aluguel.Itens) _ctx.Itens.Adicionar(item);
        _ctx.Bicicletas.SalvarTodos(bicicletas);             // estados atualizados
        return aluguel;
    }

    /// <summary>
    /// Registra a devolução: finaliza o aluguel, calcula multa por atraso
    /// «extend», recalcula o valor e libera as bicicletas.
    /// </summary>
    public Aluguel RegistrarDevolucao(int aluguelId, DateTime dataDevolucao)
    {
        var aluguel = Hidratar(aluguelId);
        var cliente = _ctx.Clientes.Listar().First(c => c.Id == aluguel.ClienteId);

        aluguel.Finalizar(dataDevolucao);
        aluguel.CalcularMulta();                              // «extend»
        aluguel.CalcularValor(cliente.ObterDesconto());       // «include»

        var bicicletas = _ctx.Bicicletas.Listar();
        foreach (var item in aluguel.Itens)
        {
            var bike = bicicletas.First(b => b.Id == item.BicicletaId);
            bike.AlterarEstado(EstadoBicicleta.Disponivel);
        }

        PersistirAluguel(aluguel);
        _ctx.Bicicletas.SalvarTodos(bicicletas);
        return aluguel;
    }

    /// <summary>
    /// Registra o pagamento do aluguel, acumula pontos de fidelidade e
    /// «include» Emitir Recibo.
    /// </summary>
    public Recibo RegistrarPagamento(int aluguelId, decimal valorPago, FormaPagamento forma)
    {
        var aluguel = Hidratar(aluguelId);
        var pagamento = new Pagamento
        {
            Id = WheelsContext.ProximoId(_ctx.Pagamentos.Listar(), p => p.Id),
            AluguelId = aluguelId
        };
        pagamento.Registrar(aluguel.ValorTotal, valorPago, forma);
        _ctx.Pagamentos.Adicionar(pagamento);

        // Programa de fidelidade: 1 ponto a cada R$ 10 pagos (mudança TP4).
        var clientes = _ctx.Clientes.Listar();
        var cliente = clientes.First(c => c.Id == aluguel.ClienteId);
        cliente.AdicionarPontos((int)(valorPago / 10m));
        _ctx.Clientes.SalvarTodos(clientes);

        var recibo = new Recibo
        {
            Id = WheelsContext.ProximoId(_ctx.Recibos.Listar(), r => r.Id),
            AluguelId = aluguelId,
            PagamentoId = pagamento.Id
        };
        _ctx.Recibos.Adicionar(recibo);                       // «include» Emitir Recibo
        return recibo;
    }

    /// <summary>Reconstrói um aluguel com seus itens e respectivas bicicletas.</summary>
    public Aluguel Hidratar(int aluguelId)
    {
        var aluguel = _ctx.Alugueis.Listar().First(a => a.Id == aluguelId);
        var bicicletas = _ctx.Bicicletas.Listar();
        aluguel.Itens = _ctx.Itens.Listar()
            .Where(i => i.AluguelId == aluguelId)
            .Select(i => { i.Bicicleta = bicicletas.First(b => b.Id == i.BicicletaId); return i; })
            .ToList();
        return aluguel;
    }

    private void PersistirAluguel(Aluguel aluguel)
    {
        var todos = _ctx.Alugueis.Listar();
        var idx = todos.FindIndex(a => a.Id == aluguel.Id);
        if (idx >= 0) todos[idx] = aluguel;
        _ctx.Alugueis.SalvarTodos(todos);

        // Atualiza os valores unitários dos itens deste aluguel.
        var itens = _ctx.Itens.Listar().Where(i => i.AluguelId != aluguel.Id).ToList();
        itens.AddRange(aluguel.Itens);
        _ctx.Itens.SalvarTodos(itens);
    }
}
