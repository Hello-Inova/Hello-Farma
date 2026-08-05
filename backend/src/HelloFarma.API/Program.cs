using System.Text;
using System.Threading.RateLimiting;
using HelloFarma.Application.Behaviors;
using HelloFarma.Application.Interfaces;
using HelloFarma.Application.UseCases.Produtos.CriarProduto;
using HelloFarma.Domain.Entities.Auth;
using HelloFarma.Application.Services;
using HelloFarma.Infrastructure.Identity;
using HelloFarma.Infrastructure.Persistence;
using HelloFarma.Infrastructure.Repositories;
using HelloFarma.Infrastructure.Persistence.Seed;
using HelloFarma.Infrastructure.Services;
using HelloFarma.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// API First — Swagger/OpenAPI habilitado por padrão
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    });
    options.AddSecurityRequirement(new()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CriarProdutoCommand).Assembly);
    cfg.AddOpenBehavior(typeof(AuditoriaBehavior<,>));
});

builder.Services.AddDbContext<HelloFarmaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Multi-tenant: resolve o TenantId do usuário autenticado a partir do JWT
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenant, CurrentTenantAccessor>();
builder.Services.AddScoped<IRequestContext, RequestContextAccessor>();

// Repositórios (Repository Pattern) e Unit of Work
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ILoteRepository, LoteRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
builder.Services.AddScoped<IBaixaEstoqueService, BaixaEstoqueService>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<ICurrentUser, CurrentUserAccessor>();
builder.Services.AddScoped<IEntradaEstoqueService, EntradaEstoqueService>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddScoped<IPedidoCompraRepository, PedidoCompraRepository>();
builder.Services.AddScoped<IContaFinanceiraRepository, ContaFinanceiraRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoDeliveryRepository, PedidoDeliveryRepository>();
builder.Services.AddScoped<IDocumentoFiscalRepository, DocumentoFiscalRepository>();
builder.Services.AddScoped<IEmissorFiscal, EmissorFiscalSimulado>();
builder.Services.AddScoped<IPlanoRepository, PlanoRepository>();
builder.Services.AddScoped<IAssinaturaRepository, AssinaturaRepository>();
builder.Services.AddScoped<IFilialRepository, FilialRepository>();
builder.Services.AddScoped<IDevolucaoRepository, DevolucaoRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Segurança
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? new JwtSettings { SecretKey = "dev-secret-troque-em-producao-32-caracteres-min" };

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Sem isso, o ASP.NET Core remapeia automaticamente claims JWT padrão (ex.: "sub")
        // para URIs longas de ClaimTypes, quebrando a leitura de JwtRegisteredClaimNames.Sub
        // feita em CurrentUserAccessor — o que zerava silenciosamente o UsuarioId em toda requisição.
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});


// Rate limiting — protege contra abuso e força bruta (ex.: tentativas de login).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Política global: por IP, 120 requisições por minuto.
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var chave = httpContext.Connection.RemoteIpAddress?.ToString() ?? "desconhecido";
        return RateLimitPartition.GetFixedWindowLimiter(chave, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 120,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
        });
    });

    // Política restrita para autenticação — mitiga força bruta de login/registro.
    options.AddPolicy("auth", httpContext =>
    {
        var chave = httpContext.Connection.RemoteIpAddress?.ToString() ?? "desconhecido";
        return RateLimitPartition.GetFixedWindowLimiter(chave, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HelloFarmaDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.Run();
