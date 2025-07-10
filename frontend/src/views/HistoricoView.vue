<script setup lang="ts">
import { onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useAnaliseStore } from '@/stores/analiseStore';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';
import { RouterLink } from 'vue-router';

const analiseStore = useAnaliseStore();
const { historico, carregando, erro } = storeToRefs(analiseStore);

// Formata a data para uma leitura mais fácil
function formatarData(dataString: string) {
  return new Date(dataString).toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
}

// Busca o histórico quando o componente é montado na tela
onMounted(() => {
  analiseStore.buscarHistorico();
});
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <NavBar />

    <main class="flex-grow">
      
      <div class="bg-white shadow-sm">
        <div class="container mx-auto py-6 px-4 sm:px-6 lg:px-8">
          <h1 class="text-3xl font-bold leading-tight text-gray-900">Seu Histórico de Análises</h1>
          <p class="mt-1 text-sm text-gray-500">Acesse e reveja os insights gerados anteriormente.</p>
        </div>
      </div>

      <div class="container mx-auto py-8 px-4 sm:px-6 lg:px-8">
        
        <div v-if="carregando" class="text-center py-12">
            <svg class="animate-spin h-8 w-8 text-blue-600 mx-auto" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            <p class="mt-4 text-gray-500">Carregando histórico...</p>
        </div>

        <div v-else-if="erro" class="p-6 bg-red-50 border border-red-200 text-red-800 rounded-lg shadow-sm">
          <h3 class="font-bold text-lg">Ocorreu um Erro</h3>
          <p class="mt-1">{{ erro }}</p>
        </div>

        <div v-else-if="historico.length > 0" class="space-y-4">
          <div v-for="item in historico" :key="item.id">
            <RouterLink :to="{ name: 'detalhe-analise', params: { id: item.id } }" class="block group">
              <div class="p-6 bg-white rounded-lg shadow-md hover:shadow-xl hover:border-blue-500 border border-transparent transition-all duration-300">
                <div class="flex justify-between items-center">
                  <p class="text-lg font-semibold text-gray-800 group-hover:text-blue-600 transition-colors">
                    {{ item.contexto }}
                  </p>
                  <svg class="h-5 w-5 text-gray-400 group-hover:text-blue-600 transition-transform duration-300 group-hover:translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" /></svg>
                </div>
                <p class="text-sm text-gray-500 mt-2">Análise criada em: {{ formatarData(item.dataCriacao) }}</p>
              </div>
            </RouterLink>
          </div>
        </div>

        <div v-else class="text-center p-12 bg-white rounded-lg shadow">
            <h2 class="text-xl font-semibold text-gray-700">Seu histórico está vazio</h2>
            <p class="mt-2 text-gray-500 mb-6">Nenhuma análise foi salva ainda. Que tal criar a primeira?</p>
            <RouterLink to="/analise" class="inline-block bg-blue-600 text-white hover:bg-blue-700 px-6 py-3 rounded-md text-sm font-medium transition-colors shadow-sm">
              Criar Nova Análise
            </RouterLink>
        </div>

      </div>
    </main>

    <Footer />
  </div>
</template>