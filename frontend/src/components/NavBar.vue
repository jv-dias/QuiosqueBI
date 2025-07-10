<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';

const authStore = useAuthStore();
const { isAuthenticated } = storeToRefs(authStore);
const route = useRoute();

const handleLogout = () => {
    authStore.logout();
};
</script>

<template>
    <nav class="bg-white border-b border-gray-200 shadow-sm sticky top-0 z-50">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="flex justify-between h-16">

                <div class="flex items-center">
                    <RouterLink to="/" class="flex items-center hover:opacity-80 transition-opacity duration-200">
                        <img src="/src/assets/icon-azul2.webp" alt="QuiosqueBI" class="h-12 w-12 object-contain" />
                        <span class="text-xl font-bold text-gray-800 ml-3">QuiosqueBI</span>
                    </RouterLink>
                </div>

                <div class="flex items-center space-x-4">

                    <template v-if="isAuthenticated">
                        <RouterLink v-if="route.name !== 'analise'" to="/analise"
                            class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors shadow-sm">
                            Nova Análise
                        </RouterLink>
                        <RouterLink v-if="route.name !== 'historico'" to="/historico"
                            class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors shadow-sm">
                            Histórico
                        </RouterLink>
                        <button @click="handleLogout"
                            class="bg-red-500 hover:bg-red-600 text-white px-3 py-2 rounded-md text-sm font-medium transition-colors">
                            Logout
                        </button>
                    </template>

                    <template v-else>
                        <RouterLink v-if="route.name !== 'login'" to="/login"
                            class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors shadow-sm">
                            Login
                        </RouterLink>
                        <RouterLink v-if="route.name !== 'register'" to="/register"
                            class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded-md text-sm font-medium transition-colors shadow-sm">
                            Registrar
                        </RouterLink>
                    </template>

                </div>
            </div>
        </div>
    </nav>
</template>

<style scoped>
/* Opcional: Apenas para garantir que a imagem não exceda a altura da navbar */
img {
    max-height: 48px;
}
</style>