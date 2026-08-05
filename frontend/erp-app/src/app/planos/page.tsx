"use client";

import { useEffect, useState } from "react";
import { billingApi, type Plano } from "@/lib/billing-api";

export default function PlanosPage() {
  const [planos, setPlanos] = useState<Plano[]>([]);

  useEffect(() => {
    billingApi.listarPlanos().then(setPlanos);
  }, []);

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Planos</h1>
        <p className="text-[var(--color-muted-foreground)]">Assinatura Hello Farma — Hello Platform</p>
      </header>

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
        {planos.map((p) => (
          <div key={p.id} className="rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-6">
            <p className="text-sm text-[var(--color-muted-foreground)]">{p.nome}</p>
            <p className="mb-4 text-2xl font-semibold">R$ {p.precoMensal.toFixed(2)}/mês</p>
            <ul className="space-y-1 text-sm text-[var(--color-muted-foreground)]">
              <li>Até {p.limiteUsuarios} usuários</li>
              <li>Até {p.limiteFiliais} filiais</li>
              <li>Até {p.limiteProdutos} produtos</li>
              <li>{p.permiteDelivery ? "✓" : "✗"} Delivery</li>
              <li>{p.permiteIA ? "✓" : "✗"} Hello Farma IA</li>
            </ul>
          </div>
        ))}
      </div>
    </main>
  );
}
