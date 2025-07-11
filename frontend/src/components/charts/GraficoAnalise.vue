<script setup lang="ts">
import { computed, defineProps } from 'vue';
import { DataZoomComponent } from 'echarts/components';

// As props que o componente recebe continuam as mesmas
const props = defineProps({
  tipo: {
    type: String,
    required: true
  },
  titulo: {
    type: String,
    default: 'Gráfico de Análise'
  },
  dados: {
    type: Array as () => { categoria: any; valor: number }[],
    required: true
  }
});

// A principal mudança: criamos um único objeto 'option' para a ECharts
const option = computed(() => {
  // Configurações base que são comuns a quase todos os gráficos
  const baseOptions = {
    title: {
      text: props.titulo,
      left: 'center'
    },
    tooltip: {
      trigger: 'axis', // O tooltip aparece ao passar o mouse sobre o eixo
      axisPointer: {
        type: 'shadow'
      }
    },
    grid: {
      left: '3%',
      right: '4%',
      bottom: '3%',
      containLabel: true
    },
    series: [] as any[] // As séries de dados serão preenchidas abaixo
  };

  // Lógica para cada tipo de gráfico
  if (props.tipo === 'bar' || props.tipo === 'line') {
    return {
      ...baseOptions,
      xAxis: {
        type: 'category',
        data: props.dados.map(item => item.categoria)
      },
      yAxis: {
        type: 'value'
      },      
      dataZoom: [
        {
          type: 'inside', // Habilita o zoom com o scroll do mouse e o pan (arrastar)
          start: 0,       // Posição inicial do zoom (0% do início)
          end: 100         // Posição final do zoom (mostra os primeiros 20% dos dados)
        },
        {
          type: 'slider', // Adiciona a barra de rolagem na parte inferior
          start: 0,
          end: 100,
          //bottom: 5,
          // Estilização opcional da barra de rolagem
          handleIcon: 'path://M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7v-1.2h6.6V24.4z M13.3,22H6.7v-1.2h6.6V22z',
          handleSize: '80%',
          handleStyle: {
            color: '#fff',
            shadowBlur: 3,
            shadowColor: 'rgba(0, 0, 0, 0.6)',
            shadowOffsetX: 2,
            shadowOffsetY: 2,
          },
        }
      ],
      series: [{
        data: props.dados.map(item => item.valor),
        type: props.tipo // 'bar' ou 'line'
      }]
    };
  }

if (props.tipo === 'pie') {
  return {
    title: {
      text: props.titulo,
      left: 'center'
    },
    tooltip: {
      trigger: 'item'
    },
    // ===================================================================
    // ALTERAÇÕES NA LEGENDA
    // ===================================================================
    legend: {
      orient: 'vertical', // 1. Garante que a legenda fique em uma única coluna vertical
      left: 'left',       // 2. Alinha a legenda à esquerda do contêiner do gráfico
      top: 'center',      // 3. Centraliza a legenda verticalmente
      itemGap: 15,        // 4. Aumenta o espaçamento vertical entre cada item da legenda
      padding: [0, 50, 0, 20] // Adiciona um padding para afastar do gráfico
    },
    // ===================================================================
    series: [{
      type: 'pie',
      // 5. Ajustamos o raio e o centro para dar espaço à legenda
      radius: '70%', 
      center: ['70%', '50%'], // Move o centro do gráfico um pouco para a direita
      data: props.dados.map(item => ({
        value: item.valor,
        name: item.categoria
      })),
      emphasis: {
        itemStyle: {
          shadowBlur: 10,
          shadowOffsetX: 0,
          shadowColor: 'rgba(0, 0, 0, 0.5)'
        }
      }
    }]
  };
}

if (props.tipo === 'funil') {
  return {
    title: {
      text: props.titulo,
      left: 'center'
    },
    tooltip: {
      trigger: 'item',
      formatter: '{b}: {c} ({d}%)' // Formato do tooltip: Categoria: Valor (Porcentagem)
    },
    legend: {
      orient: 'vertical',
      left: 'left',
      data: props.dados.map(item => item.categoria)
    },
    series: [{
      name: 'Funil',
      type: 'funnel',
      left: '25%', // Dá espaço para a legenda
      width: '65%',
      label: {
        show: true,
        position: 'inside'
      },
      data: props.dados
        .sort((a, b) => b.valor - a.valor) // O funil precisa que os dados estejam ordenados do maior para o menor
        .map(item => ({
          value: item.valor,
          name: item.categoria
        }))
    }]
  };
}

  // Retorna uma opção vazia se o tipo não for reconhecido
  return {};
});
</script>

<template>
  <div>
    <v-chart
      v-if="dados && dados.length > 0"
      class="chart"
      :option="option"
      autoresize
      style="height: 400px;"
    />
    <div v-else class="h-64 flex items-center justify-center bg-gray-50 rounded-lg">
        <p class="text-gray-400">Não há dados para exibir neste gráfico.</p>
    </div>
  </div>
</template>