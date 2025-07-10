<script setup lang="ts">
import { RouterLink } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';
import Footer from '@/components/Footer.vue';

const authStore = useAuthStore();
const { isAuthenticated } = storeToRefs(authStore);
</script>

<template>
  <div class="bg-white">
    <main>
        <div class="relative bg-gray-900">
          <div aria-hidden="true" class="absolute inset-0 overflow-hidden">
          <img src="/src/assets/bg-dados.webp" alt="Fundo abstrato com gráficos" class="w-full h-full object-cover object-center opacity-25">
        </div>        
        <div aria-hidden="true" class="absolute inset-0 bg-gray-900 opacity-50"></div>

        <div class="relative max-w-4xl mx-auto py-24 px-4 sm:px-6 sm:py-32 lg:px-8 text-center">
          <h1 class="text-4xl font-extrabold tracking-tight text-white sm:text-5xl lg:text-6xl">
            Transforme Dados Brutos em Decisões Brilhantes
          </h1>
          <p class="mt-6 max-w-2xl mx-auto text-xl text-gray-300">
            Pare de lutar com planilhas. Descreva o que você precisa em português claro e deixe nossa IA criar os gráficos e relatórios para você em segundos.
          </p>
          <div class="mt-12">
            <RouterLink v-if="isAuthenticated" to="/analise" class="inline-block bg-blue-600 border border-transparent rounded-md py-3 px-8 font-medium text-white hover:bg-blue-700 transition-transform duration-300 ease-in-out hover:scale-105">
              Ir para o seu Painel
            </RouterLink>
            <RouterLink v-else to="/register" class="inline-block bg-blue-600 border border-transparent rounded-md py-3 px-8 font-medium text-white hover:bg-blue-700 transition-transform duration-300 ease-in-out hover:scale-105">
              Crie sua Conta Grátis e Comece a Analisar
            </RouterLink>
          </div>
        </div>
      </div>

      <div class="py-16 bg-gray-50 overflow-hidden">
        <div class="max-w-7xl mx-auto px-4 space-y-8 sm:px-6 lg:px-8">
          <div class="text-center">
            <h2 class="text-3xl font-extrabold text-gray-900">Como funciona em 3 passos simples</h2>
            <p class="mt-4 text-lg text-gray-500">Inteligência sem complicação.</p>
          </div>
          <div class="grid grid-cols-1 gap-12 sm:grid-cols-2 lg:grid-cols-3">
            <div class="text-center">
              <div class="p-4 bg-blue-600 rounded-full inline-block text-white mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" /></svg>
              </div>
              <h3 class="text-xl font-bold">1. Envie sua Planilha</h3>
              <p class="mt-2 text-gray-500">Faça o upload do seu arquivo CSV ou XLSX de forma segura.</p>
            </div>
            <div class="text-center">
               <div class="p-4 bg-blue-600 rounded-full inline-block text-white mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 8h10M7 12h4m1 8l-4-4H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-3l-4 4z" /></svg>
              </div>
              <h3 class="text-xl font-bold">2. Descreva seu Objetivo</h3>
              <p class="mt-2 text-gray-500">Diga o que você quer analisar. Ex: "Vendas por produto".</p>
            </div>
            <div class="text-center">
              <div class="p-4 bg-blue-600 rounded-full inline-block text-white mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
              </div>
              <h3 class="text-xl font-bold">3. Receba seus Insights</h3>
              <p class="mt-2 text-gray-500">Visualize gráficos e relatórios gerados instantaneamente.</p>
            </div>
          </div>
        </div>
      </div>
      
      <div class="bg-white py-16">
        <div class="max-w-3xl mx-auto text-center">
          <p class="text-2xl font-medium text-gray-900">
            "O QuiosqueBI economizou horas de trabalho manual no Excel. O que levava uma tarde inteira para ser feito, agora leva segundos. Simplesmente incrível."
          </p>
          <footer class="mt-8">
            <p class="text-base font-bold text-blue-600">Mãe do criador do projeto. (FONTE 100% CONFIÁVEL)</p>
          </footer>
        </div>
      </div>
      
      <div class="bg-gray-50">
        <div class="max-w-4xl mx-auto text-center py-16 px-4 sm:py-20 sm:px-6 lg:px-8">
          <h2 class="text-3xl font-extrabold text-gray-900 sm:text-4xl">
            <span class="block">Pronto para simplificar suas análises?</span>
          </h2>
          <p class="mt-4 text-lg leading-6 text-gray-500">Crie sua conta e comece a transformar dados em decisões hoje mesmo.</p>
            <div class="mt-8">
                <RouterLink v-if="isAuthenticated" to="/analise" class="inline-block bg-blue-600 border border-transparent rounded-md py-3 px-8 font-medium text-white hover:bg-blue-700 transition-transform duration-300 ease-in-out hover:scale-105">
                Começar a Analisar
                </RouterLink>
                <RouterLink v-else to="/register" class="inline-block bg-blue-600 border border-transparent rounded-md py-3 px-8 font-medium text-white hover:bg-blue-700 transition-transform duration-300 ease-in-out hover:scale-105">
                Criar Conta Gratuita
                </RouterLink>
            </div>
        </div>
      </div>

    </main>
  </div>
  <Footer />
</template>