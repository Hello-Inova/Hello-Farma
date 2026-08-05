"use client";

import { useEffect, useState } from "react";
import { estoqueApi } from "@/lib/estoque-api";
import type { Lote } from "@/types/estoque";

function corAlerta(dias: number) {
  if (dias <= 30) return "text-[var(--color-danger)]";
  if (dias <= 60) return "text-[var(--color-warning)]";
  return "text-[var(--color-muted-foreground)]";
}

export default function EstoquePage() {
  const [lotes, setLotes] = useState<Lote[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    estoqueApi
      .lotesProximosVencimento(90)
      .then(setLotes)
      .finally(() => setCarregando(false));
  }, []);

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Estoque</h1>
        <p className="text-[var(--color-muted-foreground)]">
          Lotes próximos do vencimento (próximos 90 dias) — regra FEFO aplicada nas saídas
        </p>
      </header>

      <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
            <tr>
              <th className="px-4 py-3">Produto</th>
              <th className="px-4 py-3">Lote</th>
              <th className="px-4 py-3">Validade</th>
              <th className="px-4 py-3">Qtd.</th>
              <th className="px-4 py-3">Dias p/ vencer</th>
            </tr>
          </thead>
          <tbody>
            {carregando && (
              <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Carregando...</td></tr>
            )}
            {!carregando && lotes.length === 0 && (
              <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Nenhum lote próximo do vencimento.</td></tr>
            )}
            {lotes.map((l) => (
              <tr key={l.id} className="border-b border-[var(--color-border)] last:border-0">
                <td className="px-4 py-3 font-medium">{l.produtoNome}</td>
                <td className="px-4 py-3">{l.numeroLote}</td>
                <td className="px-4 py-3">{new Date(l.validade).toLocaleDateString("pt-BR")}</td>
                <td className="px-4 py-3">{l.quantidadeAtual}</td>
                <td className={`px-4 py-3 font-medium ${corAlerta(l.diasParaVencer)}`}>{l.diasParaVencer} dias</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </main>
  );
}
