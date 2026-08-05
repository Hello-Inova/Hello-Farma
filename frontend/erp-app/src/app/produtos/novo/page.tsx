"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { produtosApi } from "@/lib/produtos-api";

export default function NovoProdutoPage() {
  const router = useRouter();
  const [form, setForm] = useState({
    nome: "",
    ean: "",
    tipoProduto: 1,
    pmc: 0,
    pf: 0,
    controlado: false,
    receitaObrigatoria: false,
    laboratorio: "",
    principioAtivo: "",
  });
  const [salvando, setSalvando] = useState(false);
  const [erro, setErro] = useState<string | null>(null);

  function set<K extends keyof typeof form>(key: K, value: (typeof form)[K]) {
    setForm((f) => ({ ...f, [key]: value }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSalvando(true);
    setErro(null);
    try {
      await produtosApi.criar(form);
      router.push("/produtos");
    } catch {
      setErro("Não foi possível salvar o produto.");
    } finally {
      setSalvando(false);
    }
  }

  return (
    <main className="flex-1 p-6 md:p-8">
      <h1 className="mb-6 text-2xl font-semibold">Novo produto</h1>

      <form onSubmit={handleSubmit} className="max-w-xl space-y-4 rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-6">
        <div>
          <label className="mb-1 block text-sm font-medium">Nome</label>
          <input required value={form.nome} onChange={(e) => set("nome", e.target.value)}
            className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm" />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="mb-1 block text-sm font-medium">EAN</label>
            <input required value={form.ean} onChange={(e) => set("ean", e.target.value)}
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm" />
          </div>
          <div>
            <label className="mb-1 block text-sm font-medium">Tipo</label>
            <select value={form.tipoProduto} onChange={(e) => set("tipoProduto", Number(e.target.value))}
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm">
              <option value={1}>Genérico</option>
              <option value={2}>Similar</option>
              <option value={3}>Referência</option>
              <option value={9}>Outro</option>
            </select>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="mb-1 block text-sm font-medium">Laboratório</label>
            <input value={form.laboratorio} onChange={(e) => set("laboratorio", e.target.value)}
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm" />
          </div>
          <div>
            <label className="mb-1 block text-sm font-medium">Princípio ativo</label>
            <input value={form.principioAtivo} onChange={(e) => set("principioAtivo", e.target.value)}
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm" />
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="mb-1 block text-sm font-medium">PMC (R$)</label>
            <input type="number" step="0.01" required value={form.pmc}
              onChange={(e) => set("pmc", Number(e.target.value))}
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm" />
          </div>
          <div>
            <label className="mb-1 block text-sm font-medium">PF (R$)</label>
            <input type="number" step="0.01" required value={form.pf}
              onChange={(e) => set("pf", Number(e.target.value))}
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm" />
          </div>
        </div>

        <div className="flex gap-6">
          <label className="flex items-center gap-2 text-sm">
            <input type="checkbox" checked={form.controlado} onChange={(e) => set("controlado", e.target.checked)} />
            Medicamento controlado
          </label>
          <label className="flex items-center gap-2 text-sm">
            <input type="checkbox" checked={form.receitaObrigatoria} onChange={(e) => set("receitaObrigatoria", e.target.checked)} />
            Receita obrigatória
          </label>
        </div>

        {erro && <p className="text-sm text-[var(--color-danger)]">{erro}</p>}

        <button type="submit" disabled={salvando}
          className="rounded-[var(--radius-sm)] bg-[var(--color-primary)] px-5 py-2 text-sm font-medium text-[var(--color-primary-foreground)] disabled:opacity-60">
          {salvando ? "Salvando..." : "Salvar produto"}
        </button>
      </form>
    </main>
  );
}
