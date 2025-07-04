# QuiosqueBI

Este projeto é composto por um backend em .NET e um frontend em Vue.js, destinado à análise e visualização de dados de vendas.

## Estrutura do Projeto

- **backend/**: API desenvolvida em .NET para processamento e análise dos dados.
- **frontend/**: Aplicação web desenvolvida em Vue.js para visualização e interação com os dados.

## Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (recomendado: versão 18 ou superior)
- [npm](https://www.npmjs.com/) ou [yarn](https://yarnpkg.com/)

## Como rodar o projeto

### Backend

1. Acesse a pasta `backend`:
   ```sh
   cd backend
   ```
2. Restaure os pacotes e execute a API:
   ```sh
   dotnet restore
   dotnet run
   ```
3. A API estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

### Frontend

1. Acesse a pasta `frontend`:
   ```sh
   cd frontend
   ```
2. Instale as dependências:
   ```sh
   npm install
   # ou
   yarn install
   ```
3. Execute o servidor de desenvolvimento:
   ```sh
   npm run dev
   # ou
   yarn dev
   ```
4. Acesse a aplicação em `http://localhost:5173` (ou porta configurada pelo Vite).

## Portas Padrão

- **Backend (.NET)**: http://localhost:5000 ou https://localhost:5001
- **Frontend (Vue.js)**: http://localhost:5173

## Configuração

- O arquivo `backend/appsettings.Development.json` contém as configurações de ambiente do backend e não é versionado.
- Para rodar localmente, crie um arquivo `appsettings.Development.json` na pasta `backend` com as configurações necessárias.

## Funcionalidades

- Upload e análise de arquivos CSV com dados.
- Visualização de dados e gráficos no frontend.

## Estrutura de Pastas

```
backend/    # API .NET
frontend/   # Aplicação Vue.js
```

## Licença

Este projeto é de uso interno e não possui licença aberta.
