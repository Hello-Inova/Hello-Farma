# Auditoria: implementação atual vs. Master Prompt

Data: 2026-08-05. Revisão do que foi construído nos módulos 1–8 contra cada exigência do [MASTER_PROMPT.md](./MASTER_PROMPT.md).

Resumo honesto: o que existe hoje é um **núcleo operacional funcional de um único módulo (ERP)** — auth, produtos, estoque com FEFO, PDV, compras, financeiro básico, CRM/delivery básico e stubs de fiscal/IA/billing. É uma fração real do que o master prompt pede. As lacunas abaixo são grandes, principalmente em: as outras 3 aplicações da plataforma, multi-filial, mensageria/tempo real, segurança de produção, e boa parte da profundidade de cada módulo (trocas/devoluções, campanhas, conciliação, SPED, etc.).

## 1. Arquitetura

| Exigência | Status |
|---|---|
| Clean Architecture / DDD / SOLID | Feito — camadas Domain/Application/Infrastructure/API separadas |
| Repository + Service Pattern + DI | Feito |
| API First / REST / Swagger | Feito — Swagger configurado com JWT |
| CQRS | Feito via MediatR (Commands/Queries) |
| Event Driven | **Não feito** — existe `DomainEvent` base class mas nenhum evento é disparado ou consumido; RabbitMQ está no docker-compose mas nenhum código publica/consome fila |
| Modular Monolith | Parcial — módulos organizados por pasta, mas sem separação de assembly/projeto por módulo (tudo dentro de Application/Infrastructure únicos) |

## 2. As quatro aplicações da plataforma

| Aplicação | Status |
|---|---|
| ERP Hello Farma | Construído (backend completo + telas principais) |
| Hello Platform (admin da Hello Inova) | **Não construído** — só existe billing básico dentro do próprio ERP; não há painel de clientes/farmácias, auditoria, monitoramento, suporte, feature flags |
| Portal do Cliente (e-commerce por farmácia) | **Não construído** — `frontend/portal-app` é só um placeholder |
| App do Entregador | **Não construído** — `frontend/delivery-app` é só um placeholder |

## 3. Multi-empresa / multi-filial

- Isolamento por `TenantId` com query filter global: **feito**.
- **Conceito de Filial não existe.** O master prompt fala em limite de filiais por plano e trocas/transferências entre filiais — hoje o sistema só entende uma "farmácia" como unidade única, não uma rede com múltiplas lojas.

## 4. Modelo comercial / Billing

- `Plano` com limites (usuários, filiais, produtos, delivery, IA): entidade existe, **mas nenhum limite é verificado** em tempo de execução (dá para cadastrar 500 usuários mesmo num plano de 5).
- `Assinatura` com Trial/Ativa/Cancelada/Inadimplente: feito.
- **Não implementado:** Upgrade/Downgrade de plano, Proration, Cupons, Créditos, Cobranças/Faturas, integração de pagamento real (Mercado Pago/Asaas/Stone/Pagar.me/Efí) — hoje não existe cobrança de fato, só o registro da assinatura.

## 5. Pagamentos

- Enum `FormaPagamento` cobre Pix/Cartão Crédito/Débito/Dinheiro/Convênio.
- **Faltam:** QR Code e Link de Pagamento como formas explícitas, e qualquer integração real de gateway (hoje é só um número guardado, não processa pagamento nenhum).

## 6. Especialização farmacêutica (Produto)

- ANVISA, EAN, Laboratório, Princípio Ativo, Categoria Terapêutica, Forma Farmacêutica, Concentração, Genérico/Similar/Referência, Controlado, Receita Obrigatória, PMC, PF: **feito**.
- **Falta:** campo de Temperatura de armazenamento no produto (só existe Localização, e essa está no Lote).

## 7. Estoque

| Exigência | Status |
|---|---|
| Lotes / Validade | Feito |
| Regra FEFO na saída | Feito |
| Produtos próximos do vencimento | Feito |
| Curva ABC | **Não feito** |
| Giro de estoque | **Não feito** |
| Inventário (contagem/ajuste) | **Não feito** — só existem entrada e saída, não um fluxo de inventário físico com divergência |
| Transferências entre filiais | **Não feito** (depende do conceito de Filial, que não existe) |
| Perdas / Avarias | Enum existe (`TipoMovimentacaoEstoque.Perda/Avaria`) mas **não há caso de uso/endpoint dedicado** para registrá-las — só entrada/saída genéricas |

## 8. Medicamentos controlados

- Flags `Controlado` e `ReceitaObrigatoria` no produto: feito.
- **Não feito:** entidade de Receituário (dados da receita/prescritor vinculados à venda), regras de auditoria específicas para controlados, qualquer exigência formal da ANVISA além do flag booleano.

## 9. Compras

- Fluxo Cotação → Pedido → Recebimento: feito (cotação e pedido colapsados em uma única chamada).
- **Conferência**: existe o método `Conferir()` na entidade, mas **nenhum comando/endpoint chama isso** — o status nunca avança para Conferido na prática.
- **Importação de XML de NF-e do fornecedor**: **não feito**.

## 10. PDV

