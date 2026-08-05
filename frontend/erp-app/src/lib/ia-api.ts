import { apiClient } from "@/lib/api-client";

export interface PrevisaoVendas {
  mediaDiariaUltimos30Dias: number;
  previsaoProximos7Dias: number;
  diasAnalisados: number;
}

export const iaApi = {
  async previsaoVendas(): Promise<PrevisaoVendas> {
    const res = await apiClient.authorizedFetch("/api/v1/ia/previsao-vendas");
    if (!res.ok) throw new Error("Falha ao carregar previsão.");
    return res.json();
  },
};
