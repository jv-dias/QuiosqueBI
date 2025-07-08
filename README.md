# QuiosqueBI - Análise de Dados com IA

**⚠️ Importante:** Este é um projeto de **portfólio** criado para demonstrar habilidades em desenvolvimento Full-Stack com .NET e Vue.js, e a integração com APIs de Inteligência Artificial para análise de dados.

O QuiosqueBI é uma aplicação web que permite a usuários fazerem upload de um arquivo de dados (CSV/XLSX) e, através de um simples comando em linguagem natural, receberem análises visuais geradas dinamicamente. As análises são salvas, permitindo a criação de um histórico que pode ser revisitado a qualquer momento.

---

## Conceito Principal: A Magia da IA

O grande diferencial deste projeto é como ele interpreta a necessidade do usuário. O fluxo é o seguinte:

1.  **Upload e Contexto:** O usuário envia um arquivo de dados e descreve o que deseja analisar.
2.  **Inteligência Artificial em Ação:** O backend em .NET envia os cabeçalhos do arquivo e o objetivo do usuário para a **API do Google Gemini**.
3.  **Plano de Análise Dinâmico:** A IA interpreta a solicitação e retorna um plano de análise em JSON, sugerindo os melhores gráficos e cruzamentos de dados.
4.  **Processamento e Visualização:** O backend executa o plano, processa os dados e envia os resultados para o frontend em Vue.js, que renderiza os gráficos.
5.  **Persistência e Histórico:** Os resultados da análise são **salvos em um banco de dados PostgreSQL**, permitindo que o usuário acesse seu histórico e reveja análises passadas.

---

## 🚀 Stack de Tecnologias

* **Backend:** API RESTful com **.NET 8**, **Entity Framework Core**
* **Banco de Dados:** **PostgreSQL**
* **Frontend:** Single Page Application (SPA) com **Vue 3** (Composition API) + Vite
* **Inteligência Artificial:** Google Gemini API
* **Estilização:** Tailwind CSS
* **Linguagens:** C#, TypeScript

---

## 📋 Pré-requisitos

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [Node.js](https://nodejs.org/) (v18 ou superior)
* [PostgreSQL](https://www.postgresql.org/download/) com um banco de dados criado (ex: `QuiosqueBI_DB`).

---

## ⚙️ Configuração Essencial

Para a aplicação funcionar corretamente, você precisa configurar a chave da API do Gemini e a conexão com o banco de dados.

1.  Navegue até a pasta do backend: `cd backend`
2.  Crie um arquivo chamado `appsettings.Development.json`.
3.  Adicione sua chave de API e sua string de conexão dentro dele:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "Gemini": {
        "ApiKey": "SUA_CHAVE_API_DO_GEMINI_VAI_AQUI"
      },
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=5432;Database=QuiosqueBI_DB;User Id=postgres;Password=SUA_SENHA_DO_POSTGRES;"
      }
    }
    ```

---

## ⚡ Como Rodar o Projeto

Você precisará de dois terminais abertos, um para o backend e um para o frontend.

### Backend (.NET API)

1.  **Navegue até a pasta:**
    ```sh
    cd backend
    ```
2.  **Restaure os pacotes do .NET:**
    ```sh
    dotnet restore
    ```
3.  **Execute as Migrations do Banco de Dados:** Este comando criará as tabelas necessárias no seu banco PostgreSQL.
    ```sh
    dotnet ef database update
    ```
4.  **Execute a API:**
    ```sh
    dotnet run
    ```

### Frontend (Vue.js App)

1.  **Navegue até a pasta:**
    ```sh
    cd frontend
    ```
2.  **Instale as dependências:**
    ```sh
    npm install
    ```
3.  **Execute o servidor de desenvolvimento:**
    ```sh
    npm run dev
    ```
4.  Acesse a aplicação no seu navegador em `http://localhost:5173`.

---

## ✨ Funcionalidades Principais

* **Análise via IA:** Utiliza a API do Google Gemini para interpretar comandos em linguagem natural e gerar planos de análise.
* **Suporte a Múltiplos Formatos:** Realiza o upload e a leitura otimizada de arquivos `.csv` e `.xlsx`.
* **Processamento de Arquivos Grandes:** Utiliza técnicas de streaming para analisar arquivos com mais de 20.000 linhas com baixo consumo de memória.
* **Persistência e Histórico:** Salva os resultados de cada análise em um banco de dados PostgreSQL e permite que o usuário visualize seu histórico.
* **Visualização de Dados Dinâmica:** Renderiza múltiplos tipos de gráficos (barras, linhas, pizza) de forma interativa no frontend.

---

## 🗺️ Roadmap Futuro

Abaixo estão algumas funcionalidades e melhorias planejadas para futuras versões:

* **Autenticação e Autorização de Usuários:** Implementar um sistema de login/cadastro (JWT) para que cada usuário tenha seu próprio histórico de análises privado.
* **Refinamento da IA:** Melhorar os prompts enviados à IA para permitir análises mais complexas e customizadas.
* **Implantação e Acesso Público (Deployment):** Configurar pipelines de CI/CD para automatizar o deploy e hospedar a aplicação em uma plataforma de nuvem (Azure, AWS, etc.).

---

## 📜 Licença

Este projeto é de código aberto para fins educacionais e de portfólio, sob a licença MIT. Sinta-se à vontade para clonar, estudar e modificar o código.