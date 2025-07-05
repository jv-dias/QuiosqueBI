# QuiosqueBI - Análise de Dados com IA

**⚠️ Importante:** Este é um projeto de **portfólio** criado para demonstrar habilidades em desenvolvimento Full-Stack com .NET e Vue.js, e a integração com APIs de Inteligência Artificial para análise de dados.

O QuiosqueBI é uma aplicação web que permite a usuários fazerem upload de um arquivo de dados (CSV) e, através de um simples comando em linguagem natural, receberem análises e visualizações gráficas geradas dinamicamente.

---

## Conceito Principal: A Magia da IA

O grande diferencial deste projeto é como ele interpreta a necessidade do usuário. O fluxo é o seguinte:

1.  **Upload e Contexto:** O usuário envia um arquivo CSV e descreve o que deseja analisar (ex: "Quais foram os produtos mais vendidos por cidade?").
2.  **Inteligência Artificial em Ação:** O backend em .NET não possui regras de negócio fixas. Em vez disso, ele envia os cabeçalhos do arquivo e o objetivo do usuário para a **API do Google Gemini**.
3.  **Plano de Análise Dinâmico:** A IA do Gemini interpreta a solicitação e retorna um "plano de análise" em formato JSON, sugerindo quais colunas devem ser cruzadas (dimensões e métricas) e qual o melhor tipo de gráfico para cada caso.
4.  **Processamento e Visualização:** O backend executa o plano de análise recebido, processa os dados e envia os resultados para o frontend em Vue.js, que renderiza os gráficos para o usuário.

Em resumo, o QuiosqueBI usa uma IA generativa como um "motor de análise" dinâmico, capaz de se adaptar a diferentes conjuntos de dados sem precisar de programação prévia para cada cenário.

---

## 🚀 Stack de Tecnologias

* **Backend:** API RESTful com .NET 8
* **Frontend:** Single Page Application (SPA) com Vue 3 (Composition API) + Vite
* **Inteligência Artificial:** Google Gemini API
* **Estilização:** Tailwind CSS
* **Linguagens:** C#, TypeScript

---

## 📋 Pré-requisitos

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [Node.js](https://nodejs.org/) (v18 ou superior)
* [npm](https://www.npmjs.com/) ou [yarn](https://yarnpkg.com/)

---

## ⚙️ Configuração Essencial

Para que a integração com a IA funcione, você precisa da sua chave de API do Google Gemini.

1.  Navegue até a pasta do backend: `cd backend`
2.  Crie um arquivo chamado `appsettings.Development.json`.
3.  Adicione sua chave de API dentro dele, como no exemplo abaixo:

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
2.  **Restaure os pacotes e execute:**
    ```sh
    dotnet restore
    dotnet run
    ```
3.  A API estará disponível em `http://localhost:5159` (verifique o arquivo `Properties/launchSettings.json` para as portas exatas).

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

## 🗺️ Roadmap Futuro

Abaixo estão algumas funcionalidades e melhorias planejadas para futuras versões do QuiosqueBI:

* **Suporte a Múltiplos Formatos de Arquivo:**
    * Implementar a capacidade de analisar arquivos `.xlsx` (Excel), além do `.csv` atual, tornando a ferramenta mais versátil para diferentes usuários.

* **Persistência de Dados e Histórico de Análises:**
    * Integrar um banco de dados para salvar as análises geradas.
    * Criar uma área onde o usuário possa ver e revisitar seu histórico de análises.

* **Autenticação e Autorização de Usuários:**
    * Implementar um sistema de login/cadastro (provavelmente com JWT) para que cada usuário tenha seu próprio espaço de trabalho e histórico privado.

* **Implantação e Acesso Público (Deployment):**
    * Configurar pipelines de CI/CD (Continuous Integration/Continuous Deployment) para automatizar o processo de build e deploy.
    * Hospedar a aplicação em uma plataforma de nuvem (como Azure, AWS ou Vercel/Render) para torná-la acessível publicamente.

---

## 📜 Licença

Este projeto é de código aberto para fins educacionais e de portfólio, sob a licença MIT. Sinta-se à vontade para clonar, estudar e modificar o código.