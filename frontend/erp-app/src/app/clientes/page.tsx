"use client";

import { useEffect, useState } from "react";
import { crmApi, type Cliente } from "@/lib/crm-api";

export default function ClientesPage() {
  const [clientes, setClientes] = useState<Cliente[]>([]);

  useEffect(() => {
    crmApi.listar().then(setClientes);
  }, []);

  return (
    <main className="flex-1 p-6 md:p-8">
      <header className="mb-6">
        <h1 className="text-2xl font-semibold">Clientes</h1>
        <p className="text-[var(--color-muted-foreground)]">CRM — histórico e cashback</p>
      </header>

      <div className="overflow-x-auto rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)]">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-[var(--color-border)] text-[var(--color-muted-foreground)]">
            <tr>
              <th className="px-4 py-3">Nome</th>
              <th className="px-4 py-3">CPF</th>
              <th className="px-4 py-3">Telefone</th>
              <th className="px-4 py-3">Cashback</th>
            </tr>
          </thead>
          <tbody>
            {clientes.map((c) => (
              <tr key={c.id} className="border-b border-[var(--color-border)] last:border-0">
                <td className="px-4 py-3 font-medium">{c.nome}</td>
                <td className="px-4 py-3">{c.cpf ?? "-"}</td>
                <td className="px-4 py-3">{c.telefone ?? "-"}</td>
                <td className="px-4 py-3">R$ {c.saldoCashback.toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </main>
  );
}
