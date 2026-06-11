namespace WheelsRental.Models;

/// <summary>Tipo da bicicleta. Influencia o valor por hora do aluguel.</summary>
public enum TipoBicicleta
{
    Urbana,
    Mountain,
    Eletrica,
    Especial
}

/// <summary>Estado atual de uma bicicleta no acervo da loja.</summary>
public enum EstadoBicicleta
{
    Disponivel,
    Alugada,
    Manutencao,
    Reservada
}

/// <summary>Situação de um aluguel.</summary>
public enum StatusAluguel
{
    Aberto,
    Finalizado,
    Cancelado
}

/// <summary>Forma de pagamento utilizada pelo cliente.</summary>
public enum FormaPagamento
{
    Dinheiro,
    Cartao,
    Pix
}

/// <summary>Situação de uma reserva feita pelo cliente (mudança de requisitos do TP4).</summary>
public enum StatusReserva
{
    Pendente,
    Confirmada,
    Cancelada,
    Expirada
}

/// <summary>Situação de uma ordem de manutenção (mudança de requisitos do TP4).</summary>
public enum StatusManutencao
{
    Aberta,
    EmAndamento,
    Concluida
}
