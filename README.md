# Hello Farma

Plataforma SaaS Enterprise especializada para o varejo farmacêutico brasileiro, desenvolvida pela **Hello Inova**.

> Ver o [Master Prompt completo](docs/MASTER_PROMPT.md) com todas as diretrizes de produto e arquitetura.

## Estrutura do repositório

```
hello-farma/
├── docs/                      # Documentação e master prompt
├── backend/                   # Solução .NET 9 (Clean Architecture + DDD)
│   ├── src/
│   │   ├── HelloFarma.Domain          # Entidades, Value Objects, Eventos de Domínio
│   │   ├── HelloFarma.Application     # Casos de uso (CQRS), DTOs, interfaces
│   │   ├── HelloFarma.Infrastructure  # EF Core, Repositórios, integrações
│   │   └── HelloFarma.API             # ASP.NET Core Web API (REST + OpenAPI)
│   └── tests/
│       ├── HelloFarma.UnitTests
│       └── HelloFarma.IntegrationTests
├── frontend/
│   ├── erp-app/                # ERP Hello Farma (Next.js) — operação diária da farmácia
│   ├── portal-app/             # Portal do Cliente (e-commerce por farmácia)
│   ├── delivery-app/           # App do entregador
│   └── platform-app/           # Hello Platform — painel administrativo da Hello Inova
└── docker-compose.yml          # Postgres, Redis, RabbitMQ, MinIO para desenvolvimento local
```

## Aplicações

1. **Hello Platform** — administração da Hello Inova (clientes, planos, billing, auditoria).
2. **ERP Hello Farma** — aplicação principal da farmácia (PDV, estoque, compras, financeiro, fiscal, CRM, IA).
3. **Portal do Cliente** — e-commerce integrado por tenant.
4. **App do Entregador** — gestão de entregas em tempo real.

## Arquitetura

Clean Architecture + DDD, multi-tenant (cada farmácia é um `TenantId` isolado), API First (REST/OpenAPI), Modular Monolith preparado para microsserviços, CQRS via MediatR e eventos de domínio para comunicação entre módulos.

## Stack

- **Backend:** .NET 9, ASP.NET Core, EF Core, PostgreSQL, Redis, SignalR, Hangfire, RabbitMQ
- **Frontend:** Next.js, TypeScript, Tailwind CSS, shadcn/ui, TanStack Query, Zustand, React Hook Form, Zod
- **Mobile:** Flutter
- **Infra:** Docker, Kubernetes (preparado), GitHub Actions, Nginx, Cloudflare, MinIO/S3

## Como rodar localmente

```bash
# Infraestrutura (Postgres, Redis, RabbitMQ, MinIO)
docker compose up -d

# Backend
cd backend
dotnet restore
dotnet run --project src/HelloFarma.API

# Frontend (ERP)
cd frontend/erp-app
npm install
npm run dev
```

## Regra de desenvolvimento

Antes de implementar qualquer funcionalidade nova, seguir o processo descrito no [Master Prompt](docs/MASTER_PROMPT.md): problema de negócio → objetivo → regras de negócio → casos de uso → modelagem → fluxo de interface → endpoints → componentes → testes → critérios de aceite.

---
**Hello Inova** · contato@helloinova.com.br · [helloinova.com.br](https://www.helloinova.com.br/)
