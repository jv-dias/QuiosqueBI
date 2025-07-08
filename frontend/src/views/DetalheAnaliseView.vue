<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';
import GraficoAnalise from '@/components/charts/GraficoAnalise.vue';

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

            // *** NOVA LÓGICA DE FORMATAÇÃO DE DATA AQUI ***
            // Se a categoria for uma string e incluir a letra 'T' (de Time)
            if (typeof categoriaFormatada === 'string' && categoriaFormatada.includes('T')) {
              // Pega apenas a parte antes do 'T'
              categoriaFormatada = categoriaFormatada.split('T')[0];
            }

            return {
              categoria: categoriaFormatada,
              valor: pontoDoGrafico.Valor
            }
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
  if (!id || Array.isArray(id)) { // Adiciona verificação extra
    erro.value = "ID da análise inválido.";
    carregando.value = false;
    return;
  }
  
  try {
    const url = `http://localhost:5159/api/analise/historico/${id}`;
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
  <main class="container mx-auto py-10 px-4">
    <div v-if="carregando">Carregando análise...</div>
    <div v-else-if="erro">{{ erro }}</div>
    <div v-else-if="analise">
      <h1 class="text-3xl font-bold mb-2">Detalhes da Análise</h1>
      <p class="text-lg text-gray-600 mb-8">Contexto: "{{ analise.contexto }}"</p>

      <div class="space-y-8">
        <div v-for="(resultado, index) in resultadosParseados" :key="index" class="p-6 bg-white rounded-lg shadow-xl">
          <GraficoAnalise 
  :titulo="resultado.titulo"
  :tipo="mapChartType(resultado.tipoGrafico)"
  :dados="resultado.dados"  />
        </div>
      </div>
    </div>
  </main>
</template>