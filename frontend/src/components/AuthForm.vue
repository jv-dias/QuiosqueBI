<script setup lang="ts">
import { ref, defineProps, defineEmits } from 'vue';

// 1. Definimos as 'props' que o componente pode receber de fora
const props = defineProps({
  title: {
    type: String,
    required: true,
  },
  buttonText: {
    type: String,
    required: true,
  },
  isRegistering: { // Prop para saber se devemos mostrar o campo "Confirmar Senha"
    type: Boolean,
    default: false,
  },
});

// 2. Definimos os eventos que o componente pode emitir para o pai
const emit = defineEmits(['submit']);


const nome = ref('');
const sobrenome = ref('');
const email = ref('');
const password = ref('');
const confirmPassword = ref('');

const handleSubmit = () => {
  if (props.isRegistering && password.value !== confirmPassword.value) {
    alert('As senhas não coincidem!');
    return;
  }

const payload = {
  nome: nome.value,
  sobrenome: sobrenome.value,
  email: email.value,
  password: password.value
  };
  emit('submit', payload);
};
</script>

<template>
  <div class="p-8 bg-white rounded-lg shadow-md w-96">
    <h2 class="text-2xl font-bold text-center mb-6">{{ props.title }}</h2>
    <form @submit.prevent="handleSubmit" class="space-y-6">
        <div v-if="props.isRegistering" class="flex space-x-4">
        <div class="w-1/2">
          <label for="nome" class="block text-sm font-medium text-gray-700">Nome</label>
          <input v-model="nome" type="text" id="nome" class="mt-1 block w-full p-2 border border-gray-300 rounded-md" required />
        </div>
        <div class="w-1/2">
          <label for="sobrenome" class="block text-sm font-medium text-gray-700">Sobrenome</label>
          <input v-model="sobrenome" type="text" id="sobrenome" class="mt-1 block w-full p-2 border border-gray-300 rounded-md" required />
        </div>
      </div>
      <div>
        <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
        <input v-model="email" type="email" id="email" class="mt-1 block w-full p-2 border border-gray-300 rounded-md" required />
      </div>
      <div>
        <label for="password" class="block text-sm font-medium text-gray-700">Senha</label>
        <input v-model="password" type="password" id="password" class="mt-1 block w-full p-2 border border-gray-300 rounded-md" required />
      </div>

      <div v-if="props.isRegistering">
        <label for="confirmPassword" class="block text-sm font-medium text-gray-700">Confirmar Senha</label>
        <input v-model="confirmPassword" type="password" id="confirmPassword" class="mt-1 block w-full p-2 border border-gray-300 rounded-md" required />
      </div>
      
      <div>
        <button type="submit" class="w-full py-2 px-4 bg-blue-600 text-white rounded-md hover:bg-blue-700">{{ props.buttonText }}</button>
      </div>
    </form>
  </div>
</template>