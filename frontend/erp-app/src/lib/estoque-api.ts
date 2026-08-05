import { apiClient } from "@/lib/api-client";
import type { Lote } from "@/types/estoque";

export const estoqueApi = {
  async lotesProximosVencimento(diasAlerta = 90): Promise<Lote[]> {
    const res = await apiClient.authorizedFetch(`/api/v1/estoque/lotes/proximos-vencimento?diasAlerta=${diasAlerta}`);
    if (!res.ok) throw new Error("Falha ao carregar lotes.");
    return res.json();
  },

  async registrarEntrada(payload: {
    produtoId: string;
    numeroLote: string;
    validade: string;
    quantidade: number;
    localizacao?: string;
    motivo?: string;
  }) {
    const res = await apiClient.authorizedFetch("/api/v1/estoque/entradas", {
      method: "POST",
      body: JSON.stringify(payload),
    });
    if (!res.ok) throw new Error("Falha ao registrar entrada.");
    return res.json();
  },
};
