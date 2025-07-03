/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}", // Isso garante que ele leia todos os seus arquivos Vue e TypeScript
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}