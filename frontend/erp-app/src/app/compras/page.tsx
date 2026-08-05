"use client";

import { useEffect, useState } from "react";
import { comprasApi } from "@/lib/compras-api";
import { STATUS_PEDIDO_LABEL, type PedidoCompra } from "@/types/compra";

export default function ComprasPage() {
  const [pedidos, setPedidos] = useState<PedidoCompra[]>([]);
  const [carregando, setCarregando] = useState(true);

  async function carregar() {
    setCarregando(true);
    const dados = await comprasApi.listarPedidos();
    setPedidos(dados);
    setCarregando(false);
  }

  useEffect(() => {
    carregar();
  }, []);

  async function handleReceber(id: string) {
    await comprasApi.receberPedido(id);
    carregar();
  }

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Compras</h1>
        <p className="text-[var(--color-muted-foreground)]">
          Fluxo: Cotação → Pedido → Recebimento → Conferência → Entrada em estoque
        </p>
      </header>

      <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
            <tr>
              <th className="px-4 py-3">Pedido</th>
              <th className="px-4 py-3">Itens</th>
              <th className="px-4 py-3">Valor total</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody>
            {carregando && (
              <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Carregando...</td></tr>
            )}
            {!carregando && pedidos.length === 0 && (
              <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Nenhum pedido de compra.</td></tr>
            )}
            {pedidos.map((p) => (
              <tr key={p.id} className="border-b border-[var(--color-border)] last:border-0">
                <td className="px-4 py-3 font-mono text-xs">{p.id.slice(0, 8)}</td>
                <td className="px-4 py-3">{p.itens.length} item(ns)</td>
                <td className="px-4 py-3">R$ {p.valorTotal.toFixed(2)}</td>
                <td className="px-4 py-3">{STATUS_PEDIDO_LABEL[p.status]}</td>
                <td className="px-4 py-3 text-right">
                  {p.status === 2 && (
                    <button onClick={() => handleReceber(p.id)} className="text-xs text-[var(--color-primary)]">
                      Confirmar recebimento
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
