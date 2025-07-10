import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import axios from 'axios';
import router from '@/router'; // Importamos o router para poder redirecionar

// Define a interface para os dados que os formulários enviam
interface AuthPayload {
  email: string;
  password: string;
}

// URL base da sua API. É uma boa prática defini-la em um só lugar.
const API_URL = 'http://localhost:5159/api/auth';

export const useAuthStore = defineStore('auth', () => {
  // Tenta pegar o token do localStorage ao iniciar. Se não existir, começa como nulo.
  const token = ref(localStorage.getItem('token') || null);

  // Configura o cabeçalho de autorização do axios com o token inicial (se existir)
  if (token.value) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token.value}`;
  }

  // Uma propriedade computada que nos diz reativamente se o usuário está logado.
  const isAuthenticated = computed(() => !!token.value);

  // Ação para fazer o login
  async function login(payload: AuthPayload) {
    try {
      const response = await axios.post(`${API_URL}/login`, payload);
      
      const newToken = response.data.token;
      token.value = newToken;
      localStorage.setItem('token', newToken); // Salva o token para persistir entre sessões
      axios.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
      
      // Redireciona o usuário para a página principal ou de histórico após o login
      await router.push('/analise'); 
    } catch (error) {
      console.error("Falha no login:", error);
      alert("Email ou senha inválidos.");
    }
  }

  // Ação para fazer o logout
  function logout() {
    token.value = null;
    localStorage.removeItem('token'); // Remove o token do armazenamento
    delete axios.defaults.headers.common['Authorization'];
    
    // Redireciona para a página de login para que o usuário não fique em uma página protegida
    router.push('/login');
  }

  // Ação para registrar um novo usuário
  async function registrar(payload: AuthPayload) {
     try {
        await axios.post(`${API_URL}/register`, payload);
        alert("Usuário registrado com sucesso! Por favor, faça o login.");
        await router.push('/login');
     } catch(error) {
        console.error("Falha no registro:", error);
        alert("Falha no registro. O usuário pode já existir ou a senha é muito fraca.");
     }
  }

  // Expõe os estados e as ações para que os componentes possam usá-los
  return { 
    token, 
    isAuthenticated, 
    login, 
    logout, 
    registrar 
  };
});