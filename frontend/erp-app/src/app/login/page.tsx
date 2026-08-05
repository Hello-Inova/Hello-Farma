"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { apiClient } from "@/lib/api-client";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [erro, setErro] = useState<string | null>(null);
  const [carregando, setCarregando] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);
    setCarregando(true);
    try {
      await apiClient.login(email, senha);
      router.push("/");
    } catch {
      setErro("E-mail ou senha inválidos.");
    } finally {
      setCarregando(false);
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center bg-[var(--color-muted)] px-4">
      <form
        onSubmit={handleSubmit}
        className="w-full max-w-sm rounded-[var(--radius-lg)] border border-[var(--color-border)] bg-[var(--color-card)] p-8 shadow-[var(--shadow-card)]"
      >
        <h1 className="mb-1 text-xl font-semibold text-[var(--color-card-foreground)]">Hello Farma</h1>
        <p className="mb-6 text-sm text-[var(--color-muted-foreground)]">Acesse sua farmácia</p>

        <label className="mb-1 block text-sm font-medium">E-mail</label>
        <input
          type="email"
          required
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="mb-4 w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 outline-none focus:border-[var(--color-primary)]"
          placeholder="voce@farmacia.com.br"
        />

        <label className="mb-1 block text-sm font-medium">Senha</label>
        <input
          type="password"
          required
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
          className="mb-4 w-full rounded-[var(--radius-sm)] border border-[var(--color-border)] px-3 py-2 outline-none focus:border-[var(--color-primary)]"
          placeholder="••••••••"
        />

        {erro && <p className="mb-4 text-sm text-[var(--color-danger)]">{erro}</p>}

        <button
          type="submit"
          disabled={carregando}
          className="w-full rounded-[var(--radius-sm)] bg-[var(--color-primary)] py-2 font-medium text-[var(--color-primary-foreground)] disabled:opacity-60"
        >
          {carregando ? "Entrando..." : "Entrar"}
        </button>
      </form>
    </main>
  );
}
