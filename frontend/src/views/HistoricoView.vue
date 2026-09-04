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
      <div class="bg-white shadow-sm border-b border-gray-100">
        <div class="container mx-auto py-5 sm:py-6 px-4 sm:px-6 lg:px-8">
          <h1 class="text-2xl sm:text-3xl font-bold leading-tight text-gray-900">Seu Histórico de Análises</h1>
          <p class="mt-1 text-sm text-gray-500">Acesse e reveja os insights gerados anteriormente.</p>
        </div>
      </div>

      <div class="container mx-auto py-6 sm:py-8 px-4 sm:px-6 lg:px-8">
        
        <div v-if="carregando" class="text-center py-12">
            <svg class="animate-spin h-8 w-8 text-blue-600 mx-auto" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            <p class="mt-3 text-sm text-gray-500">Carregando histórico...</p>
        </div>

        <div v-else-if="erro" class="p-5 sm:p-6 bg-red-50 border border-red-200 text-red-800 rounded-xl shadow-sm">
          <h3 class="font-bold text-base sm:text-lg">Ocorreu um Erro</h3>
          <p class="mt-1 text-sm sm:text-base">{{ erro }}</p>
        </div>

        <div v-else-if="historico.length > 0" class="space-y-3 sm:space-y-4">
          <div v-for="item in historico" :key="item.id">
            <RouterLink :to="{ name: 'detalhe-analise', params: { id: item.id } }" class="block group">
              <div class="p-4 sm:p-5 bg-white rounded-xl shadow-sm hover:shadow-md hover:border-blue-400 border border-gray-200 transition-all duration-200 active:scale-[0.99]">
                <div class="flex items-start sm:items-center justify-between gap-3">
                  <p class="text-base sm:text-lg font-semibold text-gray-800 group-hover:text-blue-600 transition-colors break-words flex-1">
                    {{ item.contexto }}
                  </p>
                  <svg class="h-5 w-5 text-gray-400 group-hover:text-blue-600 flex-shrink-0 mt-0.5 sm:mt-0 transition-transform duration-200 group-hover:translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                  </svg>
                </div>
                <div class="flex items-center text-xs sm:text-sm text-gray-400 mt-2.5 space-x-1.5">
                  <svg class="w-4 h-4 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
                  </svg>
                  <span>Criada em: {{ formatarData(item.dataCriacao) }}</span>
                </div>
              </div>
            </RouterLink>
          </div>
        </div>

        <div v-else class="text-center p-8 sm:p-12 bg-white rounded-xl shadow-sm border border-gray-200">
            <div class="mx-auto w-12 h-12 rounded-full bg-blue-50 flex items-center justify-center text-blue-600 mb-3">
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.832 5.477 15.446 5 17.1 5s3.332.477 4.5 1.253v13C20.332 18.477 18.754 18 17.1 18s-3.332.477-4.5 1.253" />
              </svg>
            </div>
            <h2 class="text-lg sm:text-xl font-semibold text-gray-700">Seu histórico está vazio</h2>
            <p class="mt-2 text-sm sm:text-base text-gray-500 mb-6">Nenhuma análise foi salva ainda. Que tal criar a primeira?</p>
            <RouterLink to="/analise" class="w-full sm:w-auto inline-block bg-blue-600 text-white hover:bg-blue-700 px-6 py-3 rounded-lg text-sm font-semibold transition-colors shadow-sm active:scale-95">
              Criar Nova Análise
            </RouterLink>
        </div>

      </div>
    </main>

    <Footer />
  </div>
</template>