"use client";

import { useEffect, useState } from "react";
import { plataformaApi, STATUS_ASSINATURA_LABEL, type TenantPlataforma } from "@/lib/plataforma-api";

export default function PlataformaPage() {
  const [tenants, setTenants] = useState<TenantPlataforma[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  async function carregar() {
    setCarregando(true);
    setErro(null);
    try {
      const dados = await plataformaApi.listarTenants();
      setTenants(dados);
    } catch {
      setErro("Não foi possível carregar as farmácias (acesso restrito a SuperAdmin da Hello Inova).");
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    carregar();
  }, []);

  async function alternarStatus(tenant: TenantPlataforma) {
    const acao = tenant.ativo ? "suspender" : "reativar";
    if (!confirm(`Confirma ${acao} o acesso de "${tenant.nomeFantasia}"?`)) return;
    try {
      if (tenant.ativo) await plataformaApi.suspender(tenant.id);
      else await plataformaApi.ativar(tenant.id);
      await carregar();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Falha ao alterar status.");
    }
  }

  const totalAtivos = tenants.filter((t) => t.ativo).length;

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Hello Platform</h1>
        <p className="text-[var(--color-muted-foreground)]">
          Painel administrativo da Hello Inova — {tenants.length} farmácia(s) cadastradas, {totalAtivos} ativa(s)
        </p>
      </header>

      {erro && <p className="mb-4 text-sm text-[var(--color-danger)]">{erro}</p>}
      {carregando && <p className="text-sm text-[var(--color-muted-foreground)]">Carregando...</p>}

      {!carregando && !erro && (
        <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
          <table className="w-full text-left text-sm">
            <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
              <tr>
                <th className="px-4 py-3">Farmácia</th>
                <th className="px-4 py-3">CNPJ</th>
                <th className="px-4 py-3">Plano</th>
                <th className="px-4 py-3">Assinatura</th>
                <th className="px-4 py-3">Usuários</th>
                <th className="px-4 py-3">Cadastrada em</th>
                <th className="px-4 py-3">Status</th>
                <th className="px-4 py-3" />
              </tr>
            </thead>
            <tbody>
              {tenants.length === 0 && (
                <tr><td colSpan={8} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Nenhuma farmácia cadastrada ainda.</td></tr>
              )}
              {tenants.map((t) => (
                <tr key={t.id} className="border-b border-[var(--color-border)] last:border-0">
                  <td className="px-4 py-3 font-medium">
                    {t.nomeFantasia}
                    <div className="text-xs text-[var(--color-muted-foreground)]">{t.razaoSocial}</div>
                  </td>
                  <td className="px-4 py-3">{t.cnpj}</td>
                  <td className="px-4 py-3">{t.planoNome ?? "-"}</td>
                  <td className="px-4 py-3">
                    {t.statusAssinatura != null ? STATUS_ASSINATURA_LABEL[t.statusAssinatura] ?? "-" : "Sem assinatura"}
                  </td>
                  <td className="px-4 py-3">{t.usuariosAtivos}</td>
                  <td className="px-4 py-3">{new Date(t.createdAtUtc).toLocaleDateString("pt-BR")}</td>
                  <td className="px-4 py-3">
                    <span className={t.ativo ? "text-[var(--color-primary)]" : "text-[var(--color-danger)]"}>
                      {t.ativo ? "Ativa" : "Suspensa"}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-right">
                    <button onClick={() => alternarStatus(t)} className="text-xs text-[var(--color-danger)]">
                      {t.ativo ? "Suspender" : "Reativar"}
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </main>
  );
}
