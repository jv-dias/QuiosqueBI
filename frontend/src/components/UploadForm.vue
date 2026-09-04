<script setup lang="ts">
import { ref } from 'vue';
import { useAnaliseStore } from '@/stores/analiseStore';
import { storeToRefs } from 'pinia';

const contexto = ref<string>('');
const arquivo = ref<File | null>(null);
const analiseStore = useAnaliseStore();
const { carregando } = storeToRefs(analiseStore);
const fileInputRef = ref<HTMLInputElement | null>(null);

function handleFileChange(event: Event) {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    arquivo.value = target.files[0];
  }
}

function clearFile() {
  arquivo.value = null;
  if (fileInputRef.value) {
    fileInputRef.value.value = '';
  }
}

async function handleSubmit() {
  if (!arquivo.value || !contexto.value.trim()) {
    alert('Por favor, selecione um arquivo e descreva o objetivo da análise.');
    return;
  }

  const formData = new FormData();
  formData.append('arquivo', arquivo.value);
  formData.append('contexto', contexto.value);

  await analiseStore.analisarArquivo(formData);
}
</script>

<template>
  <div class="w-full">
    <form class="space-y-5" @submit.prevent="handleSubmit">
      <!-- Upload do Arquivo -->
      <div>
        <label class="block text-sm font-semibold text-gray-700 mb-1.5">
          1. Selecione sua planilha (.csv ou .xlsx)
        </label>

        <div
          @click="fileInputRef?.click()"
          class="relative border-2 border-dashed border-gray-300 hover:border-blue-500 rounded-xl p-4 sm:p-5 text-center cursor-pointer transition-colors bg-gray-50/70 hover:bg-blue-50/40"
        >
          <input
            ref="fileInputRef"
            id="file-upload"
            name="file-upload"
            type="file"
            accept=".csv,.xlsx"
            @change="handleFileChange"
            class="sr-only"
          />

          <div v-if="!arquivo" class="space-y-2">
            <svg class="mx-auto h-9 w-9 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" />
            </svg>
            <div class="text-sm text-gray-600">
              <span class="font-medium text-blue-600 underline">Toque para escolher</span> o arquivo
            </div>
            <p class="text-xs text-gray-400">Suporta arquivos .CSV ou .XLSX</p>
          </div>

          <div v-else class="flex items-center justify-between bg-white p-2.5 rounded-lg border border-blue-200">
            <div class="flex items-center space-x-2.5 overflow-hidden">
              <svg class="h-6 w-6 text-green-600 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              <div class="text-left truncate">
                <p class="text-sm font-medium text-gray-800 truncate">{{ arquivo.name }}</p>
                <p class="text-xs text-gray-500">{{ (arquivo.size / 1024).toFixed(1) }} KB</p>
              </div>
            </div>
            <button
              type="button"
              @click.stop="clearFile"
              class="p-1 text-gray-400 hover:text-red-500 rounded-full transition-colors flex-shrink-0 ml-2"
              title="Remover arquivo"
            >
              <svg class="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </div>
      </div>

      <!-- Contexto / Pergunta -->
      <div>
        <label for="context" class="block text-sm font-semibold text-gray-700 mb-1.5">
          2. Descreva o objetivo da sua análise
        </label>
        <textarea
          id="context"
          name="context"
          rows="3"
          v-model="contexto"
          class="block w-full p-3 border border-gray-300 rounded-xl shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-base sm:text-sm transition-all"
          placeholder="Ex: 'Analisar as vendas do último trimestre para encontrar meu produto mais vendido por cidade.'"
        ></textarea>
      </div>

      <!-- Botão de Ação -->
      <div>
        <button
          type="submit"
          :disabled="carregando || !arquivo || !contexto.trim()"
          :class="[
            'w-full flex justify-center items-center py-3 px-4 rounded-xl shadow-sm text-base font-semibold text-white transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500',
            carregando || !arquivo || !contexto.trim()
              ? 'bg-blue-300 cursor-not-allowed'
              : 'bg-blue-600 hover:bg-blue-700 active:scale-98 shadow-md hover:shadow-lg'
          ]"
        >
          <svg
            v-if="carregando"
            class="animate-spin -ml-1 mr-2 h-5 w-5 text-white"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
          >
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          {{ carregando ? 'Processando dados...' : 'Analisar Dados' }}
        </button>
      </div>
    </form>
  </div>
</template>