namespace HelloFarma.Application.DTOs;

public record PrevisaoVendasDto(decimal MediaDiariaUltimos30Dias, decimal PrevisaoProximos7Dias, int DiasAnalisados);
