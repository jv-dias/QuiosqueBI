# QuiosqueBI - Frontend (Vue.js 3)

![Vue.js](https://img.shields.io/badge/vuejs-%2335495e.svg?style=for-the-badge&logo=vuedotjs&logoColor=%234FC08D)
![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white)
![Pinia](https://img.shields.io/badge/pinia-%23FFB217.svg?style=for-the-badge&logo=vue.js&logoColor=white)
![ECharts](https://img.shields.io/badge/echarts-AA3435?style=for-the-badge&logo=apacheecharts&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/tailwindcss-%2338B2AC.svg?style=for-the-badge&logo=tailwind-css&logoColor=white)
![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)

Este repositório contém a interface de usuário (UI) para a aplicação **QuiosqueBI**. É uma Single Page Application (SPA) moderna, reativa e responsiva, construída com **Vue.js 3** e **TypeScript**, projetada para oferecer uma experiência de usuário fluida e intuitiva desde o registro até a visualização de análises complexas.

<div align="center">
  <a href="https://victorious-dune-05e42d21e.1.azurestaticapps.net" target="_blank">
    <img src="https://img.shields.io/badge/Ver_Aplicação_no_Ar-0078D4?style=for-the-badge&logo=azure-static-web-apps&logoColor=white" alt="Link para a aplicação" />
  </a>
</div>

---

> ### ⚙️ **Backend API (.NET)**
> Toda a lógica de negócio, integração com IA e gerenciamento de banco de dados é feita por uma API .NET que está em um repositório separado.
> **[Acesse o repositório do Backend aqui](https://github.com/jv-dias/QuiosqueBI-Backend)**

---

## ✨ Funcionalidades Principais

* **Interface Reativa e Moderna:** Construída com Vue 3 (Composition API) e estilizada com Tailwind CSS para um design limpo e totalmente responsivo.
* **Sistema de Autenticação Completo:** Telas de Registro e Login com design profissional, gerenciamento de estado via Pinia e proteção de rotas para usuários autenticados.
* **Painel de Análise Interativo:** Interface para upload de arquivos (CSV/XLSX) e submissão de objetivos de análise em linguagem natural.
* **Visualização de Dados Avançada:** Renderização de múltiplos tipos de gráficos (barras, linhas, pizza, funil) com a biblioteca de alta performance **ECharts**, incluindo ferramentas de interatividade como zoom e scroll para grandes volumes de dados.
* **Histórico de Análises Pessoal:** Uma página protegida onde o usuário pode ver e revisitar todas as suas análises salvas anteriormente, com navegação para a visualização detalhada.

## 🚀 Stack de Tecnologias

* **Framework:** Vue.js 3 (Composition API)
* **Linguagem:** TypeScript
* **Build Tool:** Vite
* **Gerenciamento de Estado:** Pinia
* **Roteamento:** Vue Router
* **Chamadas HTTP:** Axios
* **Gráficos:** Apache ECharts (via `vue-echarts`)
* **Estilização:** Tailwind CSS

## ⚙️ Configuração do Ambiente Local

Para rodar o frontend localmente, você precisará do Node.js (v18+) e de um gerenciador de pacotes (npm, yarn, etc.).

1.  **Clone o repositório:**
    ```sh
    git clone [https://github.com/jv-dias/QuiosqueBI.git](https://github.com/jv-dias/QuiosqueBI.git)
    cd QuiosqueBI
    ```

2.  **Instale as dependências:**
    ```sh
    npm install
    ```

3.  **Configure a URL da API:**
    Para que o frontend saiba onde encontrar o backend localmente, crie um arquivo chamado `.env.local` na raiz da pasta `frontend`. Este arquivo é ignorado pelo Git. Adicione a seguinte linha:
    ```
    VITE_API_URL=http://localhost:5159/api
    ```
    *Nota: Garanta que o [backend do QuiosqueBI](https://github.com/jv-dias/QuiosqueBI-Backend) esteja rodando neste endereço.*

4.  **Execute o Servidor de Desenvolvimento:**
    ```sh
    npm run dev
    ```
    A aplicação estará disponível em `http://localhost:5173`.

## ✨ Destaques da Arquitetura

* **Gerenciamento de Estado com Pinia:** A store de autenticação (`authStore`) gerencia o token JWT, o estado do usuário e a lógica de login/logout de forma centralizada e segura.
* **Roteamento Seguro:** Utilização de `meta` fields e Navigation Guards do Vue Router para proteger rotas que exigem autenticação, redirecionando usuários não autorizados para a página de login.
* **Componentização:** A aplicação é construída com componentes reutilizáveis e bem definidos (`AuthForm`, `GraficoAnalise`, `NavBar`), promovendo a manutenibilidade e consistência do código.
* **CI/CD com GitHub Actions:** O projeto está configurado para deploy contínuo no **Azure Static Web Apps**. Cada `push` para a branch `main` dispara um workflow que automaticamente faz o build e a implantação da nova versão do site.

## 📜 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE.md) para mais detalhes.