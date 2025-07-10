<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';
import { ref } from 'vue'; // Importe o ref para o dropdown

const authStore = useAuthStore();
const { isAuthenticated, userFirstName } = storeToRefs(authStore);
const route = useRoute();

const handleLogout = () => {
  authStore.logout();
};

// Lógica para controlar a abertura do menu dropdown
const isDropdownOpen = ref(false);
const toggleDropdown = () => {
    isDropdownOpen.value = !isDropdownOpen.value;
};
</script>

<template>
    <nav class="bg-white border-b border-gray-200 shadow-sm sticky top-0 z-50">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="flex justify-between h-16">
                
                <div class="flex items-center space-x-8">
                    <RouterLink to="/" class="flex items-center flex-shrink-0 hover:opacity-80 transition-opacity duration-200">
                        <img src="/src/assets/icon-azul2.webp" alt="QuiosqueBI" class="h-12 w-12 object-contain" />
                        <span class="text-xl font-bold text-gray-800 ml-3">QuiosqueBI</span>
                    </RouterLink>
                    
                    <div v-if="isAuthenticated" class="hidden sm:flex sm:space-x-4">
                        <RouterLink to="/analise" 
                            :class="[route.name === 'analise' ? 'border-blue-500 text-gray-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
                            class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                            Nova Análise
                        </RouterLink>
                        <RouterLink to="/historico" 
                            :class="[route.name === 'historico' ? 'border-blue-500 text-gray-900' : 'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700']"
                            class="inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                            Histórico
                        </RouterLink>
                    </div>
                </div>
                
                <div class="flex items-center">
                    <div v-if="!isAuthenticated" class="flex items-center space-x-4">
                        <RouterLink v-if="route.name !== 'login'" to="/login" class="text-gray-600 hover:text-blue-600 px-3 py-2 rounded-md text-sm font-medium transition-colors">
                            Login
                        </RouterLink>
                        <RouterLink v-if="route.name !== 'register'" to="/register" class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors shadow-sm">
                            Registrar
                        </RouterLink>
                    </div>

                    <div v-if="isAuthenticated" class="relative">
                        <button @click="toggleDropdown" class="flex items-center space-x-2 p-2 rounded-full hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500">
                             <span class="text-gray-700 font-medium text-sm hidden sm:block">Olá, {{ userFirstName }}</span>
                             <span class="inline-flex items-center justify-center h-8 w-8 rounded-full bg-blue-500">
                                <span class="text-sm font-medium leading-none text-white">{{ userFirstName.charAt(0).toUpperCase() }}</span>
                            </span>
                        </button>
                        
                        <div v-if="isDropdownOpen" @click.away="isDropdownOpen = false" class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 z-10">                            
                            <a href="#" @click.prevent="handleLogout" class="block px-4 py-2 text-sm text-red-600 hover:bg-red-50">Logout</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </nav>
</template>

<style scoped>
img {
    max-height: 48px;
}
</style>