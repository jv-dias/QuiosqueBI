<script setup lang="ts">
import { RouterLink } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';
import Footer from '@/components/Footer.vue';
import NavBar from '@/components/NavBar.vue';

const authStore = useAuthStore();
const { isAuthenticated } = storeToRefs(authStore);
</script>

<template>
  <NavBar />
  <div class="bg-white">
    <main>
        <div class="relative bg-gray-900">
          <div aria-hidden="true" class="absolute inset-0 overflow-hidden">
          <img src="/src/assets/bg-dados.webp" alt="Fundo abstrato com gráficos" class="w-full h-full object-cover object-center opacity-25">
        </div>        
        <div aria-hidden="true" class="absolute inset-0 bg-gray-900 opacity-50"></div>

        <div class="relative max-w-4xl mx-auto py-14 px-4 sm:px-6 sm:py-24 lg:py-32 text-center">
          <h1 class="text-3xl sm:text-5xl lg:text-6xl font-extrabold tracking-tight text-white leading-tight">
            Transforme Dados Brutos em Decisões Brilhantes
          </h1>
          <p class="mt-4 sm:mt-6 max-w-2xl mx-auto text-base sm:text-xl text-gray-300">
            Pare de lutar com planilhas. Descreva o que você precisa em português claro e deixe nossa IA criar os gráficos e relatórios para você em segundos.
          </p>
          <div class="mt-8 sm:mt-12 flex justify-center">
            <RouterLink
              v-if="isAuthenticated"
              to="/analise"
              class="w-full sm:w-auto text-center bg-blue-600 border border-transparent rounded-lg py-3.5 px-8 font-semibold text-white hover:bg-blue-700 transition-all duration-200 shadow-lg hover:shadow-xl active:scale-95"
            >
              Ir para o seu Painel
            </RouterLink>
            <RouterLink
              v-else
              to="/register"
              class="w-full sm:w-auto text-center bg-blue-600 border border-transparent rounded-lg py-3.5 px-6 sm:px-8 font-semibold text-white hover:bg-blue-700 transition-all duration-200 shadow-lg hover:shadow-xl active:scale-95 text-sm sm:text-base"
            >
              Criar Conta Gratuita e Analisar
            </RouterLink>
          </div>
        </div>
      </div>

      <div class="py-12 sm:py-16 bg-gray-50 overflow-hidden">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
          <div class="text-center">
            <h2 class="text-2xl sm:text-3xl font-extrabold text-gray-900">Como funciona em 3 passos simples</h2>
            <p class="mt-2 text-base text-gray-500">Inteligência sem complicação.</p>
          </div>
          <div class="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-3">
            <div class="text-center p-6 bg-white rounded-xl shadow-sm border border-gray-100">
              <div class="p-3.5 bg-blue-600 rounded-2xl inline-block text-white mb-4 shadow-sm">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" /></svg>
              </div>
              <h3 class="text-lg sm:text-xl font-bold text-gray-900">1. Envie sua Planilha</h3>
              <p class="mt-2 text-sm sm:text-base text-gray-500">Faça o upload do seu arquivo CSV ou XLSX de forma segura.</p>
            </div>
            <div class="text-center p-6 bg-white rounded-xl shadow-sm border border-gray-100">
               <div class="p-3.5 bg-blue-600 rounded-2xl inline-block text-white mb-4 shadow-sm">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 8h10M7 12h4m1 8l-4-4H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-3l-4 4z" /></svg>
              </div>
              <h3 class="text-lg sm:text-xl font-bold text-gray-900">2. Descreva seu Objetivo</h3>
              <p class="mt-2 text-sm sm:text-base text-gray-500">Diga o que você quer analisar. Ex: "Vendas por produto".</p>
            </div>
            <div class="text-center p-6 bg-white rounded-xl shadow-sm border border-gray-100">
              <div class="p-3.5 bg-blue-600 rounded-2xl inline-block text-white mb-4 shadow-sm">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
              </div>
              <h3 class="text-lg sm:text-xl font-bold text-gray-900">3. Receba seus Insights</h3>
              <p class="mt-2 text-sm sm:text-base text-gray-500">Visualize gráficos e relatórios gerados instantaneamente.</p>
            </div>
          </div>
        </div>
      </div>
      
      <div class="bg-white py-12 sm:py-16">
        <div class="max-w-3xl mx-auto text-center px-4 sm:px-6 lg:px-8">
          <p class="text-lg sm:text-2xl font-medium text-gray-900 leading-relaxed italic">
            "O QuiosqueBI economizou horas de trabalho manual no Excel. O que levava uma tarde inteira para ser feito, agora leva segundos. Simplesmente incrível."
          </p>
          <footer class="mt-6">
            <p class="text-sm sm:text-base font-bold text-blue-600">Mãe do criador do projeto. (FONTE 100% CONFIÁVEL)</p>
          </footer>
        </div>
      </div>
      
      <div class="bg-gray-50">
        <div class="max-w-4xl mx-auto text-center py-12 px-4 sm:py-20 sm:px-6 lg:px-8">
          <h2 class="text-2xl sm:text-4xl font-extrabold text-gray-900">
            <span class="block">Pronto para simplificar suas análises?</span>
          </h2>
          <p class="mt-3 sm:mt-4 text-base sm:text-lg text-gray-500">Crie sua conta e comece a transformar dados em decisões hoje mesmo.</p>
          <div class="mt-8 flex justify-center">
            <RouterLink
              v-if="isAuthenticated"
              to="/analise"
              class="w-full sm:w-auto text-center bg-blue-600 border border-transparent rounded-lg py-3.5 px-8 font-semibold text-white hover:bg-blue-700 transition-all duration-200 shadow-md active:scale-95"
            >
              Começar a Analisar
            </RouterLink>
            <RouterLink
              v-else
              to="/register"
              class="w-full sm:w-auto text-center bg-blue-600 border border-transparent rounded-lg py-3.5 px-8 font-semibold text-white hover:bg-blue-700 transition-all duration-200 shadow-md active:scale-95"
            >
              Criar Conta Gratuita
            </RouterLink>
          </div>
        </div>
      </div>

    </main>
  </div>
  <Footer />
</template>