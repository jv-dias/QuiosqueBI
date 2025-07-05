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
  <div class="page-layout">
    <NavBar />

    <main class="main-content container mx-auto py-10 px-4">
      <div class="text-center mb-12">
        <h1 class="text-4xl font-bold text-gray-800">Quiosque BI</h1>
        <p class="mt-2 text-lg text-gray-600">Transforme seus dados em decisões com apenas um clique.</p>
      </div>

      <UploadForm />

      <div class="mt-12">
        <div v-if="carregando" class="text-center">
          <p class="text-blue-600 animate-pulse">Analisando... A IA está pensando!</p>
        </div>

        <div v-else-if="erro" class="text-center p-4 bg-red-100 text-red-700 rounded-lg">
          <p><strong>Ocorreu um erro:</strong> {{ erro }}</p>
        </div>

        <div v-else-if="resultados.length > 0" class="space-y-8">
          <div v-for="(resultado, index) in resultados" :key="index" class="p-6 bg-white rounded-lg shadow-xl">
            <GraficoAnalise :titulo="resultado.titulo" :tipo="mapChartType(resultado.tipoGrafico)"
              :dados="resultado.dados" />
          </div>
        </div>
        <div v-else class="text-center p-4 bg-blue-50 text-blue-700 rounded-lg">
          <p>Envie um arquivo CSV e um contexto para gerar análises.</p>
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