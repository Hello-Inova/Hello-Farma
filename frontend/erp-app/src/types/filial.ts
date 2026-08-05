export interface Filial {
  id: string;
  nome: string;
  cnpj?: string | null;
  endereco?: string | null;
  cidade?: string | null;
  uf?: string | null;
  ativa: boolean;
  matriz: boolean;
}
