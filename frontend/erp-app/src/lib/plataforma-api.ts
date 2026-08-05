import { apiClient } from "@/lib/api-client";

export interface TenantPlataforma {
  id: string;
  razaoSocial: string;
  nomeFantasia: string;
  cnpj: string;
  ativo: boolean;
  planoNome?: string | null;
  statusAssinatura?: number | null;
  usuariosAtivos: number;
  createdAtUtc: string;
}

export const STATUS_ASSINATURA_LABEL: Record<number, string> = {
  1: "Trial",
  2: "Ativa",
  3: "Cancelada",
  4: "Inadimplente",
};

export const plataformaApi = {
  async listarTenants(): Promise<TenantPlataforma[]> {
    const res = await apiClient.authorizedFetch("/api/v1/plataforma/tenants");
    if (!res.ok) throw new Error("Falha ao carregar farmácias da plataforma.");
    return res.json();
  },

  async suspender(tenantId: string): Promise<void> {
    const res = await apiClient.authorizedFetch(`/api/v1/plataforma/tenants/${tenantId}/suspender`, { method: "POST" });
    if (!res.ok) throw new Error("Falha ao suspender farmácia.");
  },

  async ativar(tenantId: string): Promise<void> {
    const res = await apiClient.authorizedFetch(`/api/v1/plataforma/tenants/${tenantId}/ativar`, { method: "POST" });
    if (!res.ok) throw new Error("Falha ao reativar farmácia.");
  },
};
