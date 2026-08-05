import { apiClient } from "@/lib/api-client";

export interface ContaFinanceira {
  id: string;
  tipo: number;
  descricao: string;
  valor: number;
  dataVencimento: string;
  pagaEmUtc?: string | null;
  status: number;
}

export interface FluxoCaixa {
  totalEntradas: number;
  totalSaidas: number;
  saldo: number;
}

export const financeiroApi = {
  async listarContas(): Promise<ContaFinanceira[]> {
    const res = await apiClient.authorizedFetch("/api/v1/financeiro/contas");
    if (!res.ok) throw new Error("Falha ao carregar contas.");
    return res.json();
  },

  async fluxoCaixa(): Promise<FluxoCaixa> {
    const res = await apiClient.authorizedFetch("/api/v1/financeiro/fluxo-caixa");
    if (!res.ok) throw new Error("Falha ao carregar fluxo de caixa.");
    return res.json();
  },

  async baixarConta(id: string) {
    const res = await apiClient.authorizedFetch(`/api/v1/financeiro/contas/${id}/baixar`, { method: "POST" });
    if (!res.ok) throw new Error("Falha ao baixar conta.");
  },
};
