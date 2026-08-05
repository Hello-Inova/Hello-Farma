export interface Lote {
  id: string;
  produtoId: string;
  produtoNome: string;
  numeroLote: string;
  validade: string;
  quantidadeAtual: number;
  localizacao?: string | null;
  diasParaVencer: number;
}
