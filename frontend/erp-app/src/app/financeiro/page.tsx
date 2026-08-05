"use client";

import { useEffect, useState } from "react";
import { financeiroApi, type ContaFinanceira, type FluxoCaixa } from "@/lib/financeiro-api";
import { KpiCard } from "@/components/dashboard/KpiCard";

const STATUS_LABEL: Record<number, string> = { 1: "Pendente", 2: "Paga", 3: "Vencida", 4: "Cancelada" };

export default function FinanceiroPage() {
  const [contas, setContas] = useState<ContaFinanceira[]>([]);
  const [fluxo, setFluxo] = useState<FluxoCaixa | null>(null);

  async function carregar() {
    const [c, f] = await Promise.all([financeiroApi.listarContas(), financeiroApi.fluxoCaixa()]);
    setContas(c);
    setFluxo(f);
  }

  useEffect(() => {
    carregar();
  }, []);

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Financeiro</h1>
        <p className="text-[var(--color-muted-foreground)]">Fluxo de caixa e contas a pagar/receber</p>
      </header>

      {fluxo && (
        <section className="mb-6 grid grid-cols-1 gap-4 sm:grid-cols-3">
          <KpiCard titulo="Entradas (mês)" valor={`R$ ${fluxo.totalEntradas.toFixed(2)}`} tendencia="alta" />
          <KpiCard titulo="Saídas (mês)" valor={`R$ ${fluxo.totalSaidas.toFixed(2)}`} tendencia="baixa" />
          <KpiCard titulo="Saldo" valor={`R$ ${fluxo.saldo.toFixed(2)}`} tendencia={fluxo.saldo >= 0 ? "alta" : "baixa"} />
        </section>
      )}

      <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
            <tr>
              <th className="px-4 py-3">Descrição</th>
              <th className="px-4 py-3">Tipo</th>
              <th className="px-4 py-3">Valor</th>
              <th className="px-4 py-3">Vencimento</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody>
            {contas.map((c) => (
              <tr key={c.id} className="border-b border-[var(--color-border)] last:border-0">
                <td className="px-4 py-3">{c.descricao}</td>
                <td className="px-4 py-3">{c.tipo === 1 ? "Receber" : "Pagar"}</td>
                <td className="px-4 py-3">R$ {c.valor.toFixed(2)}</td>
                <td className="px-4 py-3">{new Date(c.dataVencimento).toLocaleDateString("pt-BR")}</td>
                <td className="px-4 py-3">{STATUS_LABEL[c.status]}</td>
                <td className="px-4 py-3 text-right">
                  {c.status === 1 && (
                    <button
                      onClick={async () => { await financeiroApi.baixarConta(c.id); carregar(); }}
                      className="text-xs text-[var(--color-primary)]"
                    >
                      Baixar
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </main>
  );
}
