using System.Text;

namespace WheelsRental.Data;

/// <summary>
/// Repositório genérico que persiste entidades em arquivos CSV (uma entidade
/// por linha). É a base da camada de persistência iniciada no TP3 e consolidada
/// no TP5. Cada entidade fornece suas funções de serialização (ToCsv/FromCsv).
/// </summary>
/// <typeparam name="T">Tipo da entidade persistida.</typeparam>
public class CsvRepository<T>
{
    private readonly string _caminho;
    private readonly string _cabecalho;
    private readonly Func<T, string> _serializar;
    private readonly Func<string, T> _desserializar;
    private readonly object _lock = new();

    public CsvRepository(string caminho, string cabecalho,
                         Func<T, string> serializar, Func<string, T> desserializar)
    {
        _caminho = caminho;
        _cabecalho = cabecalho;
        _serializar = serializar;
        _desserializar = desserializar;

        var dir = Path.GetDirectoryName(_caminho);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        if (!File.Exists(_caminho))
            File.WriteAllText(_caminho, _cabecalho + Environment.NewLine, Encoding.UTF8);
    }

    /// <summary>Lê todas as entidades do arquivo CSV (ignora o cabeçalho).</summary>
    public List<T> Listar()
    {
        lock (_lock)
        {
            return File.ReadAllLines(_caminho, Encoding.UTF8)
                       .Skip(1)
                       .Where(l => !string.IsNullOrWhiteSpace(l))
                       .Select(_desserializar)
                       .ToList();
        }
    }

    /// <summary>Reescreve o arquivo CSV inteiro com a lista informada.</summary>
    public void SalvarTodos(IEnumerable<T> itens)
    {
        lock (_lock)
        {
            var sb = new StringBuilder();
            sb.AppendLine(_cabecalho);
            foreach (var item in itens) sb.AppendLine(_serializar(item));
            File.WriteAllText(_caminho, sb.ToString(), Encoding.UTF8);
        }
    }

    /// <summary>Adiciona uma nova entidade ao final do arquivo.</summary>
    public void Adicionar(T item)
    {
        lock (_lock)
        {
            File.AppendAllText(_caminho, _serializar(item) + Environment.NewLine, Encoding.UTF8);
        }
    }
}
