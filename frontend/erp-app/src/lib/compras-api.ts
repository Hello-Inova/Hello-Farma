import { apiClient } from "@/lib/api-client";
import type { PedidoCompra } from "@/types/compra";

export const comprasApi = {
  async listarPedidos(): Promise<PedidoCompra[]> {
    const res = await apiClient.authorizedFetch("/api/v1/compras/pedidos");
    if (!res.ok) throw new Error("Falha ao carregar pedidos de compra.");
    return res.json();
  },

  async receberPedido(id: string) {
    const res = await apiClient.authorizedFetch(`/api/v1/compras/pedidos/${id}/receber`, { method: "POST" });
    if (!res.ok) throw new Error("Falha ao confirmar recebimento.");
  },
};
