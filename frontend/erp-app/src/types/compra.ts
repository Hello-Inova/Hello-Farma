export interface ItemPedidoCompra {
  produtoId: string;
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
  subtotal: number;
  numeroLote: string;
  validade: string;
}

export interface PedidoCompra {
  id: string;
  fornecedorId: string;
  status: number;
  valorTotal: number;
  recebidoEmUtc?: string | null;
  itens: ItemPedidoCompra[];
}

export const STATUS_PEDIDO_LABEL: Record<number, string> = {
  1: "Cotação",
  2: "Pedido realizado",
  3: "Recebido",
  4: "Conferido",
  9: "Cancelado",
};
