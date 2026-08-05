import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Hello Farma | ERP",
  description: "Plataforma de gestão especializada para farmácias — Hello Inova.",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="pt-BR" className="h-full antialiased">
      <body className="min-h-full flex flex-col bg-[var(--color-background)] text-[var(--color-foreground)]">
        {children}
      </body>
    </html>
  );
}
