<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute, RouterLink } from 'vue-router';
import axios from 'axios';
import GraficoAnalise from '@/components/charts/GraficoAnalise.vue';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';

const API_BASE_URL = import.meta.env.VITE_API_URL;

const route = useRoute();
const analise = ref<any>(null);
const carregando = ref(true);
const erro = ref<string | null>(null);

const resultadosParseados = computed(() => {
  if (analise.value && analise.value.resultadosJson) {
    try {
      const resultadosComPascalCase = JSON.parse(analise.value.resultadosJson);

      return resultadosComPascalCase.map((resultado: any) => {
        return {
          titulo: resultado.Titulo,
          tipoGrafico: resultado.TipoGrafico,
          dados: (resultado.Dados || []).map((pontoDoGrafico: any) => {

            let categoriaFormatada = pontoDoGrafico.Categoria;

            if (typeof categoriaFormatada === 'string' && categoriaFormatada.includes('T')) {
              categoriaFormatada = categoriaFormatada.split('T')[0];
            }

            return {
              categoria: categoriaFormatada,
              valor: pontoDoGrafico.Valor
            };
          })
        };
      });
    } catch (e) {
      erro.value = "Erro ao processar os dados dos resultados.";
      console.error("Erro no JSON.parse ou no mapeamento:", e);
      return [];
    }
  }
  return [];
});

function mapChartType(tipoApi: string): string {
  switch (tipoApi?.toLowerCase()) {
    case 'barras': return 'bar';
    case 'linha': return 'line';
    case 'pizza': return 'pie';
    default: return 'bar';
  }
}

onMounted(async () => {
  const id = route.params.id;
  if (!id || Array.isArray(id)) {
    erro.value = "ID da análise inválido.";
    carregando.value = false;
    return;
  }

  try {
    const url = `${API_BASE_URL}/analise/historico/${id}`;
    const response = await axios.get(url);
    analise.value = response.data;
  } catch (e: any) {
    erro.value = "Análise não encontrada ou erro ao buscar os dados.";
    console.error(e);
  } finally {
    carregando.value = false;
  }
});
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <NavBar />

    <main class="flex-grow container mx-auto py-6 sm:py-8 px-4 sm:px-6 lg:px-8">
      <!-- Botão Voltar -->
      <div class="mb-5">
        <RouterLink
          to="/historico"
          class="inline-flex items-center text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors py-1.5 px-3 rounded-lg hover:bg-blue-50/80 -ml-3"
        >
          <svg class="w-4 h-4 mr-1.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
          Voltar ao Histórico
        </RouterLink>
      </div>

      <!-- Estado de Carregamento -->
      <div v-if="carregando" class="flex flex-col items-center justify-center py-16 text-center">
        <svg class="animate-spin h-8 w-8 text-blue-600 mb-3" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
        <p class="text-sm sm:text-base text-gray-500">Carregando detalhes da análise...</p>
      </div>

      <!-- Estado de Erro -->
      <div v-else-if="erro" class="p-5 sm:p-6 bg-red-50 border border-red-200 text-red-800 rounded-xl shadow-sm">
        <h3 class="font-bold text-base sm:text-lg">Ocorreu um Erro</h3>
        <p class="mt-1 text-sm sm:text-base">{{ erro }}</p>
      </div>

      <!-- Detalhes e Gráficos -->
      <div v-else-if="analise">
        <div class="bg-white p-4 sm:p-6 rounded-xl shadow-sm border border-gray-200 mb-6">
          <span class="text-xs font-semibold uppercase tracking-wider text-blue-600">Relatório Salvo</span>
          <h1 class="text-2xl sm:text-3xl font-bold text-gray-900 mt-1 mb-2">Detalhes da Análise</h1>
          <p class="text-sm sm:text-base text-gray-600 leading-relaxed break-words">
            <strong class="text-gray-800">Objetivo:</strong> "{{ analise.contexto }}"
          </p>
        </div>

        <div class="space-y-6 sm:space-y-8">
          <div
            v-for="(resultado, index) in resultadosParseados"
            :key="index"
            class="p-4 sm:p-6 bg-white rounded-xl shadow-sm border border-gray-200"
          >
            <GraficoAnalise
              :titulo="resultado.titulo"
              :tipo="mapChartType(resultado.tipoGrafico)"
              :dados="resultado.dados"
            />
          </div>
        </div>
      </div>
    </main>

    <Footer />
  </div>
</template>
