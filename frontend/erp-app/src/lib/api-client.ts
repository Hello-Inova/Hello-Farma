const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

const TOKEN_KEY = "hellofarma:accessToken";
const REFRESH_KEY = "hellofarma:refreshToken";

export interface LoginResponse {
  accessToken: string;
  expiraEmUtc: string;
  refreshToken: string;
  nomeUsuario: string;
  papel: string;
  tenantId: string;
}

/**
 * Client HTTP simples para a API do Hello Farma. Guarda o access/refresh token
 * em memória de sessão (sessionStorage) — nunca em localStorage por período indefinido,
 * seguindo a diretriz de segurança do master prompt.
 */
export const apiClient = {
  async login(email: string, senha: string): Promise<LoginResponse> {
    const res = await fetch(`${API_BASE_URL}/api/v1/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, senha }),
    });

    if (!res.ok) {
      throw new Error("E-mail ou senha inválidos.");
    }

    const data: LoginResponse = await res.json();
    sessionStorage.setItem(TOKEN_KEY, data.accessToken);
    sessionStorage.setItem(REFRESH_KEY, data.refreshToken);
    return data;
  },

  logout() {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(REFRESH_KEY);
  },

  getToken(): string | null {
    if (typeof window === "undefined") return null;
    return sessionStorage.getItem(TOKEN_KEY);
  },

  async authorizedFetch(path: string, init: RequestInit = {}) {
    const token = this.getToken();
    return fetch(`${API_BASE_URL}${path}`, {
      ...init,
      headers: {
        ...init.headers,
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
    });
  },
};
