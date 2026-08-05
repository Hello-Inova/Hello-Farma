"use client";

import { useEffect, useState } from "react";
import { KpiCard } from "@/components/dashboard/KpiCard";
import { iaApi, type PrevisaoVendas } from "@/lib/ia-api";

export default function DashboardPage() {
  const [previsao, setPrevisao] = useState<PrevisaoVendas | null>(null);

  useEffect(() => {
    iaApi.previsaoVendas().then(setPrevisao).catch(() => {});
  }, []);

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Hello Farma</h1>
        <p className="text-[var(--color-muted-foreground)]">
          Visão geral da operação da farmácia
        </p>
      </header>

      <section className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <KpiCard titulo="Vendas hoje" valor="R$ 4.320,00" variacao="+8% vs. ontem" tendencia="alta" />
        <KpiCard titulo="Ticket médio" valor="R$ 62,40" variacao="+3%" tendencia="alta" />
        <KpiCard titulo="Produtos próx. do vencimento" valor="17" variacao="atenção" tendencia="baixa" />
        <KpiCard titulo="Pedidos delivery em rota" valor="5" tendencia="neutra" />
        {previsao && (
          <KpiCard
            titulo="Hello Farma IA — previsão 7 dias"
            valor={`R$ ${previsao.previsaoProximos7Dias.toFixed(2)}`}
            variacao={`média diária: R$ ${previsao.mediaDiariaUltimos30Dias.toFixed(2)}`}
            tendencia="neutra"
          />
        )}
      </section>
    </main>
  );
}
