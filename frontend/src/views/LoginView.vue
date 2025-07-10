<script setup lang="ts">
import AuthForm from '@/components/AuthForm.vue';
import NavBar from '@/components/NavBar.vue';
import Footer from '@/components/Footer.vue';
import { useAuthStore } from '@/stores/authStore';

const authStore = useAuthStore();
</script>

<template>
  <NavBar />
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <AuthForm 
      title="Login" 
      button-text="Entrar"
      @submit="authStore.login" 
    />
  </div>
  <Footer />
</template>