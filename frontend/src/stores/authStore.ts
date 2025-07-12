import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import axios from 'axios';
import router from '@/router';
import { jwtDecode } from 'jwt-decode'; // Importe a biblioteca

interface AuthPayload {
  email: string;
  password: string;
}

// Interface para o conteúdo que esperamos do token decodificado
interface DecodedToken {
  // A chave do token não é 'name', mas sim o nome completo do schema da claim.
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
  
  // O mesmo para o email, para sermos consistentes
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
}

// --- AQUI ESTÁ A CORREÇÃO PRINCIPAL ---
// 1. Define a URL base da API usando a variável de ambiente do Vite.
// 2. Se a variável não existir (no ambiente local), ele usa o endereço de desenvolvimento.
const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7169/api';

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token') || null);
  const user = ref<DecodedToken | null>(null);

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
    try {
      const response = await axios.post(`${API_BASE_URL}/login`, payload);
      updateUserState(response.data.token);
      await router.push('/historico');
    } catch (error) {
      console.error("Falha no login:", error);
      alert("Email ou senha inválidos.");
    }
  }

  function logout() {
    updateUserState(null);
    router.push('/login');
  }

  async function registrar(payload: any) {
     try {
        await axios.post(`${API_BASE_URL}/register`, payload);
        alert("Usuário registrado com sucesso! Por favor, faça o login.");
        await router.push('/login');
     } catch(error) {
        console.error("Falha no registro:", error);
        alert("Falha no registro. Verifique os dados e tente novamente.");
     }
  }

  return {
    isAuthenticated,
    userFirstName, 
    login,
    logout,
    registrar
  };
});