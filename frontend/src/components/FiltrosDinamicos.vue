<script setup lang="ts">
import { ref, computed } from 'vue';
import type { MetadadosColuna } from '@/tipos/perfilamento';
import { useAnaliseStore } from '@/stores/analiseStore';

const props = defineProps<{
  perfilamento: MetadadosColuna[];
}>();

const analiseStore = useAnaliseStore();
const isExpandedMobile = ref(true);

const colunasFiltravelis = () =>
  props.perfilamento.filter(m => m.relevancia === 'alta' && m.sugestaoFiltro !== 'nenhum');

const totalFiltrosAtivos = computed(() => {
  return Object.values(analiseStore.filtrosAtivos).filter(v => {
    if (Array.isArray(v)) return v.length > 0;
    return !!v;
  }).length;
});

function onCaixaSelecaoChange(nomeColuna: string, opcao: string, checked: boolean) {
  const atual = (analiseStore.filtrosAtivos[nomeColuna] as string[]) ?? [];
  const novoValor = checked
    ? [...atual, opcao]
    : atual.filter(v => v !== opcao);
  analiseStore.definirFiltro(nomeColuna, novoValor);
}

function onIntervaloChange(nomeColuna: string, tipo: 'min' | 'max', valor: string, meta: MetadadosColuna) {
  const atual = (analiseStore.filtrosAtivos[nomeColuna] as [number, number]) ?? [meta.minimo ?? 0, meta.maximo ?? 100];
  const novoValor: [number, number] = tipo === 'min'
    ? [Number(valor), atual[1]]
    : [atual[0], Number(valor)];
  analiseStore.definirFiltro(nomeColuna, novoValor);
}

function onDataChange(nomeColuna: string, valor: string) {
  analiseStore.definirFiltro(nomeColuna, valor);
}

function isSelecionado(nomeColuna: string, opcao: string): boolean {
  const atual = analiseStore.filtrosAtivos[nomeColuna] as string[] | undefined;
  return atual ? atual.includes(opcao) : false;
}

function getIntervaloMin(nomeColuna: string, meta: MetadadosColuna): number {
  const atual = analiseStore.filtrosAtivos[nomeColuna] as [number, number] | undefined;
  return atual ? atual[0] : (meta.minimo ?? 0);
}

function getIntervaloMax(nomeColuna: string, meta: MetadadosColuna): number {
  const atual = analiseStore.filtrosAtivos[nomeColuna] as [number, number] | undefined;
  return atual ? atual[1] : (meta.maximo ?? 100);
}
</script>

<template>
  <div v-if="colunasFiltravelis().length > 0" class="bg-white rounded-xl shadow-sm border border-gray-200 p-4 sm:p-5 mb-6">
    <div class="flex items-center justify-between">
      <button
        type="button"
        @click="isExpandedMobile = !isExpandedMobile"
        class="flex items-center space-x-2 text-left focus:outline-none group"
      >
        <h2 class="text-base sm:text-lg font-bold text-gray-800 flex items-center">
          <svg class="w-5 h-5 mr-1.5 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
          </svg>
          Filtros
        </h2>
        <span v-if="totalFiltrosAtivos > 0" class="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-semibold bg-blue-100 text-blue-800">
          {{ totalFiltrosAtivos }} ativo(s)
        </span>
        <!-- Seta indicadora (visível em mobile) -->
        <svg
          class="w-4 h-4 text-gray-400 group-hover:text-gray-600 transition-transform duration-200 sm:hidden"
          :class="{ 'rotate-180': isExpandedMobile }"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
        </svg>
      </button>

      <button
        v-if="totalFiltrosAtivos > 0"
        @click="analiseStore.limparFiltros()"
        class="text-xs sm:text-sm text-red-600 hover:text-red-800 font-medium px-2 py-1 rounded hover:bg-red-50 transition-colors"
      >
        Limpar Filtros
      </button>
    </div>

    <!-- Conteúdo dos filtros -->
    <div
      :class="[
        'mt-4 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3.5 sm:gap-4 transition-all duration-200',
        isExpandedMobile ? 'block' : 'hidden sm:grid'
      ]"
    >
      <div
        v-for="meta in colunasFiltravelis()"
        :key="meta.nomeColuna"
        class="border border-gray-200/80 bg-gray-50/50 rounded-lg p-3 sm:p-3.5"
      >
        <p class="text-xs sm:text-sm font-semibold text-gray-700 mb-2 flex items-center justify-between">
          <span>{{ meta.nomeColuna }}</span>
          <span class="text-[10px] sm:text-xs text-gray-400 font-normal px-1.5 py-0.5 bg-white rounded border border-gray-100">{{ meta.tipoSemantico }}</span>
        </p>

        <!-- caixaSelecao -->
        <div v-if="meta.sugestaoFiltro === 'caixaSelecao'" class="space-y-1.5 max-h-36 overflow-y-auto pr-1">
          <label
            v-for="opcao in meta.opcoes ?? []"
            :key="opcao"
            class="flex items-center gap-2.5 text-xs sm:text-sm text-gray-700 cursor-pointer py-1 hover:bg-white/60 rounded px-1 -mx-1"
          >
            <input
              type="checkbox"
              :checked="isSelecionado(meta.nomeColuna, opcao)"
              @change="e => onCaixaSelecaoChange(meta.nomeColuna, opcao, (e.target as HTMLInputElement).checked)"
              class="w-4 h-4 rounded text-blue-600 focus:ring-blue-500 accent-blue-600"
            />
            <span class="truncate">{{ opcao }}</span>
          </label>
        </div>

        <!-- controleIntervalo -->
        <div v-else-if="meta.sugestaoFiltro === 'controleIntervalo'" class="space-y-2.5 pt-1">
          <div class="flex items-center gap-2">
            <label class="text-xs text-gray-500 w-8">Mín</label>
            <input
              type="range"
              :min="meta.minimo ?? 0"
              :max="meta.maximo ?? 100"
              :value="getIntervaloMin(meta.nomeColuna, meta)"
              @input="e => onIntervaloChange(meta.nomeColuna, 'min', (e.target as HTMLInputElement).value, meta)"
              class="w-full accent-blue-600 h-2 bg-gray-200 rounded-lg cursor-pointer"
            />
            <span class="text-xs font-medium text-gray-700 w-12 text-right">{{ getIntervaloMin(meta.nomeColuna, meta) }}</span>
          </div>
          <div class="flex items-center gap-2">
            <label class="text-xs text-gray-500 w-8">Máx</label>
            <input
              type="range"
              :min="meta.minimo ?? 0"
              :max="meta.maximo ?? 100"
              :value="getIntervaloMax(meta.nomeColuna, meta)"
              @input="e => onIntervaloChange(meta.nomeColuna, 'max', (e.target as HTMLInputElement).value, meta)"
              class="w-full accent-blue-600 h-2 bg-gray-200 rounded-lg cursor-pointer"
            />
            <span class="text-xs font-medium text-gray-700 w-12 text-right">{{ getIntervaloMax(meta.nomeColuna, meta) }}</span>
          </div>
        </div>

        <!-- seletorData -->
        <div v-else-if="meta.sugestaoFiltro === 'seletorData'">
          <input
            type="date"
            :value="(analiseStore.filtrosAtivos[meta.nomeColuna] as string) ?? ''"
            @change="e => onDataChange(meta.nomeColuna, (e.target as HTMLInputElement).value)"
            class="block w-full text-base sm:text-sm border border-gray-300 rounded-lg px-3 py-2 bg-white focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
      </div>
    </div>
  </div>
</template>
