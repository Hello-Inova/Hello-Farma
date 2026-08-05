import { apiClient } from "@/lib/api-client";

export interface LogAuditoria {
  id: string;
  usuarioId: string;
  usuarioNome?: string | null;
  acao: string;
  sucesso: boolean;
  erro?: string | null;
  ipAddress?: string | null;
  createdAtUtc: string;
}

export const auditoriaApi = {
  async listar(quantidade = 200): Promise<LogAuditoria[]> {
    const res = await apiClient.authorizedFetch(`/api/v1/auditoria?quantidade=${quantidade}`);
    if (!res.ok) throw new Error("Falha ao carregar auditoria.");
    return res.json();
  },
};
