namespace HelloFarma.Application.DTOs;

public record PlanoDto(Guid Id, string Nome, decimal PrecoMensal, int LimiteUsuarios, int LimiteFiliais, int LimiteProdutos, bool PermiteDelivery, bool PermiteIA);
