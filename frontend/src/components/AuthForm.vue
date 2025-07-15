<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import { useAuthStore } from '@/stores/authStore';
import { RouterLink } from 'vue-router';

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

const authStore = useAuthStore();

const nome = ref('');
const sobrenome = ref('');
const email = ref('');
const password = ref('');
const confirmPassword = ref('');
const formError = ref('');

// Watch para sincronizar erros da store com o componente
watch(() => authStore.error, (newError) => {
  if (newError) {
    formError.value = newError;
  } else {
    formError.value = '';
  }
});

// Computed para verificar se a mensagem contém texto sobre email duplicado
const isEmailDuplicateError = computed(() => {
  const errorMessage = formError.value || authStore.error || '';
  return errorMessage.includes('Este email já está cadastrado');
});

// Validação básica de email
const isEmailValid = computed(() => {
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return !email.value || emailPattern.test(email.value);
});

// Validação de força de senha
const isPasswordStrong = computed(() => {
  return !password.value || password.value.length >= 6;
});

// Validação de confirmação de senha
const doPasswordsMatch = computed(() => {
  return !confirmPassword.value || password.value === confirmPassword.value;
});

// Validação do formulário como um todo
const isFormValid = computed(() => {
  if (props.isRegistering) {
    return nome.value && sobrenome.value && email.value && isEmailValid.value &&
      password.value && isPasswordStrong.value &&
      confirmPassword.value && doPasswordsMatch.value;
  } else {
    return email.value && isEmailValid.value && password.value;
  }
});

function submitForm() {
  formError.value = '';

  // Validações personalizadas
  if (props.isRegistering) {
    if (!isEmailValid.value) {
      formError.value = 'Por favor, informe um email válido.';
      return;
    }

    if (!isPasswordStrong.value) {
      formError.value = 'A senha deve ter pelo menos 6 caracteres.';
      return;
    }

    if (!doPasswordsMatch.value) {
      formError.value = 'As senhas não conferem.';
      return;
    }

    if (!nome.value || !sobrenome.value) {
      formError.value = 'Nome e sobrenome são obrigatórios.';
      return;
    }

    emit('submit', { nome: nome.value, sobrenome: sobrenome.value, email: email.value, password: password.value });
  } else {
    emit('submit', { email: email.value, password: password.value });
  }
}
</script>

<template>
  <div class="p-8 bg-white rounded-lg shadow-md w-96">
    <h2 class="text-2xl font-bold text-center mb-6">{{ props.title }}</h2>

    <!-- Exibição de erro -->
    <div v-if="formError || authStore.error" class="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
      <template v-if="isEmailDuplicateError">
        Este email já está cadastrado. Por favor, tente outro email ou faça
        <RouterLink to="/login" class="font-medium text-blue-600 hover:text-blue-500 underline">
          login
        </RouterLink>.
      </template>
      <template v-else>
        {{ formError || authStore.error }}
      </template>
    </div>

    <form @submit.prevent="submitForm" class="space-y-6">
      <!-- Campos de Registro -->
      <template v-if="isRegistering">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label for="nome" class="block text-sm font-medium text-gray-700">Nome</label>
            <input type="text" id="nome" v-model="nome" required
              class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm" />
          </div>
          <div>
            <label for="sobrenome" class="block text-sm font-medium text-gray-700">Sobrenome</label>
            <input type="text" id="sobrenome" v-model="sobrenome" required
              class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm" />
          </div>
        </div>
      </template>

      <!-- Email -->
      <div>
        <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
        <input type="email" id="email" v-model="email" required
          :class="['mt-1 block w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none sm:text-sm',
            isEmailValid ? 'border-gray-300 focus:ring-blue-500 focus:border-blue-500' : 'border-red-300 focus:ring-red-500 focus:border-red-500']" />
        <p v-if="!isEmailValid && email" class="mt-1 text-sm text-red-600">Por favor, informe um email válido.</p>
      </div>

      <!-- Senha -->
      <div>
        <label for="password" class="block text-sm font-medium text-gray-700">Senha</label>
        <input type="password" id="password" v-model="password" required
          :class="['mt-1 block w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none sm:text-sm',
            isPasswordStrong ? 'border-gray-300 focus:ring-blue-500 focus:border-blue-500' : 'border-red-300 focus:ring-red-500 focus:border-red-500']" />
        <p v-if="!isPasswordStrong && password" class="mt-1 text-sm text-red-600">A senha deve ter pelo menos 6
          caracteres.
        </p>
      </div>

      <!-- Confirmação de Senha para Registro -->
      <div v-if="isRegistering">
        <label for="confirmPassword" class="block text-sm font-medium text-gray-700">Confirme a Senha</label>
        <input type="password" id="confirmPassword" v-model="confirmPassword" required
          :class="['mt-1 block w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none sm:text-sm',
            doPasswordsMatch ? 'border-gray-300 focus:ring-blue-500 focus:border-blue-500' : 'border-red-300 focus:ring-red-500 focus:border-red-500']" />
        <p v-if="!doPasswordsMatch && confirmPassword" class="mt-1 text-sm text-red-600">As senhas não conferem.</p>
      </div>

      <div>
        <button type="submit" :disabled="!isFormValid"
          :class="['w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white focus:outline-none',
            isFormValid ? 'bg-blue-600 hover:bg-blue-700 focus:ring-2 focus:ring-offset-2 focus:ring-blue-500' : 'bg-blue-300 cursor-not-allowed']">
          {{ props.buttonText }}
        </button>
      </div>
    </form>
  </div>
</template>
