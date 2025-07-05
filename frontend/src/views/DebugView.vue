<script setup lang="ts">
import { ref } from 'vue';
import { useAnaliseStore, type DebugInfo } from '@/stores/analiseStore';
import { storeToRefs } from 'pinia';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';

const analiseStore = useAnaliseStore();
const { debugInfo, carregandoDebug, erro } = storeToRefs(analiseStore);

const arquivoDebug = ref<File | null>(null);
const contextoDebug = ref<string>('');

function handleFileChangeDebug(event: Event) {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    arquivoDebug.value = target.files[0];
  }
}

async function handleGetDebugData() {
  if (!arquivoDebug.value || !contextoDebug.value) {
    alert('Por favor, selecione um arquivo e descreva o contexto para depuração.');
    return;
  }

  const formData = new FormData();
  formData.append('arquivo', arquivoDebug.value);
  formData.append('contexto', contextoDebug.value);

  await analiseStore.obterDadosDepuracao(formData);
}

// Função para renderizar uma célula de dados na tabela (para objetos dinâmicos)
function renderDataCell(item: Record<string, any>, header: string): any {
  const itemAsDict = item as { [key: string]: any };
  return itemAsDict[header] !== undefined ? itemAsDict[header] : '';
}
</script>

<template>
  <div class="page-layout">
    <NavBar />

    <main class="main-content container mx-auto py-10 px-4">
      <div class="text-center mb-12">
        <h1 class="text-4xl font-bold text-gray-800">Quiosque BI - Painel de Depuração</h1>
        <p class="mt-2 text-lg text-gray-600">Visualize dados brutos e o plano da IA para depuração.</p>
      </div>

      <div class="max-w-2xl mx-auto p-8 bg-gray-50 rounded-lg shadow-md mb-12">
        <h2 class="text-2xl font-bold text-gray-800 mb-6">Obter Dados de Depuração</h2>
        <div class="space-y-6">
          <div>
            <label for="debug-file-upload" class="block text-sm font-medium text-gray-700">
              1. Selecione a planilha (.csv) para depuração
            </label>
            <div class="mt-1">
              <input id="debug-file-upload" name="debug-file-upload" type="file" accept=".csv"
                @change="handleFileChangeDebug" class="block w-full text-sm text-slate-500
                              file:mr-4 file:py-2 file:px-4
                              file:rounded-md file:border-0
                              file:text-sm file:font-semibold
                              file:bg-purple-50 file:text-purple-700
                              hover:file:bg-purple-100" />
            </div>
          </div>

          <div>
            <label for="debug-context" class="block text-sm font-medium text-gray-700">
              2. Descreva o objetivo da análise para a depuração
            </label>
            <div class="mt-1">
              <textarea id="debug-context" name="debug-context" rows="3" v-model="contextoDebug"
                class="block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-purple-500 focus:border-purple-500 sm:text-sm"
                placeholder="Ex: 'Entender a distribuição de clientes por região.'"></textarea>
            </div>
          </div>

          <div>
            <button @click="handleGetDebugData" :disabled="carregandoDebug"
              class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-purple-600 hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-purple-500">
              {{ carregandoDebug ? 'Obtendo Dados...' : 'Obter Dados de Depuração' }}
            </button>
          </div>
        </div>
      </div>

      <div v-if="carregandoDebug" class="text-center">
        <p class="text-purple-600 animate-pulse">Carregando dados de depuração...</p>
      </div>

      <div v-else-if="erro && debugInfo === null" class="text-center p-4 bg-red-100 text-red-700 rounded-lg">
        <p><strong>Ocorreu um erro ao obter dados de depuração:</strong> {{ erro }}</p>
      </div>

      <div v-else-if="debugInfo" class="p-8 bg-white rounded-lg shadow-md">
        <h3 class="text-2xl font-semibold text-gray-700 mb-4">Informações de Depuração:</h3>

        <div class="mb-6 p-4 bg-white rounded-lg shadow-sm border border-gray-200">
          <h4 class="font-bold text-lg text-gray-800 mb-2">Plano Sugerido pela IA:</h4>
          <pre
            class="bg-gray-100 p-3 rounded-md text-sm whitespace-pre-wrap">{{ JSON.stringify(debugInfo.planoDaIA, null, 2) }}</pre>
        </div>

        <div class="mb-6 p-4 bg-white rounded-lg shadow-sm border border-gray-200">
          <h4 class="font-bold text-lg text-gray-800 mb-2">Cabeçalhos Encontrados no Arquivo:</h4>
          <pre
            class="bg-gray-100 p-3 rounded-md text-sm whitespace-pre-wrap">{{ JSON.stringify(debugInfo.cabecalhosDoArquivo, null, 2) }}</pre>
        </div>

        <div class="p-4 bg-white rounded-lg shadow-sm border border-gray-200">
          <h4 class="font-bold text-lg text-gray-800 mb-2">Primeiras 10 Linhas do Arquivo Bruto:</h4>
          <div class="overflow-x-auto">
            <table class="min-w-full divide-y divide-gray-200">
              <thead class="bg-gray-50">
                <tr>
                  <th v-for="header in debugInfo.cabecalhosDoArquivo" :key="header"
                    class="px-4 py-2 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    {{ header }}
                  </th>
                </tr>
              </thead>
              <tbody class="bg-white divide-y divide-gray-200">
                <tr v-for="(item, rowIndex) in debugInfo.dadosBrutosDoArquivo" :key="rowIndex">
                  <td v-for="header in debugInfo.cabecalhosDoArquivo" :key="header"
                    class="px-4 py-2 whitespace-nowrap text-sm text-gray-900">
                    {{ renderDataCell(item, header) }}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <p v-if="debugInfo.dadosBrutosDoArquivo.length === 0" class="text-center text-gray-500 mt-4">Nenhum dado bruto
            para exibir.</p>
        </div>
      </div>
    </main>

    <Footer />
  </div>
</template>

<style scoped>
.page-layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.main-content {
  flex: 1;
}
</style>