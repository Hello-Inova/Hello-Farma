"use client";

import { useEffect, useState } from "react";
import { deliveryApi, STATUS_DELIVERY_LABEL, type PedidoDelivery } from "@/lib/delivery-api";

const PROXIMO_STATUS: Record<number, number | null> = { 1: 2, 2: 3, 3: 4, 4: 5, 5: 6, 6: null, 9: null };

export default function DeliveryPage() {
  const [pedidos, setPedidos] = useState<PedidoDelivery[]>([]);

  async function carregar() {
    setPedidos(await deliveryApi.listar());
  }

  useEffect(() => {
    carregar();
  }, []);

  async function avancar(pedido: PedidoDelivery) {
    const proximo = PROXIMO_STATUS[pedido.status];
    if (!proximo) return;
    await deliveryApi.avancarStatus(pedido.id, proximo);
    carregar();
  }

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Delivery</h1>
        <p className="text-[var(--color-muted-foreground)]">
          Pedido → Pagamento → Separação → Expedição → Entrega → Avaliação
        </p>
      </header>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {pedidos.map((p) => (
          <div key={p.id} className="rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-4">
            <p className="text-xs text-[var(--color-muted-foreground)]">Pedido {p.id.slice(0, 8)}</p>
            <p className="mb-2 font-medium">{p.enderecoEntrega}</p>
            <span className="mb-3 inline-block rounded-full bg-[var(--color-primary)]/10 px-2 py-0.5 text-xs text-[var(--color-primary)]">
              {STATUS_DELIVERY_LABEL[p.status]}
            </span>
            {PROXIMO_STATUS[p.status] && (
              <button onClick={() => avancar(p)} className="block w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] py-1.5 text-xs">
                Avançar para {STATUS_DELIVERY_LABEL[PROXIMO_STATUS[p.status]!]}
              </button>
            )}
          </div>
        ))}
        {pedidos.length === 0 && <p className="text-sm text-[var(--color-muted-foreground)]">Nenhum pedido de delivery em andamento.</p>}
      </div>
    </main>
  );
}
