namespace HelloFarma.Application.DTOs;

public record PedidoDeliveryDto(Guid Id, Guid VendaId, string EnderecoEntrega, int Status, Guid? EntregadorId, int? AvaliacaoNota);
