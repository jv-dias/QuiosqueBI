import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import axios from 'axios';
import router from '@/router';
import { jwtDecode } from 'jwt-decode';

interface AuthPayload {
  email: string;
  password: string;
}

interface DecodedToken {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
}

// Usar a URL completa com https para evitar problemas
const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://quiosquebi-api.azurewebsites.net/api';

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token') || null);
  const user = ref<DecodedToken | null>(null);
  const error = ref<string | null>(null);

  function updateUserState(newToken: string | null) {
    token.value = newToken;
    if (newToken) {
      localStorage.setItem('token', newToken);
      axios.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
      try {
        user.value = jwtDecode(newToken);
      } catch (error) {
        console.error("Erro ao decodificar o token:", error);
        user.value = null;
      }
    } else {
      localStorage.removeItem('token');
      delete axios.defaults.headers.common['Authorization'];
      user.value = null;
    }
  }

  // Inicializa o estado quando a store é criada
  updateUserState(token.value);

  const isAuthenticated = computed(() => !!token.value);

  const userFirstName = computed(() => {
    // Acessa a propriedade usando a chave correta e completa
    const fullName = user.value?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    if (fullName) {
      return fullName.split('-')[0];
    }
    return '';
  });

  async function login(payload: AuthPayload) {
    error.value = null;
    try {
      console.log(`Tentando login em: ${API_BASE_URL}/auth/login`);
      const response = await axios.post(`${API_BASE_URL}/auth/login`, payload);
      updateUserState(response.data.token);
      await router.push('/historico');
    } catch (e: any) {
      console.error("Falha no login:", e);
      error.value = e.message || "Erro desconhecido durante login";
    }
  }

  function logout() {
    updateUserState(null);
    router.push('/login');
  }

  async function registrar(payload: any) {
    error.value = null;
    try {
      console.log(`Tentando registrar em: ${API_BASE_URL}/auth/register`);
      await axios.post(`${API_BASE_URL}/auth/register`, payload, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      alert("Usuário registrado com sucesso! Por favor, faça o login.");
      await router.push('/login');
    } catch (e: any) {
      console.error("Falha no registro:", e);

      // Verificar especificamente o erro de email duplicado
      if (e.response && e.response.status === 409) {
        error.value = "Este email já está cadastrado. Por favor, tente outro email ou faça login.";
      } else {
        error.value = e.response?.data?.message || e.message || "Erro desconhecido durante o registro";
      }
    }
  }

  function clearError() {
    error.value = null;
  }

  return {
    isAuthenticated,
    userFirstName,
    error,
    login,
    logout,
    registrar,
    clearError
  };
});
