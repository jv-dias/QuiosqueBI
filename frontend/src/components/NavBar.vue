<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';


const authStore = useAuthStore();
const { isAuthenticated } = storeToRefs(authStore);
const route = useRoute();

// Função para lidar com o logout
const handleLogout = () => {
  authStore.logout();
};
</script>

<template>
    <nav class="bg-blue-600 text-white shadow-lg">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="flex justify-between h-20">
                <div class="flex items-center">
                    <RouterLink to="/" class="flex items-center hover:opacity-80 transition-opacity duration-200">
                        <img src="/src/assets/icon.webp" alt="QuiosqueBI" class="h-14 w-14 object-contain mr-3" />
                        <span class="text-xl font-bold text-white">QuiosqueBI</span>
                    </RouterLink>
                </div>
                <div class="flex items-center space-x-4">
                    
                    <template v-if="isAuthenticated">
                        <RouterLink to="/historico"
                            class="px-4 py-2 rounded-md text-sm font-medium hover:bg-blue-700 transition-colors duration-200">
                            Histórico
                        </RouterLink>
                        <button @click="handleLogout"
                            class="bg-red-500 hover:bg-red-600 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200">
                            Logout
                        </button>
                    </template>
                    
                    <template v-else>
                        <RouterLink v-if="route.name !== 'login'" to="/login"
                            class="px-4 py-2 rounded-md text-sm font-medium hover:bg-blue-700 transition-colors duration-200">
                            Login
                        </RouterLink>
                        <RouterLink v-if="route.name !== 'register'" to="/register"
                            class="bg-white text-blue-600 hover:bg-gray-200 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200">
                            Registrar
                        </RouterLink>
                    </template>

                    <RouterLink to="/debug"
                        class="bg-gray-700 hover:bg-gray-800 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200">
                        Debug
                    </RouterLink>

                </div>
            </div>
        </div>
    </nav>
</template>

<style scoped>
/* Garantir que a imagem não quebre o layout em diferentes tamanhos */
img {
    max-height: 56px;
}
</style>