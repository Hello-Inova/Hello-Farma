"use client";

import { useEffect, useRef, useState } from "react";
import { produtosApi } from "@/lib/produtos-api";
import { vendasApi } from "@/lib/vendas-api";
import type { Produto } from "@/types/produto";

interface LinhaCarrinho {
  produto: Produto;
  quantidade: number;
}

const FORMAS_PAGAMENTO = [
  { valor: 1, label: "PIX" },
  { valor: 2, label: "Cartão de crédito" },
  { valor: 3, label: "Cartão de débito" },
  { valor: 4, label: "Dinheiro" },
  { valor: 5, label: "Convênio" },
];

export default function PdvPage() {
  const [busca, setBusca] = useState("");
  const [resultados, setResultados] = useState<Produto[]>([]);
  const [carrinho, setCarrinho] = useState<LinhaCarrinho[]>([]);
  const [formaPagamento, setFormaPagamento] = useState(1);
  const [mensagem, setMensagem] = useState<string | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    inputRef.current?.focus();
  }, []);

  async function handleBusca(termo: string) {
    setBusca(termo);
    if (termo.trim().length < 2) {
      setResultados([]);
      return;
    }
    const dados = await produtosApi.listar(termo);
    setResultados(dados);
  }

  function adicionarAoCarrinho(produto: Produto) {
    setCarrinho((atual) => {
      const existente = atual.find((l) => l.produto.id === produto.id);
      if (existente) {
        return atual.map((l) => (l.produto.id === produto.id ? { ...l, quantidade: l.quantidade + 1 } : l));
      }
      return [...atual, { produto, quantidade: 1 }];
    });
    setBusca("");
    setResultados([]);
    inputRef.current?.focus();
  }

  function alterarQuantidade(produtoId: string, quantidade: number) {
    setCarrinho((atual) =>
      quantidade <= 0
        ? atual.filter((l) => l.produto.id !== produtoId)
        : atual.map((l) => (l.produto.id === produtoId ? { ...l, quantidade } : l))
    );
  }

  const total = carrinho.reduce((soma, l) => soma + l.produto.pmc * l.quantidade, 0);

  async function finalizarVenda() {
    if (carrinho.length === 0) return;
    setMensagem(null);
    try {
      await vendasApi.criar(
        carrinho.map((l) => ({ produtoId: l.produto.id, quantidade: l.quantidade })),
        formaPagamento
      );
      setCarrinho([]);
      setMensagem("Venda concluída com sucesso!");
    } catch {
      setMensagem("Não foi possível concluir a venda (verifique o estoque).");
    }
  }

  return (
    <main className="flex flex-1 flex-col gap-4 p-6 md:flex-row md:p-8">
      <section className="flex-1">
        <h1 className="mb-4 text-2xl font-semibold">PDV</h1>

        <div className="relative">
          <input
            ref={inputRef}
            value={busca}
            onChange={(e) => handleBusca(e.target.value)}
            placeholder="Buscar produto por nome ou código de barras..."
            className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-4 py-3 text-lg outline-none focus:border-[var(--color-primary)]"
            autoFocus
          />
          {resultados.length > 0 && (
            <ul className="absolute z-10 mt-1 w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] bg-[var(--color-card)] shadow-[var(--shadow-card)]">
              {resultados.map((p) => (
                <li key={p.id}>
                  <button
                    onClick={() => adicionarAoCarrinho(p)}
                    className="flex w-full justify-between px-4 py-2 text-left text-sm hover:bg-[var(--color-muted)]"
                  >
                    <span>{p.nome}</span>
                    <span className="text-[var(--color-muted-foreground)]">R$ {p.pmc.toFixed(2)}</span>
                  </button>
                </li>
              ))}
            </ul>
          )}
        </div>

        <div className="mt-6 space-y-2">
          {carrinho.map((l) => (
            <div key={l.produto.id} className="flex items-center justify-between rounded-[var(--radius-sm)] border border-[var(--color-border)] px-4 py-2">
              <span className="flex-1">{l.produto.nome}</span>
              <input
                type="number"
                min={0}
                value={l.quantidade}
                onChange={(e) => alterarQuantidade(l.produto.id, Number(e.target.value))}
                className="w-16 rounded-[var(--radius-sm)] border border-[var(--color-border)] px-2 py-1 text-center text-sm"
              />
              <span className="w-24 text-right font-medium">R$ {(l.produto.pmc * l.quantidade).toFixed(2)}</span>
            </div>
          ))}
          {carrinho.length === 0 && <p className="text-sm text-[var(--color-muted-foreground)]">Carrinho vazio.</p>}
        </div>
      </section>

      <aside className="w-full rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-6 md:w-80">
        <p className="text-sm text-[var(--color-muted-foreground)]">Total</p>
        <p className="mb-4 text-3xl font-semibold">R$ {total.toFixed(2)}</p>

        <label className="mb-1 block text-sm font-medium">Forma de pagamento</label>
        <select
          value={formaPagamento}
          onChange={(e) => setFormaPagamento(Number(e.target.value))}
          className="mb-4 w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm"
        >
          {FORMAS_PAGAMENTO.map((f) => (
            <option key={f.valor} value={f.valor}>{f.label}</option>
          ))}
        </select>

        {mensagem && <p className="mb-4 text-sm">{mensagem}</p>}

        <button
          onClick={finalizarVenda}
          disabled={carrinho.length === 0}
          className="w-full rounded-[var(--radius-sm)] bg-[var(--color-primary)] py-3 font-medium text-[var(--color-primary-foreground)] disabled:opacity-60"
        >
          Finalizar venda
        </button>
      </aside>
    </main>
  );
}
