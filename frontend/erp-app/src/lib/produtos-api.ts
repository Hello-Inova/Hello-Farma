import { apiClient } from "@/lib/api-client";
import type { Produto } from "@/types/produto";

export const produtosApi = {
  async listar(busca?: string): Promise<Produto[]> {
    const query = busca ? `?busca=${encodeURIComponent(busca)}` : "";
    const res = await apiClient.authorizedFetch(`/api/v1/produtos${query}`);
    if (!res.ok) throw new Error("Falha ao carregar produtos.");
    return res.json();
  },

  async criar(payload: Omit<Produto, "id" | "ativo">): Promise<{ id: string }> {
    const res = await apiClient.authorizedFetch("/api/v1/produtos", {
      method: "POST",
      body: JSON.stringify(payload),
    });
    if (!res.ok) throw new Error("Falha ao criar produto.");
    return res.json();
  },

  async desativar(id: string): Promise<void> {
    const res = await apiClient.authorizedFetch(`/api/v1/produtos/${id}`, { method: "DELETE" });
    if (!res.ok) throw new Error("Falha ao desativar produto.");
  },
};
