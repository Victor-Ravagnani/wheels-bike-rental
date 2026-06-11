# Sistema de Aluguel de Bicicletas Wheels

Projeto de Bloco — Análise e Desenvolvimento de Sistemas (Instituto Infnet)
Aluno: **Victor Bueno Ravagnani**

Sistema desenvolvido em **C# / ASP.NET Core Razor Pages (.NET 8)**, com
**persistência em arquivos CSV**, consolidando as etapas TP1 a TP5 do projeto.

## Como executar

Pré-requisito: [.NET SDK 8.0](https://dotnet.microsoft.com/download).

```bash
cd WheelsRental
dotnet run
```

O navegador deve abrir em `https://localhost:5001` (ou na porta indicada no
console). Na primeira execução, os dados iniciais (clientes, funcionários e
bicicletas) são gerados automaticamente.

## Estrutura

```
WheelsRental/
├── Program.cs              # Configuração da aplicação e injeção de dependência
├── WheelsRental.csproj     # Projeto .NET 8 (Razor Pages)
├── Models/                 # Entidades de domínio + serialização CSV
│   ├── Cliente.cs, Bicicleta.cs, Aluguel.cs, ItemAluguel.cs
│   ├── Pagamento.cs, Recibo.cs, Funcionario.cs
│   ├── Reserva.cs, OrdemManutencao.cs   (mudanças do TP4)
│   └── Enums.cs
├── Data/                   # Camada de persistência CSV
│   ├── CsvRepository.cs     # Repositório genérico (leitura/escrita CSV)
│   └── WheelsContext.cs     # Agrega repositórios + dados iniciais
├── Services/               # Regras de negócio
│   ├── ServicoAluguel.cs    # Aluguel, devolução, multa, pagamento, recibo
│   ├── ServicoReserva.cs    # Reserva (autoatendimento - TP4)
│   └── ServicoManutencao.cs # Ordens de manutenção (TP4)
├── Pages/                  # Interface (Razor Pages)
│   ├── Index, Bicicletas/, Clientes/, Alugueis/, Reservas/, Manutencao/
│   └── Shared/_Layout.cshtml
└── App_Data/csv/           # Arquivos CSV gerados em tempo de execução
```

## Persistência (CSV)

Cada entidade implementa `ToCsv()` e `FromCsv()`. O `CsvRepository<T>` genérico
lê e grava listas em arquivos `.csv` separados por `;`, criando cabeçalho e
diretório automaticamente. Os arquivos ficam em `App_Data/csv/`.

## Regras de negócio principais

- **Cálculo do valor**: `valorHora × horas de uso`, somado por bicicleta.
- **Desconto de fidelidade**: 5% / 10% / 15% conforme pontos do cliente.
- **Multa por atraso**: 50% sobre as horas em atraso (somente se devolução tardia).
- **Reserva**: cliente reserva bicicleta disponível (autoatendimento).
- **Manutenção**: mecânico abre ordem e a bicicleta vai para "Manutenção".
