<script setup lang="ts">
import AuthForm from '@/components/AuthForm.vue';
import { useAuthStore } from '@/stores/authStore';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';

const authStore = useAuthStore();
</script>

<template>
  <NavBar />
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <AuthForm 
      title="Registro"
      button-text="Registrar"
      :is-registering="true"
      @submit="authStore.registrar"
    />
  </div>
  <Footer />
</template>