<script setup lang="ts">
import { storeToRefs } from 'pinia';
import UploadForm from '@/components/UploadForm.vue';
import GraficoAnalise from '@/components/charts/GraficoAnalise.vue';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';
import { useAnaliseStore } from '@/stores/analiseStore';

const analiseStore = useAnaliseStore();
const { resultados, carregando, erro } = storeToRefs(analiseStore);

function mapChartType(tipoApi: string): string {
  switch (tipoApi?.toLowerCase()) {
    case 'barras': return 'bar';
    case 'linha': return 'line';
    case 'pizza': return 'pie';
    default: return 'bar';
  }
}
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <NavBar />

    <main class="flex-grow">
      <div class="bg-white shadow-sm">
        <div class="container mx-auto py-6 px-4 sm:px-6 lg:px-8">
          <h1 class="text-3xl font-bold leading-tight text-gray-900">Painel de Análise</h1>
          <p class="mt-1 text-sm text-gray-500">Envie um arquivo para começar a gerar insights.</p>
        </div>
      </div>

      <div class="container mx-auto py-8 px-4 sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
          
          <div class="lg:col-span-1">
            <div class="bg-white p-6 rounded-lg shadow">
              <h2 class="text-xl font-semibold mb-4 text-gray-800">Nova Análise</h2>
              <UploadForm />
            </div>
          </div>

          <div class="lg:col-span-2">
            
            <div v-if="carregando" class="flex flex-col items-center justify-center bg-white p-12 rounded-lg shadow text-center">
                <p class="text-xl font-semibold text-blue-600">Analisando seus dados...</p>
                <p class="mt-2 text-gray-500">A Inteligência Artificial está trabalhando. Isso pode levar alguns segundos.</p>
                <svg class="animate-spin h-8 w-8 text-blue-600 mt-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                    <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
            </div>

            <div v-else-if="erro" class="p-6 bg-red-50 border border-red-200 text-red-800 rounded-lg shadow">
              <h3 class="font-bold text-lg">Ocorreu um Erro</h3>
              <p class="mt-1">{{ erro }}</p>
            </div>

            <div v-else-if="resultados.length > 0" class="space-y-8">
              <div v-for="(resultado, index) in resultados" :key="index" class="p-6 bg-white rounded-lg shadow-lg">
                <GraficoAnalise 
                  :titulo="resultado.titulo" 
                  :tipo="mapChartType(resultado.tipoGrafico)"
                  :dados="resultado.dados" />
              </div>
            </div>
            
            <div v-else class="text-center p-12 bg-white rounded-lg shadow">
              <h2 class="text-xl font-semibold text-gray-700">Seus resultados aparecerão aqui</h2>
              <p class="mt-2 text-gray-500">Utilize o formulário ao lado para enviar um arquivo e começar sua primeira análise.</p>
            </div>
            
          </div>
        </div>
      </div>
    </main>

    <Footer />
  </div>
</template>