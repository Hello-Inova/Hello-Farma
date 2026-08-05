using HelloFarma.Application.DTOs;
using MediatR;

namespace HelloFarma.Application.UseCases.Plataforma.ListarTenants;

/// <summary>Lista todas as farmácias (tenants) da plataforma — uso exclusivo da Hello Platform (SuperAdmin).</summary>
public record ListarTenantsQuery : IRequest<IReadOnlyList<TenantPlataformaDto>>;
