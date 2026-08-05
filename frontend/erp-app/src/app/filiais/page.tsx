"use client";

import { useEffect, useState } from "react";
import { filiaisApi } from "@/lib/filiais-api";
import type { Filial } from "@/types/filial";

export default function FiliaisPage() {
  const [filiais, setFiliais] = useState<Filial[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const [mostrarForm, setMostrarForm] = useState(false);
  const [nome, setNome] = useState("");
  const [cnpj, setCnpj] = useState("");
  const [cidade, setCidade] = useState("");
  const [uf, setUf] = useState("");
  const [salvando, setSalvando] = useState(false);

  async function carregar() {
    setCarregando(true);
    setErro(null);
    try {
      const dados = await filiaisApi.listar();
      setFiliais(dados);
    } catch {
      setErro("Não foi possível carregar as filiais.");
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  async function handleCriar(e: React.FormEvent) {
    e.preventDefault();
    setSalvando(true);
    setErro(null);
    try {
      await filiaisApi.criar({ nome, cnpj: cnpj || undefined, cidade: cidade || undefined, uf: uf || undefined });
      setNome("");
      setCnpj("");
      setCidade("");
      setUf("");
      setMostrarForm(false);
      await carregar();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Falha ao criar filial (verifique o limite do seu plano).");
    } finally {
      setSalvando(false);
    }
  }

  async function handleAlternarAtiva(f: Filial) {
    try {
      if (f.ativa) {
        await filiaisApi.desativar(f.id);
      } else {
        await filiaisApi.ativar(f.id);
      }
      await carregar();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Não foi possível alterar o status da filial.");
    }
  }

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6 flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold">Filiais</h1>
          <p className="text-[var(--color-muted-foreground)]">Unidades da rede — estoque e vendas podem ser segmentados por filial</p>
        </div>
        <button
          onClick={() => setMostrarForm((v) => !v)}
          className="rounded-[var(--radius-sm)] bg-[var(--color-primary)] px-4 py-2 text-sm font-medium text-[var(--color-primary-foreground)]"
        >
          {mostrarForm ? "Cancelar" : "+ Nova filial"}
        </button>
      </header>

      {mostrarForm && (
        <form onSubmit={handleCriar} className="mb-6 grid max-w-xl gap-3 rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-4">
          <input
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            placeholder="Nome da filial"
            required
            className="rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm outline-none focus:border-[var(--color-primary)]"
          />
          <input
            value={cnpj}
            onChange={(e) => setCnpj(e.target.value)}
            placeholder="CNPJ (opcional)"
            className="rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm outline-none focus:border-[var(--color-primary)]"
          />
          <div className="flex gap-3">
            <input
              value={cidade}
              onChange={(e) => setCidade(e.target.value)}
              placeholder="Cidade"
              className="w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm outline-none focus:border-[var(--color-primary)]"
            />
            <input
              value={uf}
              onChange={(e) => setUf(e.target.value.toUpperCase())}
              placeholder="UF"
              maxLength={2}
              className="w-24 rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 text-sm outline-none focus:border-[var(--color-primary)]"
            />
          </div>
          <button
            type="submit"
            disabled={salvando}
            className="w-fit rounded-[var(--radius-sm)] bg-[var(--color-primary)] px-4 py-2 text-sm font-medium text-[var(--color-primary-foreground)] disabled:opacity-60"
          >
            {salvando ? "Salvando..." : "Salvar filial"}
          </button>
        </form>
      )}

      {erro && <p className="mb-4 text-sm text-[var(--color-danger)]">{erro}</p>}

      <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
            <tr>
              <th className="px-4 py-3">Nome</th>
              <th className="px-4 py-3">CNPJ</th>
              <th className="px-4 py-3">Cidade/UF</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3" />
            </tr>
          </thead>
          <tbody>
            {carregando && (
              <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Carregando...</td></tr>
            )}
            {!carregando && filiais.length === 0 && (
              <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Nenhuma filial cadastrada.</td></tr>
            )}
            {filiais.map((f) => (
              <tr key={f.id} className="border-b border-[var(--color-border)] last:border-0">
                <td className="px-4 py-3 font-medium">
                  {f.nome} {f.matriz && <span className="ml-2 rounded-full bg-[var(--color-primary)]/10 px-2 py-0.5 text-xs text-[var(--color-primary)]">Matriz</span>}
                </td>
                <td className="px-4 py-3">{f.cnpj ?? "-"}</td>
                <td className="px-4 py-3">{[f.cidade, f.uf].filter(Boolean).join("/") || "-"}</td>
                <td className="px-4 py-3">
                  <span className={f.ativa ? "text-[var(--color-primary)]" : "text-[var(--color-muted-foreground)]"}>
                    {f.ativa ? "Ativa" : "Inativa"}
                  </span>
                </td>
                <td className="px-4 py-3 text-right">
                  {!f.matriz && (
                    <button onClick={() => handleAlternarAtiva(f)} className="text-xs text-[var(--color-danger)]">
                      {f.ativa ? "Desativar" : "Ativar"}
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