- Busca rápida, carrinho, formas de pagamento, baixa de estoque automática: feito.
- **Não feito:** atalhos de teclado além do autofoco na busca, integração de cashback (o cliente tem saldo de cashback no CRM, mas o PDV não acumula nem resgata), fidelidade, trocas, devoluções. Vendas hoje não têm vínculo opcional a cliente na tela (o campo existe no backend, mas a UI do PDV não deixa selecionar cliente).

## 11. Delivery

- Máquina de estados Pendente→Separação→Expedição→Em rota→Entregue→Avaliado: feito.
- **Tempo real via WebSockets/SignalR: não feito** — é mencionado em comentário no código mas não há hub SignalR nem push de atualização; a tela precisa recarregar manualmente.
- Não há tela/app do entregador de fato (só a lista Kanban dentro do ERP).

## 12. CRM

- Cadastro de cliente com saldo de cashback: feito.
- **Não feito:** histórico de compras do cliente (não há tela ligando Cliente → Vendas), frequência/recompra, campanhas, integração WhatsApp/SMS/Email, fidelização além do campo de saldo.

## 13. Financeiro

- Fluxo de caixa do mês, contas a pagar/receber (manuais e automáticas via venda/compra): feito.
- **Não feito:** conciliação bancária/de cartão, detalhamento por forma de pagamento no fluxo de caixa.

## 14. Fiscal

- `DocumentoFiscal` criado automaticamente por venda, com `IEmissorFiscal` como Strategy plugável: feito como **stub simulado**.
- **Não feito:** qualquer integração real com SEFAZ/SAT, geração de XML de NFC-e/NF-e válido, SPED.

## 15. Inteligência Artificial

- Só existe **previsão de vendas** (heurística de média móvel simples).
- **Não feito:** previsão de estoque/ruptura, alertas de vencimento via IA (hoje é regra fixa, não preditiva), sugestão de campanhas/promoções, sugestão de compras, relatórios/análises financeiras assistidas, suporte ao gestor (chat/insights).

## 16. Theme Engine / Design System

- Design tokens via CSS variables (`globals.css`, `theme-tokens.ts`) com suporte a light/dark: feito como **fundação técnica**.
- **Não feito:** nenhuma interface para o tenant customizar de fato (trocar logo, cores, tipografia pela UI) nem persistência desses tokens por tenant no backend — hoje é só o valor padrão fixo no CSS.
- shadcn/ui, mencionado na stack, **não foi integrado** ao frontend.

## 17. Stack técnica — itens do master prompt não usados ainda

- **Redis**: no docker-compose, mas nenhum código lê/escreve cache.
- **SignalR**: não implementado (ver Delivery acima).
- **Hangfire**: não implementado (nenhum job/agendamento roda).
- **RabbitMQ**: no docker-compose, mas nada publica/consome mensagens.
- **MinIO/S3**: no docker-compose, mas não há upload de arquivo algum (ex.: foto de produto, logo do tenant, XML de NF-e).
- Frontend: **TanStack Query, Zustand, React Hook Form, Zod, Framer Motion, shadcn/ui** — nenhum desses foi adicionado ao `package.json`; os formulários hoje são `useState` simples e chamadas `fetch` diretas.
- **Flutter** (mobile/app do entregador): não iniciado.
- **CI/CD (GitHub Actions), Kubernetes, Nginx, Cloudflare**: não configurados.

## 18. Segurança

| Exigência | Status |
|---|---|
| JWT + Refresh Token | Feito |
| BCrypt | Feito |
| HTTPS | Não configurado (ambiente é dev/local) |
| Rate Limiting | **Não feito** |
| Auditoria (log de quem fez o quê) | **Não feito** — só há `CreatedAtUtc`/`UpdatedAtUtc`, sem log de usuário responsável por cada ação |
| LGPD (consentimento, anonimização, exportação/exclusão de dados pessoais) | **Não feito** |
| Backup | Não configurado |
| Monitoramento (Datadog/App Insights/etc.) | Não configurado |
| 2FA | **Não feito**, nem preparado |

## Testes

- 10 arquivos de teste unitário cobrindo regras de domínio centrais (Lote/FEFO, Venda, PedidoCompra, PedidoDelivery, ContaFinanceira, DocumentoFiscal, Assinatura). **Não há testes de integração** (o projeto `HelloFarma.IntegrationTests` existe mas está vazio) nem testes de frontend.

## O que eu sugiro priorizar a seguir

Não é possível fazer tudo de uma vez com qualidade — segue uma ordem sugerida por impacto:

1. **Multi-filial** — é um conceito estrutural que falta e vários outros itens dependem dele (transferência de estoque, limite de plano).
2. **PDV: cashback, trocas/devoluções, vínculo com cliente** — fecha o ciclo comercial que já está 80% pronto.
3. **Auditoria + Rate Limiting** — segurança mínima antes de qualquer uso real.
4. **Hello Platform** (painel da Hello Inova) — hoje não existe visibilidade sobre os tenants da plataforma.
5. **Integração de pagamento real** (ex.: Mercado Pago ou Asaas) — sem isso, billing e vendas por cartão/PIX são só registro, não cobrança de fato.
6. **SignalR no Delivery** — pequeno esforço, alto ganho de UX.
7. Depois disso: Portal do Cliente, App do Entregador (Flutter), IA mais robusta, integração fiscal real.

Quer que eu comece por algum desses, ou prefere revisar esta lista e me dizer a prioridade?
