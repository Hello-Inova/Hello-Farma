export interface Produto {
  id: string;
  nome: string;
  ean: string;
  registroAnvisa?: string | null;
  laboratorio?: string | null;
  principioAtivo?: string | null;
  categoriaTerapeutica?: string | null;
  formaFarmaceutica?: string | null;
  concentracao?: string | null;
  tipoProduto: number;
  controlado: boolean;
  receitaObrigatoria: boolean;
  pmc: number;
  pf: number;
  ativo: boolean;
}

export const TIPO_PRODUTO_LABEL: Record<number, string> = {
  1: "Genérico",
  2: "Similar",
  3: "Referência",
  9: "Outro",
};
