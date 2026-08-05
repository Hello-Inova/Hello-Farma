/**
 * Design Tokens tipados do Theme Engine.
 * Cada tenant (farmácia) pode sobrescrever estes valores dinamicamente
 * (marca, cores, tipografia, layout), preservando contraste e acessibilidade WCAG AA.
 */
export interface ThemeTokens {
  brand: {
    nome: string;
    logoUrl?: string;
    faviconUrl?: string;
  };
  colors: {
    primary: string;
    secondary: string;
    background: string;
    foreground: string;
    danger: string;
    success: string;
    warning: string;
  };
  typography: {
    fontFamily: string;
    baseSize: string;
  };
  layout: {
    mode: "light" | "dark" | "auto";
    density: "compacto" | "confortavel" | "amplo";
    radius: "sm" | "md" | "lg";
  };
}

export const defaultTheme: ThemeTokens = {
  brand: { nome: "Hello Farma" },
  colors: {
    primary: "#0f766e",
    secondary: "#f97316",
    background: "#ffffff",
    foreground: "#0f172a",
    danger: "#dc2626",
    success: "#16a34a",
    warning: "#d97706",
  },
  typography: { fontFamily: "Inter, sans-serif", baseSize: "16px" },
  layout: { mode: "auto", density: "confortavel", radius: "md" },
};
