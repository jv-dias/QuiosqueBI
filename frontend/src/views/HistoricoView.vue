<script setup lang="ts">
import { onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useAnaliseStore } from '@/stores/analiseStore';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';

const analiseStore = useAnaliseStore();
const { historico, carregando, erro } = storeToRefs(analiseStore);

// Formata a data para uma leitura mais fácil
function formatarData(dataString: string) {
  return new Date(dataString).toLocaleString('pt-BR');
}

// Busca o histórico quando o componente é montado na tela
onMounted(() => {
  analiseStore.buscarHistorico();
});
</script>

<template>
  <NavBar />
  <main class="container mx-auto py-10 px-4">
    <h1 class="text-4xl font-bold text-gray-800 mb-8">Histórico de Análises</h1>

    <div v-if="carregando" class="text-center">
      <p class="text-blue-600 animate-pulse">Carregando histórico...</p>
    </div>

    <div v-else-if="erro" class="text-center p-4 bg-red-100 text-red-700 rounded-lg">
      <p><strong>Ocorreu um erro:</strong> {{ erro }}</p>
    </div> 

    <div v-for="item in historico" :key="item.id"> <RouterLink :to="{ name: 'detalhe-analise', params: { id: item.id } }"> <div class="p-6 bg-white rounded-lg shadow-md hover:shadow-xl transition-shadow cursor-pointer">
    <p class="text-lg font-semibold text-gray-800">{{ item.contexto }}</p>
    <p class="text-sm text-gray-500 mt-2">Análise criada em: {{ formatarData(item.dataCriacao) }}</p>
    </div>

  </RouterLink>

</div>
    
  </main>
  <Footer />    
</template>