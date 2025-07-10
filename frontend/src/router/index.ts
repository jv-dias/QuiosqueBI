import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuthStore } from '@/stores/authStore';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/historico',
      name: 'historico',
      component: () => import('../views/HistoricoView.vue'),
      meta: { requiresAuth: true } 
    },
    {
      path: '/debug',
      name: 'debug',
      component: () => import('../views/DebugView.vue'),
    },
    {
      path: '/historico/:id',
      name: 'detalhe-analise',
      component: () => import('../views/DetalheAnaliseView.vue'),
      meta: { requiresAuth: true } 
    }, 
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue')
    }    
  ]
});

// 3. IMPLEMENTAÇÃO DO NAVIGATION GUARD (O "PORTEIRO")
router.beforeEach((to, from, next) => {
  // Inicializamos a store para poder usá-la aqui
  const authStore = useAuthStore();
  
  // Verifica se a rota para a qual o usuário está indo (o 'to') exige autenticação
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    // Se a rota exige autenticação E o usuário não está logado...
    // ...redireciona para a página de login.
    next({ name: 'login' });
  } else {
    // Caso contrário (se a rota for pública ou se o usuário estiver logado),
    // permite que a navegação continue normalmente.
    next();
  }
});

export default router;
