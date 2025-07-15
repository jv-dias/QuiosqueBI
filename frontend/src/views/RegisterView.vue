<script setup lang="ts">
import AuthForm from '@/components/AuthForm.vue';
import { useAuthStore } from '@/stores/authStore';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';
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

    <main class="flex-grow flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div class="max-w-4xl w-full bg-white rounded-2xl shadow-2xl overflow-hidden md:flex">

        <div class="w-full md:w-1/2 p-8 sm:p-12 bg-blue-600 text-white flex flex-col justify-center">
          <h2 class="text-3xl font-extrabold mb-4">Crie sua Conta Gratuita</h2>
          <p class="mb-8 text-blue-100">Junte-se ao QuiosqueBI e comece a transformar seus dados em decisões inteligentes. Ao se registrar, você terá acesso a:</p>
          <ul class="space-y-4">
            <li class="flex items-start">
              <svg class="h-6 w-6 mr-3 mt-1 text-blue-300" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" /></svg>
              <span>Análises ilimitadas com o poder da nossa IA generativa.</span>
            </li>
            <li class="flex items-start">
              <svg class="h-6 w-6 mr-3 mt-1 text-blue-300" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
              <span>Histórico pessoal para salvar e revisitar todos os seus relatórios.</span>
            </li>
            <li class="flex items-start">
              <svg class="h-6 w-6 mr-3 mt-1 text-blue-300" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
              <span>Um ambiente seguro e privado para seus dados.</span>
            </li>
          </ul>
        </div>

        <div class="w-full md:w-1/2 p-8 sm:p-12 flex flex-col justify-center">
          <AuthForm
            title="Complete seus dados"
            button-text="Criar minha conta"
            :is-registering="true"
            @submit="authStore.registrar"
          />
          <p class="mt-6 text-center text-sm text-gray-500">
            Já tem uma conta?
            <RouterLink to="/login" class="font-medium text-blue-600 hover:text-blue-500">
              Faça o login aqui
            </RouterLink>
          </p>
        </div>

      </div>
    </main>

    <Footer />
  </div>
</template>
