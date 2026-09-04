<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useAnaliseStore } from '@/stores/analiseStore';

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

const analiseStore = useAnaliseStore();

// Detecção reativa de tela mobile
const isMobile = ref(false);
const updateScreenSize = () => {
  if (typeof window !== 'undefined') {
    isMobile.value = window.innerWidth < 640;
  }
};

onMounted(() => {
  updateScreenSize();
  window.addEventListener('resize', updateScreenSize);
});

onUnmounted(() => {
  window.removeEventListener('resize', updateScreenSize);
});

const dadosFiltrados = computed(() => {
  const filtros = analiseStore.filtrosAtivos;
  const perfilamento = analiseStore.perfilamentoAtual;

  if (Object.keys(filtros).length === 0) return props.dados;

  return props.dados.filter(item => {
    for (const [nomeColuna, valor] of Object.entries(filtros)) {
      const meta = perfilamento.find(
        m => m.nomeColuna.toLowerCase() === nomeColuna.toLowerCase()
      );
      if (!meta) continue;

      if (meta.sugestaoFiltro === 'caixaSelecao') {
        const selecionados = valor as string[];
        if (selecionados.length > 0 && !selecionados.includes(String(item.categoria))) return false;
      } else if (meta.sugestaoFiltro === 'controleIntervalo') {
        const [min, max] = valor as [number, number];
        if (item.valor < min || item.valor > max) return false;
      } else if (meta.sugestaoFiltro === 'seletorData') {
        const dataStr = valor as string;
        if (dataStr && !String(item.categoria).startsWith(dataStr)) return false;
      }
    }
    return true;
  });
});

const option = computed(() => {
  const baseOptions = {
    title: {
      text: props.titulo,
      left: 'center',
      top: 5,
      textStyle: {
        fontSize: isMobile.value ? 14 : 16,
        fontWeight: 'bold' as const,
        color: '#1f2937'
      }
    },
    tooltip: {
      trigger: 'axis' as const,
      confine: true, // Garante que o tooltip nunca ultrapasse os limites da tela no celular
      axisPointer: {
        type: 'shadow' as const
      },
      extraCssText: 'border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.12);'
    },
    grid: {
      left: '2%',
      right: '3%',
      top: isMobile.value ? 55 : 65,
      bottom: isMobile.value ? '14%' : '8%',
      containLabel: true
    },
    series: [] as any[]
  };

  if (props.tipo === 'bar' || props.tipo === 'line') {
    return {
      ...baseOptions,
      xAxis: {
        type: 'category' as const,
        data: dadosFiltrados.value.map(item => item.categoria),
        axisLabel: {
          rotate: isMobile.value ? 35 : 0,
          interval: isMobile.value && dadosFiltrados.value.length > 6 ? 'auto' : 0,
          fontSize: isMobile.value ? 11 : 12,
          formatter: (val: string) => {
            if (typeof val === 'string' && val.length > 12) {
              return val.slice(0, 10) + '…';
            }
            return val;
          }
        }
      },
      yAxis: {
        type: 'value' as const,
        axisLabel: {
          fontSize: isMobile.value ? 11 : 12
        }
      },
      dataZoom: [
        {
          type: 'inside' as const,
          start: 0,
          end: 100
        },
        {
          type: 'slider' as const,
          start: 0,
          end: 100,
          bottom: 2,
          height: isMobile.value ? 18 : 24,
          handleSize: '100%',
          handleStyle: {
            color: '#2563eb',
            shadowBlur: 3
          }
        }
      ],
      series: [{
        data: dadosFiltrados.value.map(item => item.valor),
        type: props.tipo,
        itemStyle: {
          borderRadius: props.tipo === 'bar' ? [4, 4, 0, 0] : 0
        },
        smooth: props.tipo === 'line'
      }]
    };
  }

  if (props.tipo === 'pie') {
    return {
      title: {
        text: props.titulo,
        left: 'center',
        top: 5,
        textStyle: {
          fontSize: isMobile.value ? 14 : 16,
          fontWeight: 'bold' as const,
          color: '#1f2937'
        }
      },
      tooltip: {
        trigger: 'item' as const,
        confine: true,
        formatter: '{b}: {c} ({d}%)',
        extraCssText: 'border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.12);'
      },
      legend: isMobile.value
        ? {
            orient: 'horizontal' as const,
            bottom: 0,
            left: 'center',
            type: 'scroll' as const,
            pageButtonPosition: 'end' as const,
            itemGap: 10,
            itemWidth: 12,
            itemHeight: 12,
            textStyle: { fontSize: 11 }
          }
        : {
            orient: 'vertical' as const,
            left: 'left',
            top: 'center',
            itemGap: 12,
            padding: [0, 20, 0, 10]
          },
      series: [{
        type: 'pie' as const,
        radius: isMobile.value ? ['32%', '60%'] : ['40%', '70%'],
        center: isMobile.value ? ['50%', '45%'] : ['65%', '52%'],
        avoidLabelOverlap: true,
        label: {
          show: !isMobile.value,
          formatter: '{b}: {d}%'
        },
        labelLine: {
          show: !isMobile.value
        },
        data: dadosFiltrados.value.map(item => ({
          value: item.valor,
          name: item.categoria
        })),
        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.3)'
          }
        }
      }]
    };
  }

  if (props.tipo === 'funil') {
    return {
      title: {
        text: props.titulo,
        left: 'center',
        top: 5,
        textStyle: {
          fontSize: isMobile.value ? 14 : 16,
          fontWeight: 'bold' as const,
          color: '#1f2937'
        }
      },
      tooltip: {
        trigger: 'item' as const,
        confine: true,
        formatter: '{b}: {c} ({d}%)',
        extraCssText: 'border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.12);'
      },
      legend: isMobile.value
        ? {
            orient: 'horizontal' as const,
            bottom: 0,
            left: 'center',
            type: 'scroll' as const,
            textStyle: { fontSize: 11 }
          }
        : {
            orient: 'vertical' as const,
            left: 'left',
            data: dadosFiltrados.value.map(item => item.categoria)
          },
      series: [{
        name: 'Funil',
        type: 'funnel' as const,
        left: isMobile.value ? '8%' : '25%',
        width: isMobile.value ? '84%' : '65%',
        top: isMobile.value ? 50 : 60,
        bottom: isMobile.value ? 40 : 25,
        label: {
          show: true,
          position: 'inside' as const,
          fontSize: isMobile.value ? 11 : 12
        },
        data: dadosFiltrados.value
          .slice()
          .sort((a, b) => b.valor - a.valor)
          .map(item => ({
            value: item.valor,
            name: item.categoria
          }))
      }]
    };
  }

  return {};
});
</script>

<template>
  <div class="w-full">
    <v-chart
      v-if="dadosFiltrados && dadosFiltrados.length > 0"
      class="w-full"
      :option="option"
      autoresize
      :style="{ height: isMobile ? '330px' : '400px' }"
    />
    <div v-else class="h-56 sm:h-64 flex items-center justify-center bg-gray-50 rounded-xl border border-dashed border-gray-200">
      <p class="text-sm text-gray-400">Não há dados para exibir neste gráfico.</p>
    </div>
  </div>
</template>
