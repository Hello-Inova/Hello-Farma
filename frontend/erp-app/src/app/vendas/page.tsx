"use client";

import { useEffect, useState } from "react";
import { vendasApi, STATUS_VENDA_LABEL, type Venda } from "@/lib/vendas-api";

export default function VendasPage() {
  const [vendas, setVendas] = useState<Venda[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const [vendaEmDevolucao, setVendaEmDevolucao] = useState<string | null>(null);
  const [quantidades, setQuantidades] = useState<Record<string, number>>({});
  const [motivo, setMotivo] = useState("");
  const [processando, setProcessando] = useState(false);

  async function carregar() {
    setCarregando(true);
    setErro(null);
    try {
      const dados = await vendasApi.listarDoDia();
      setVendas(dados);
    } catch {
      setErro("Não foi possível carregar as vendas de hoje.");
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  function abrirDevolucao(venda: Venda) {
    setVendaEmDevolucao(venda.id);
    setQuantidades({});
    setMotivo("");
  }

  async function confirmarDevolucao(venda: Venda) {
    const itens = Object.entries(quantidades)
      .filter(([, qtd]) => qtd > 0)
      .map(([produtoId, quantidade]) => ({ produtoId, quantidade }));

    if (itens.length === 0) return;

    setProcessando(true);
    setErro(null);
    try {
      await vendasApi.devolver(venda.id, itens, motivo || undefined);
      setVendaEmDevolucao(null);
      await carregar();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Falha ao registrar devolução.");
    } finally {
      setProcessando(false);
    }
  }

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Vendas de hoje</h1>
        <p className="text-[var(--color-muted-foreground)]">Fechamento do dia e trocas/devoluções</p>
      </header>

      {erro && <p className="mb-4 text-sm text-[var(--color-danger)]">{erro}</p>}

      {carregando && <p className="text-sm text-[var(--color-muted-foreground)]">Carregando...</p>}
      {!carregando && vendas.length === 0 && (
        <p className="text-sm text-[var(--color-muted-foreground)]">Nenhuma venda registrada hoje.</p>
      )}

      <div className="space-y-3">
        {vendas.map((venda) => (
          <div key={venda.id} className="rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-4">
            <div className="flex flex-wrap items-center justify-between gap-2">
              <div>
                <p className="text-sm text-[var(--color-muted-foreground)]">
                  {new Date(venda.realizadaEmUtc).toLocaleTimeString("pt-BR")} — {STATUS_VENDA_LABEL[venda.status] ?? "-"}
                </p>
                <p className="font-medium">
                  Total R$ {venda.valorTotal.toFixed(2)}
                  {venda.cashbackUtilizado > 0 && (
                    <span className="text-[var(--color-muted-foreground)]"> · cashback usado R$ {venda.cashbackUtilizado.toFixed(2)}</span>
                  )}
                  {venda.cashbackGerado > 0 && (
                    <span className="text-[var(--color-muted-foreground)]"> · cashback gerado R$ {venda.cashbackGerado.toFixed(2)}</span>
                  )}
                </p>
              </div>
              {venda.status !== 4 && (
                <button
                  onClick={() => abrirDevolucao(venda)}
                  className="rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-1.5 text-xs"
                >
                  Trocar/Devolver
                </button>
              )}
            </div>

            <ul className="mt-2 space-y-1 text-sm text-[var(--color-muted-foreground)]">
              {venda.itens.map((item) => (
                <li key={item.produtoId} className="flex justify-between">
                  <span>{item.quantidade}x {item.produtoNome}</span>
                  <span>R$ {item.subtotal.toFixed(2)}</span>
                </li>
              ))}
            </ul>

            {vendaEmDevolucao === venda.id && (
              <div className="mt-4 rounded-[var(--radius-sm)] border border-[var(--color-border)] p-3">
                <p className="mb-2 text-sm font-medium">Selecione a quantidade a devolver por item:</p>
                <div className="space-y-2">
                  {venda.itens.map((item) => (
                    <div key={item.produtoId} className="flex items-center justify-between gap-2">
                      <span className="flex-1 text-sm">{item.produtoNome} (vendido: {item.quantidade})</span>
                      <input
                        type="number"
                        min={0}
                        max={item.quantidade}
                        value={quantidades[item.produtoId] ?? 0}
                        onChange={(e) =>
                          setQuantidades((atual) => ({
                            ...atual,
                            [item.produtoId]: Math.max(0, Math.min(item.quantidade, Number(e.target.value))),
                          }))
                        }
                        className="w-20 rounded-[var(--radius-sm)] border border-[var(--color-border)] px-2 py-1 text-center text-sm"
                      />
                    </div>
                  ))}
                </div>
                <input
                  value={motivo}
                  onChange={(e) => setMotivo(e.target.value)}
                  placeholder="Motivo (opcional)"
                  className="mt-3 w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm"
                />
                <div className="mt-3 flex gap-2">
                  <button
                    onClick={() => confirmarDevolucao(venda)}
                    disabled={processando}
                    className="rounded-[var(--radius-sm)] bg-[var(--color-primary)] px-4 py-2 text-sm font-medium text-[var(--color-primary-foreground)] disabled:opacity-60"
                  >
                    {processando ? "Processando..." : "Confirmar devolução"}
                  </button>
                  <button
                    onClick={() => setVendaEmDevolucao(null)}
                    className="rounded-[var(--radius-sm)] border border-[var(--color-border)] px-4 py-2 text-sm"
                  >
                    Cancelar
                  </button>
                </div>
              </div>
            )}
          </div>
        ))}
      </div>
    </main>
  );
}
