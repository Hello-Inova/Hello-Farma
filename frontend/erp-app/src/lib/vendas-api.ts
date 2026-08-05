import { apiClient } from "@/lib/api-client";

export interface ItemCarrinho {
  produtoId: string;
  quantidade: number;
}

export interface ItemVenda {
  produtoId: string;
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
}

export interface Venda {
  id: string;
  realizadaEmUtc: string;
  formaPagamento: number;
  status: number;
  clienteId?: string | null;
  valorTotal: number;
  cashbackUtilizado: number;
  cashbackGerado: number;
  valorPago: number;
  itens: ItemVenda[];
}

export const STATUS_VENDA_LABEL: Record<number, string> = {
  1: "Finalizada",
  2: "Cancelada",
  3: "Parcialmente devolvida",
  4: "Devolvida",
};

export const vendasApi = {
  async criar(
    itens: ItemCarrinho[],
    formaPagamento: number,
    clienteId?: string,
    cashbackUtilizado?: number
  ) {
    const res = await apiClient.authorizedFetch("/api/v1/vendas", {
      method: "POST",
      body: JSON.stringify({ itens, formaPagamento, clienteId, cashbackUtilizado: cashbackUtilizado ?? 0 }),
    });
    if (!res.ok) throw new Error("Não foi possível concluir a venda.");
    return res.json();
  },

  async listarDoDia(): Promise<Venda[]> {
    const res = await apiClient.authorizedFetch("/api/v1/vendas/hoje");
    if (!res.ok) throw new Error("Falha ao carregar vendas do dia.");
    return res.json();
  },

  async devolver(vendaId: string, itens: ItemCarrinho[], motivo?: string) {
    const res = await apiClient.authorizedFetch(`/api/v1/vendas/${vendaId}/devolucoes`, {
      method: "POST",
      body: JSON.stringify({ itens, motivo }),
    });
    if (!res.ok) throw new Error("Não foi possível registrar a devolução.");
    return res.json();
  },
};
