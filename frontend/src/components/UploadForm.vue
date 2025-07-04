<script setup lang="ts">
// DENTRO DE src/components/UploadForm.vue
import { ref } from 'vue'
import { useAnaliseStore } from '@/stores/analiseStore' // Importa nossa store

const contexto = ref<string>('')
const arquivo = ref<File | null>(null)
const analiseStore = useAnaliseStore() // Inicializa a store

function handleFileChange(event: Event) {
  const target = event.target as HTMLInputElement
  if (target.files && target.files.length > 0) {
    arquivo.value = target.files[0]
  }
}

async function handleSubmit() {
  console.log('1. Função handleSubmit foi chamada.'); // <-- PONTO DE VERIFICAÇÃO 1

  if (!arquivo.value || !contexto.value) {
    alert('Por favor, preencha todos os campos.')
    return
  }

  const formData = new FormData()
  formData.append('arquivo', arquivo.value)
  formData.append('contexto', contexto.value)

  console.log('2. Prestes a chamar a action da store...'); // <-- PONTO DE VERIFICAÇÃO 2
  await analiseStore.analisarArquivo(formData)
  console.log('4. Chamada da action concluída.'); // <-- PONTO DE VERIFICAÇÃO 4
}
</script>

<template>
  <div class="max-w-2xl mx-auto p-8 bg-slate-50 rounded-lg shadow-md">
    <form class="space-y-6" @submit.prevent="handleSubmit">
      <div>
        <label for="file-upload" class="block text-sm font-medium text-gray-700">
          1. Selecione sua planilha (.csv)
        </label>
        <div class="mt-1">
          <input id="file-upload" name="file-upload" type="file" accept=".csv" @change="handleFileChange" class="block w-full text-sm text-slate-500
                   file:mr-4 file:py-2 file:px-4
                   file:rounded-md file:border-0
                   file:text-sm file:font-semibold
                   file:bg-blue-50 file:text-blue-700
                   hover:file:bg-blue-100" />
        </div>
      </div>

      <div>
        <label for="context" class="block text-sm font-medium text-gray-700">
          2. Descreva o objetivo da sua análise
        </label>
        <div class="mt-1">
          <textarea id="context" name="context" rows="4" v-model="contexto"
            class="block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
            placeholder="Ex: 'Analisar as vendas do último trimestre para encontrar meu produto mais vendido por cidade.'"></textarea>
        </div>
      </div>

      <div>
        <button type="submit"
          class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500">
          Analisar Dados
        </button>
      </div>
    </form>
  </div>
</template>