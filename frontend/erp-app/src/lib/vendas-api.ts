import { apiClient } from "@/lib/api-client";

export interface ItemCarrinho {
  produtoId: string;
  quantidade: number;
}

export const vendasApi = {
  async criar(itens: ItemCarrinho[], formaPagamento: number, clienteId?: string) {
    const res = await apiClient.authorizedFetch("/api/v1/vendas", {
      method: "POST",
      body: JSON.stringify({ itens, formaPagamento, clienteId }),
    });
    if (!res.ok) throw new Error("Não foi possível concluir a venda.");
    return res.json();
  },
};
