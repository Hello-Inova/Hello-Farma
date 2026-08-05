import { apiClient } from "@/lib/api-client";

export interface PedidoDelivery {
  id: string;
  vendaId: string;
  enderecoEntrega: string;
  status: number;
  entregadorId?: string | null;
  avaliacaoNota?: number | null;
}

export const STATUS_DELIVERY_LABEL: Record<number, string> = {
  1: "Pendente",
  2: "Separação",
  3: "Expedição",
  4: "Em rota",
  5: "Entregue",
  6: "Avaliado",
  9: "Cancelado",
};

export const deliveryApi = {
  async listar(): Promise<PedidoDelivery[]> {
    const res = await apiClient.authorizedFetch("/api/v1/delivery/pedidos");
    if (!res.ok) throw new Error("Falha ao carregar pedidos.");
    return res.json();
  },

  async avancarStatus(id: string, novoStatus: number) {
    const res = await apiClient.authorizedFetch(`/api/v1/delivery/pedidos/${id}/status`, {
      method: "POST",
      body: JSON.stringify({ novoStatus }),
    });
    if (!res.ok) throw new Error("Falha ao avançar status.");
  },
};
