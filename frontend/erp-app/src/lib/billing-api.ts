import { apiClient } from "@/lib/api-client";

export interface Plano {
  id: string;
  nome: string;
  precoMensal: number;
  limiteUsuarios: number;
  limiteFiliais: number;
  limiteProdutos: number;
  permiteDelivery: boolean;
  permiteIA: boolean;
}

export const billingApi = {
  async listarPlanos(): Promise<Plano[]> {
    const res = await apiClient.authorizedFetch("/api/v1/billing/planos");
    if (!res.ok) throw new Error("Falha ao carregar planos.");
    return res.json();
  },
};
