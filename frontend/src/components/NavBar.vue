<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';
import { ref, watch, onMounted, onUnmounted } from 'vue';

const authStore = useAuthStore();
const { isAuthenticated, userFirstName } = storeToRefs(authStore);
const route = useRoute();

const handleLogout = () => {
  isDropdownOpen.value = false;
  isMobileMenuOpen.value = false;
  authStore.logout();
};

const isDropdownOpen = ref(false);
const isMobileMenuOpen = ref(false);

const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

const toggleMobileMenu = () => {
  isMobileMenuOpen.value = !isMobileMenuOpen.value;
};

// Fecha menus ao trocar de rota
watch(() => route.path, () => {
  isDropdownOpen.value = false;
  isMobileMenuOpen.value = false;
});

// Fechar menus ao clicar fora
const navRef = ref<HTMLElement | null>(null);
const handleClickOutside = (e: MouseEvent) => {
  if (navRef.value && !navRef.value.contains(e.target as Node)) {
    isDropdownOpen.value = false;
    isMobileMenuOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});
</script>

<template>
  <nav ref="navRef" class="bg-white border-b border-gray-200 shadow-sm sticky top-0 z-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16 items-center">
        <!-- Logo e Links Desktop -->
        <div class="flex items-center space-x-6 lg:space-x-8">
          <RouterLink to="/" class="flex items-center flex-shrink-0 hover:opacity-85 transition-opacity duration-200">
            <img src="/src/assets/icon-azul2.webp" alt="QuiosqueBI" class="h-9 w-9 sm:h-11 sm:w-11 object-contain" />
            <span class="text-lg sm:text-xl font-bold text-gray-800 ml-2.5 tracking-tight">QuiosqueBI</span>
          </RouterLink>

          <!-- Links Desktop (md+) -->
          <div v-if="isAuthenticated" class="hidden md:flex md:space-x-2">
            <RouterLink
              to="/analise"
              :class="[route.name === 'analise' ? 'border-blue-600 text-blue-600 font-semibold' : 'border-transparent text-gray-600 hover:border-gray-300 hover:text-gray-900']"
              class="inline-flex items-center px-3 pt-1 border-b-2 text-sm font-medium transition-colors"
            >
              Nova Análise
            </RouterLink>
            <RouterLink
              to="/historico"
              :class="[route.name === 'historico' || route.name === 'detalhe-analise' ? 'border-blue-600 text-blue-600 font-semibold' : 'border-transparent text-gray-600 hover:border-gray-300 hover:text-gray-900']"
              class="inline-flex items-center px-3 pt-1 border-b-2 text-sm font-medium transition-colors"
            >
              Histórico
            </RouterLink>
          </div>
        </div>

        <!-- Ações Desktop (md+) -->
        <div class="hidden md:flex md:items-center md:space-x-4">
          <template v-if="!isAuthenticated">
            <RouterLink
              v-if="route.name !== 'login'"
              to="/login"
              class="text-gray-600 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium transition-colors"
            >
              Login
            </RouterLink>
            <RouterLink
              v-if="route.name !== 'register'"
              to="/register"
              class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors shadow-sm"
            >
              Registrar
            </RouterLink>
          </template>

          <div v-else class="relative">
            <button
              @click.stop="toggleDropdown"
              class="flex items-center space-x-2 p-1.5 rounded-full hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors"
              aria-label="Menu do usuário"
            >
              <span class="text-gray-700 font-medium text-sm">Olá, {{ userFirstName }}</span>
              <span class="inline-flex items-center justify-center h-8 w-8 rounded-full bg-blue-600 text-white shadow-sm font-semibold text-xs">
                {{ userFirstName ? userFirstName.charAt(0).toUpperCase() : 'U' }}
              </span>
            </button>

            <div
              v-if="isDropdownOpen"
              class="origin-top-right absolute right-0 mt-2 w-48 rounded-lg shadow-lg py-1.5 bg-white ring-1 ring-black/5 z-20"
            >
              <div class="px-4 py-2 border-b border-gray-100 text-xs text-gray-500 font-medium">
                Conectado como <span class="font-semibold text-gray-700">{{ userFirstName }}</span>
              </div>
              <RouterLink
                to="/analise"
                class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-50 transition-colors"
              >
                Nova Análise
              </RouterLink>
              <RouterLink
                to="/historico"
                class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-50 transition-colors"
              >
                Meu Histórico
              </RouterLink>
              <div class="border-t border-gray-100 my-1"></div>
              <button
                @click="handleLogout"
                class="w-full text-left block px-4 py-2 text-sm text-red-600 hover:bg-red-50 transition-colors"
              >
                Sair da conta
              </button>
            </div>
          </div>
        </div>

        <!-- Botão Hambúrguer para Mobile (< md) -->
        <div class="flex items-center md:hidden">
          <button
            @click.stop="toggleMobileMenu"
            type="button"
            class="inline-flex items-center justify-center p-2.5 rounded-md text-gray-700 hover:text-blue-600 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-blue-500 transition-colors"
            :aria-expanded="isMobileMenuOpen"
            aria-label="Abrir menu principal"
          >
            <!-- Ícone Menu Fechado (Hambúrguer) -->
            <svg
              v-if="!isMobileMenuOpen"
              class="block h-6 w-6"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              aria-hidden="true"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
            <!-- Ícone Menu Aberto (X) -->
            <svg
              v-else
              class="block h-6 w-6 text-gray-800"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              aria-hidden="true"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>
    </div>

    <!-- Menu Mobile Expansível (< md) -->
    <transition
      enter-active-class="transition duration-200 ease-out"
      enter-from-class="opacity-0 -translate-y-2"
      enter-to-class="opacity-100 translate-y-0"
      leave-active-class="transition duration-150 ease-in"
      leave-from-class="opacity-100 translate-y-0"
      leave-to-class="opacity-0 -translate-y-2"
    >
      <div
        v-if="isMobileMenuOpen"
        class="md:hidden border-t border-gray-200 bg-white px-4 pt-3 pb-5 space-y-3 shadow-lg"
      >
        <!-- Usuário Conectado -->
        <template v-if="isAuthenticated">
          <div class="flex items-center space-x-3 pb-3 border-b border-gray-100 px-2 pt-1">
            <span class="inline-flex items-center justify-center h-10 w-10 rounded-full bg-blue-600 text-white font-semibold text-sm shadow-sm">
              {{ userFirstName ? userFirstName.charAt(0).toUpperCase() : 'U' }}
            </span>
            <div>
              <p class="text-sm font-semibold text-gray-800">Olá, {{ userFirstName }}</p>
              <p class="text-xs text-gray-500">Conectado ao QuiosqueBI</p>
            </div>
          </div>

          <div class="space-y-1">
            <RouterLink
              to="/analise"
              :class="[route.name === 'analise' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-700 hover:bg-gray-50']"
              class="flex items-center px-3 py-2.5 rounded-lg text-base font-medium transition-colors"
            >
              <svg class="w-5 h-5 mr-3 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
              </svg>
              Nova Análise
            </RouterLink>

            <RouterLink
              to="/historico"
              :class="[route.name === 'historico' || route.name === 'detalhe-analise' ? 'bg-blue-50 text-blue-700 font-semibold' : 'text-gray-700 hover:bg-gray-50']"
              class="flex items-center px-3 py-2.5 rounded-lg text-base font-medium transition-colors"
            >
              <svg class="w-5 h-5 mr-3 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              Histórico de Análises
            </RouterLink>
          </div>

          <div class="pt-2 border-t border-gray-100">
            <button
              @click="handleLogout"
              class="w-full flex items-center justify-center px-4 py-2.5 rounded-lg text-base font-medium text-red-600 bg-red-50 hover:bg-red-100 transition-colors"
            >
              <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
              </svg>
              Sair da Conta
            </button>
          </div>
        </template>

        <!-- Usuário Desconectado -->
        <template v-else>
          <div class="space-y-2 pt-1">
            <RouterLink
              to="/login"
              class="block w-full text-center px-4 py-2.5 border border-gray-300 rounded-lg text-base font-medium text-gray-700 hover:bg-gray-50 transition-colors"
            >
              Login
            </RouterLink>
            <RouterLink
              to="/register"
              class="block w-full text-center px-4 py-2.5 border border-transparent rounded-lg text-base font-semibold text-white bg-blue-600 hover:bg-blue-700 transition-colors shadow-sm"
            >
              Registrar-se Gratuitamente
            </RouterLink>
          </div>
        </template>
      </div>
    </transition>
  </nav>
</template>

<style scoped>
img {
  max-height: 44px;
}
</style>