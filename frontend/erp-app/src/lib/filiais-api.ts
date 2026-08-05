import { apiClient } from "@/lib/api-client";
import type { Filial } from "@/types/filial";

export const filiaisApi = {
  async listar(): Promise<Filial[]> {
    const res = await apiClient.authorizedFetch("/api/v1/filiais");
    if (!res.ok) throw new Error("Falha ao carregar filiais.");
    return res.json();
  },

  async criar(payload: {
    nome: string;
    cnpj?: string;
    endereco?: string;
    cidade?: string;
    uf?: string;
  }): Promise<{ id: string }> {
    const res = await apiClient.authorizedFetch("/api/v1/filiais", {
      method: "POST",
      body: JSON.stringify(payload),
    });
    if (!res.ok) throw new Error("Falha ao criar filial.");
    return res.json();
  },

  async desativar(id: string): Promise<void> {
    const res = await apiClient.authorizedFetch(`/api/v1/filiais/${id}/desativar`, { method: "POST" });
    if (!res.ok) throw new Error("Falha ao desativar filial.");
  },

  async ativar(id: string): Promise<void> {
    const res = await apiClient.authorizedFetch(`/api/v1/filiais/${id}/ativar`, { method: "POST" });
    if (!res.ok) throw new Error("Falha ao ativar filial.");
  },
};
