<script setup lang="ts">
import { computed } from 'vue';
import VueApexCharts from 'vue3-apexcharts';

// ===================================================================
// CORREÇÃO APLICADA AQUI: Definindo as props de forma mais robusta
// ===================================================================
const props = defineProps({
  tipo: {
    type: String,
    required: true
  },
  titulo: {
    type: String,
    // Se um título não for fornecido, usamos um valor padrão.
    default: 'Gráfico de Análise' 
  },
  dados: {
    type: Array as () => { categoria: any; valor: number }[] | null,
    required: true
  }
});
// ===================================================================

const series = computed(() => {
  if (!props.dados) return [];

  if (props.tipo === 'pie') {
    return props.dados.map(item => item.valor);
  }
  
  return [{
    name: props.titulo, // Usando o título da prop, que agora tem um padrão.
    data: props.dados.map(item => ({
      x: String(item.categoria),
      y: item.valor
    }))
  }];
});

const chartOptions = computed(() => {
  if (!props.dados) return {};

  const options = {
    chart: {
      // O 'id' agora é seguro, pois 'titulo' sempre terá um valor.
      id: `chart-${props.titulo.replace(/\s/g, '-')}`,
      toolbar: { show: true }
    },
    title: {
      text: props.titulo, // O título do gráfico também é seguro.
      align: 'left'
    }
  };

  if (props.tipo === 'pie') {
    return {
      ...options,
      labels: props.dados.map(item => String(item.categoria)),
      legend: { position: 'bottom' }
    };
  }

  return options;
});
</script>

<template>
  <div>
    <VueApexCharts
      v-if="dados && dados.length > 0"
      :type="tipo"
      :options="chartOptions"
      :series="series"
      height="350"
    />
    <div v-else class="h-64 flex items-center justify-center bg-gray-50">
        <p class="text-gray-400">Não há dados para exibir neste gráfico.</p>
    </div>
  </div>
</template>