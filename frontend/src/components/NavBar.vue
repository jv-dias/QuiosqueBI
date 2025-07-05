<script setup lang="ts">
</script>

<template>
    <nav class="bg-blue-600 text-white shadow-lg">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div class="flex justify-between h-16">
                <div class="flex items-center">
                    <RouterLink to="/" class="text-xl font-bold hover:text-blue-200 transition-colors duration-200">
                        QuiosqueBI
                    </RouterLink>
                </div>
                <div class="flex items-center">
                    <RouterLink to="/debug"
                        class="bg-blue-700 hover:bg-blue-800 px-4 py-2 rounded-md text-sm font-medium transition-colors duration-200">
                        Painel de Depuração
                    </RouterLink>
                </div>
            </div>
        </div>
    </nav>
</template>
