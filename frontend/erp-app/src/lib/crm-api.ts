import { apiClient } from "@/lib/api-client";

export interface Cliente {
  id: string;
  nome: string;
  cpf?: string | null;
  telefone?: string | null;
  email?: string | null;
  saldoCashback: number;
}

export const crmApi = {
  async listar(busca?: string): Promise<Cliente[]> {
    const query = busca ? `?busca=${encodeURIComponent(busca)}` : "";
    const res = await apiClient.authorizedFetch(`/api/v1/clientes${query}`);
    if (!res.ok) throw new Error("Falha ao carregar clientes.");
    return res.json();
  },
};
