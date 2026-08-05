"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { produtosApi } from "@/lib/produtos-api";
import { TIPO_PRODUTO_LABEL, type Produto } from "@/types/produto";

export default function ProdutosPage() {
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [busca, setBusca] = useState("");
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  async function carregar(termo?: string) {
    setCarregando(true);
    setErro(null);
    try {
      const dados = await produtosApi.listar(termo);
      setProdutos(dados);
    } catch {
      setErro("Não foi possível carregar os produtos.");
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  async function handleDesativar(id: string) {
    if (!confirm("Desativar este produto?")) return;
    await produtosApi.desativar(id);
    carregar(busca);
  }

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6 flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold">Produtos</h1>
          <p className="text-[var(--color-muted-foreground)]">Catálogo de produtos farmacêuticos</p>
        </div>
        <Link
          href="/produtos/novo"
          className="rounded-[var(--radius-sm)] bg-[var(--color-primary)] px-4 py-2 text-sm font-medium text-[var(--color-primary-foreground)]"
        >
          + Novo produto
        </Link>
      </header>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          carregar(busca);
        }}
        className="mb-4 flex gap-2"
      >
        <input
          value={busca}
          onChange={(e) => setBusca(e.target.value)}
          placeholder="Buscar por nome ou EAN..."
          className="w-full max-w-sm rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm outline-none focus:border-[var(--color-primary)]"
        />
        <button
          type="submit"
          className="rounded-[var(--radius-sm)] border border-[var(--color-border)] px-4 py-2 text-sm"
        >
          Buscar
        </button>
      </form>

      {erro && <p className="mb-4 text-sm text-[var(--color-danger)]">{erro}</p>}

      <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
            <tr>
              <th className="px-4 py-3">Nome</th>
              <th className="px-4 py-3">EAN</th>
              <th className="px-4 py-3">Tipo</th>
              <th className="px-4 py-3">PMC</th>
              <th className="px-4 py-3">Controlado</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody>
            {carregando && (
              <tr><td colSpan={6} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Carregando...</td></tr>
            )}
            {!carregando && produtos.length === 0 && (
              <tr><td colSpan={6} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Nenhum produto encontrado.</td></tr>
            )}
            {produtos.map((p) => (
              <tr key={p.id} className="border-b border-[var(--color-border)] last:border-0">
                <td className="px-4 py-3 font-medium">{p.nome}</td>
                <td className="px-4 py-3">{p.ean}</td>
                <td className="px-4 py-3">{TIPO_PRODUTO_LABEL[p.tipoProduto] ?? "-"}</td>
                <td className="px-4 py-3">R$ {p.pmc.toFixed(2)}</td>
                <td className="px-4 py-3">
                  {p.controlado && (
                    <span className="rounded-full bg-[var(--color-danger)]/10 px-2 py-0.5 text-xs text-[var(--color-danger)]">
                      Controlado
                    </span>
                  )}
                </td>
                <td className="px-4 py-3 text-right">
                  <button onClick={() => handleDesativar(p.id)} className="text-xs text-[var(--color-danger)]">
                    Desativar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </main>
  );
}
