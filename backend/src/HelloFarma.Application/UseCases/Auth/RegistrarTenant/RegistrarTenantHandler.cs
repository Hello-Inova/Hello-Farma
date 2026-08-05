using HelloFarma.Application.Interfaces;
using HelloFarma.Domain.Entities.Empresa;
using HelloFarma.Domain.Entities.Tenants;
using HelloFarma.Domain.Entities.Usuarios;
using HelloFarma.Domain.Enums;
using MediatR;

namespace HelloFarma.Application.UseCases.Auth.RegistrarTenant;

/// <summary>
/// Regra de negócio: CNPJ deve ser único na plataforma. O primeiro usuário criado
/// para o tenant é sempre um Administrador. Toda farmácia nasce com uma filial
/// Matriz padrão (que não pode ser desativada).
/// </summary>
public class RegistrarTenantHandler(
    ITenantRepository tenantRepository,
    IUsuarioRepository usuarioRepository,
    IFilialRepository filialRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarTenantCommand, Guid>
{
    public async Task<Guid> Handle(RegistrarTenantCommand request, CancellationToken ct)
    {
        if (await tenantRepository.CnpjExisteAsync(request.Cnpj, ct))
            throw new InvalidOperationException("Já existe uma farmácia cadastrada com este CNPJ.");

        var tenant = new Tenant(request.RazaoSocial, request.NomeFantasia, request.Cnpj, request.PlanoId);
        await tenantRepository.AddAsync(tenant, ct);

        var senhaHash = passwordHasher.Hash(request.SenhaAdmin);
        var admin = new Usuario(tenant.Id, request.NomeAdmin, request.EmailAdmin, senhaHash, PapelUsuario.Administrador);
        await usuarioRepository.AddAsync(admin, ct);

        var matriz = new Filial(tenant.Id, request.NomeFantasia, request.Cnpj, null, null, null, matriz: true);
        await filialRepository.AddAsync(matriz, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return tenant.Id;
    }
}
