<script setup lang="ts">
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
                <div class="flex items-center">
                    <RouterLink to="/debug"
                        class="bg-blue-700 hover:bg-blue-800 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200">
                        Painel de Depuração
                    </RouterLink>
                    <RouterLink to="/historico"
                        class="ml-4 bg-blue-700 hover:bg-blue-800 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200">
                        Histórico de Análises
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
    max-width: 200px;
}

/* Adicionar espaçamento entre imagem e texto */
.flex.items-center img {
    margin-right: 12px;
}
</style>
