<script setup lang="ts">
import AuthForm from '@/components/AuthForm.vue';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';
import { useAuthStore } from '@/stores/authStore';
import { RouterLink } from 'vue-router';
import { onMounted } from 'vue';

const authStore = useAuthStore();

// Limpa qualquer erro anterior ao montar o componente
onMounted(() => {
  authStore.error = null;
});
</script>

<template>
  <div class="bg-gray-100 min-h-screen flex flex-col">
    <NavBar />

    <main class="flex-grow flex items-center justify-center py-6 sm:py-12 px-3 sm:px-6 lg:px-8">
      <div class="max-w-4xl w-full bg-white rounded-2xl shadow-xl overflow-hidden md:flex">

        <div class="w-full md:w-1/2 p-6 sm:p-10 md:p-12 bg-blue-600 text-white flex flex-col justify-center text-center md:text-left">
          <h2 class="text-2xl sm:text-3xl font-extrabold mb-2 sm:mb-4">Bem-vindo de Volta!</h2>
          <p class="mb-4 sm:mb-8 text-blue-100 text-sm sm:text-base">
            Faça login para acessar seu painel e continuar a transformar dados em decisões.
          </p>
          <ul class="hidden md:block space-y-4 text-sm">
            <li class="flex items-center">
              <svg class="h-5 w-5 mr-2 text-blue-300 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              <span>Acesse seu histórico de análises.</span>
            </li>
            <li class="flex items-center">
              <svg class="h-5 w-5 mr-2 text-blue-300 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              <span>Crie novos relatórios visuais em segundos.</span>
            </li>
            <li class="flex items-center">
              <svg class="h-5 w-5 mr-2 text-blue-300 flex-shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              <span>Continue seu trabalho de onde parou.</span>
            </li>
          </ul>
        </div>

        <div class="w-full md:w-1/2 p-4 sm:p-8 md:p-12 flex flex-col justify-center">
          <AuthForm title="Acessar sua Conta" button-text="Entrar" @submit="authStore.login" />
          <p class="mt-4 sm:mt-6 text-center text-sm text-gray-600">
            Não tem uma conta?
            <RouterLink to="/register" class="font-semibold text-blue-600 hover:text-blue-500 ml-1">
              Registre-se agora
            </RouterLink>
          </p>
        </div>

      </div>
    </main>

    <Footer />
  </div>
</template>