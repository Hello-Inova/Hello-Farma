interface KpiCardProps {
  titulo: string;
  valor: string;
  variacao?: string;
  tendencia?: "alta" | "baixa" | "neutra";
}

/**
 * Card de indicador reutilizável para o dashboard do ERP.
 * Usa exclusivamente os Design Tokens do Theme Engine (nunca cores fixas).
 */
export function KpiCard({ titulo, valor, variacao, tendencia = "neutra" }: KpiCardProps) {
  const corTendencia =
    tendencia === "alta" ? "text-[var(--color-success)]" :
    tendencia === "baixa" ? "text-[var(--color-danger)]" :
    "text-[var(--color-muted-foreground)]";

  return (
    <div className="rounded-[var(--radius-md)] border border-[var(--color-border)] bg-[var(--color-card)] p-4 shadow-[var(--shadow-card)]">
      <p className="text-sm text-[var(--color-muted-foreground)]">{titulo}</p>
      <p className="mt-1 text-2xl font-semibold text-[var(--color-card-foreground)]">{valor}</p>
      {variacao && <p className={`mt-1 text-xs ${corTendencia}`}>{variacao}</p>}
    </div>
  );
}
