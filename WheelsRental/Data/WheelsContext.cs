using WheelsRental.Models;

namespace WheelsRental.Data;

/// <summary>
/// Agrega os repositórios CSV de todas as entidades do sistema e centraliza
/// a geração de identificadores. Funciona como uma "fonte de dados" única,
/// registrada como singleton na injeção de dependência (ver Program.cs).
/// </summary>
public class WheelsContext
{
    public CsvRepository<Cliente> Clientes { get; }
    public CsvRepository<Funcionario> Funcionarios { get; }
    public CsvRepository<Bicicleta> Bicicletas { get; }
    public CsvRepository<Aluguel> Alugueis { get; }
    public CsvRepository<ItemAluguel> Itens { get; }
    public CsvRepository<Pagamento> Pagamentos { get; }
    public CsvRepository<Recibo> Recibos { get; }
    public CsvRepository<Reserva> Reservas { get; }
    public CsvRepository<OrdemManutencao> OrdensManutencao { get; }

    public WheelsContext(string pastaDados)
    {
        string P(string nome) => Path.Combine(pastaDados, nome);

        Clientes = new(P("clientes.csv"),
            "Id;Nome;Cpf;Telefone;Email;PontosFidelidade",
            x => x.ToCsv(), Cliente.FromCsv);
        Funcionarios = new(P("funcionarios.csv"),
            "Id;Nome;Cargo", x => x.ToCsv(), Funcionario.FromCsv);
        Bicicletas = new(P("bicicletas.csv"),
            "Id;Modelo;Tipo;Estado;ValorHora;ValorDeposito;Especial;Restricoes",
            x => x.ToCsv(), Bicicleta.FromCsv);
        Alugueis = new(P("alugueis.csv"),
            "Id;ClienteId;FuncionarioId;DataInicio;DataPrevistaDevolucao;DataDevolucao;Status;ValorTotal;Multa",
            x => x.ToCsv(), Aluguel.FromCsv);
        Itens = new(P("itens_aluguel.csv"),
            "AluguelId;BicicletaId;ValorUnitario", x => x.ToCsv(), ItemAluguel.FromCsv);
        Pagamentos = new(P("pagamentos.csv"),
            "Id;AluguelId;ValorPago;ValorPendente;DataPagamento;Forma",
            x => x.ToCsv(), Pagamento.FromCsv);
        Recibos = new(P("recibos.csv"),
            "Id;AluguelId;PagamentoId;DataEmissao", x => x.ToCsv(), Recibo.FromCsv);
        Reservas = new(P("reservas.csv"),
            "Id;ClienteId;BicicletaId;DataReserva;DataRetiradaPrevista;Status",
            x => x.ToCsv(), Reserva.FromCsv);
        OrdensManutencao = new(P("ordens_manutencao.csv"),
            "Id;BicicletaId;MecanicoId;Descricao;DataAbertura;DataFechamento;Status",
            x => x.ToCsv(), OrdemManutencao.FromCsv);

        Seed();
    }

    /// <summary>Gera o próximo Id inteiro para uma lista de entidades.</summary>
    public static int ProximoId<T>(IEnumerable<T> itens, Func<T, int> id)
        => itens.Any() ? itens.Max(id) + 1 : 1;

    /// <summary>Popula dados iniciais quando os arquivos estão vazios.</summary>
    private void Seed()
    {
        if (Funcionarios.Listar().Count == 0)
        {
            Funcionarios.Adicionar(new Funcionario { Id = 1, Nome = "Ana Souza", Cargo = "Atendente" });
            Funcionarios.Adicionar(new Funcionario { Id = 2, Nome = "Carlos Lima", Cargo = "Mecânico" });
        }
        if (Bicicletas.Listar().Count == 0)
        {
            Bicicletas.Adicionar(new Bicicleta { Id = 1, Modelo = "Caloi City Tour", Tipo = TipoBicicleta.Urbana, ValorHora = 12m, ValorDeposito = 100m });
            Bicicletas.Adicionar(new Bicicleta { Id = 2, Modelo = "Trek Marlin 5", Tipo = TipoBicicleta.Mountain, ValorHora = 18m, ValorDeposito = 200m });
            Bicicletas.Adicionar(new Bicicleta { Id = 3, Modelo = "Specialized Turbo", Tipo = TipoBicicleta.Eletrica, ValorHora = 30m, ValorDeposito = 400m });
            Bicicletas.Adicionar(new Bicicleta { Id = 4, Modelo = "Tandem Retrô 1980", Tipo = TipoBicicleta.Especial, ValorHora = 45m, ValorDeposito = 600m, Especial = true, Restricoes = "Uso somente em ciclovias planas" });
        }
        if (Clientes.Listar().Count == 0)
        {
            Clientes.Adicionar(new Cliente { Id = 1, Nome = "João Pereira", Cpf = "12345678901", Telefone = "(21) 99999-0001", Email = "joao@email.com", PontosFidelidade = 120 });
            Clientes.Adicionar(new Cliente { Id = 2, Nome = "Maria Oliveira", Cpf = "98765432100", Telefone = "(21) 99999-0002", Email = "maria@email.com", PontosFidelidade = 0 });
        }
    }
}
