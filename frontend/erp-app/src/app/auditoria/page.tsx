"use client";

import { useEffect, useState } from "react";
import { auditoriaApi, type LogAuditoria } from "@/lib/auditoria-api";

export default function AuditoriaPage() {
  const [logs, setLogs] = useState<LogAuditoria[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  useEffect(() => {
    auditoriaApi
      .listar()
      .then(setLogs)
      .catch(() => setErro("Não foi possível carregar o log de auditoria (apenas administradores têm acesso)."))
      .finally(() => setCarregando(false));
  }, []);

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Auditoria</h1>
        <p className="text-[var(--color-muted-foreground)]">Registro de quem fez cada ação no sistema</p>
      </header>

      {erro && <p className="mb-4 text-sm text-[var(--color-danger)]">{erro}</p>}
      {carregando && <p className="text-sm text-[var(--color-muted-foreground)]">Carregando...</p>}

      {!carregando && !erro && (
        <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
          <table className="w-full text-left text-sm">
            <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
              <tr>
                <th className="px-4 py-3">Quando</th>
                <th className="px-4 py-3">Usuário</th>
                <th className="px-4 py-3">Ação</th>
                <th className="px-4 py-3">IP</th>
                <th className="px-4 py-3">Status</th>
              </tr>
            </thead>
            <tbody>
              {logs.length === 0 && (
                <tr><td colSpan={5} className="px-4 py-6 text-center text-[var(--color-muted-foreground)]">Nenhum registro ainda.</td></tr>
              )}
              {logs.map((log) => (
                <tr key={log.id} className="border-b border-[var(--color-border)] last:border-0">
                  <td className="px-4 py-3 whitespace-nowrap">{new Date(log.createdAtUtc).toLocaleString("pt-BR")}</td>
                  <td className="px-4 py-3">{log.usuarioNome ?? log.usuarioId}</td>
                  <td className="px-4 py-3 font-mono text-xs">{log.acao}</td>
                  <td className="px-4 py-3">{log.ipAddress ?? "-"}</td>
                  <td className="px-4 py-3">
                    <span className={log.sucesso ? "text-[var(--color-primary)]" : "text-[var(--color-danger)]"}>
                      {log.sucesso ? "Sucesso" : "Falha"}
                    </span>
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
