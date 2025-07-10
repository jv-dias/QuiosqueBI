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
    },
    {
      path: '/analise',
      name: 'analise',
      component: () => import('../views/AnaliseView.vue'),
      meta: { requiresAuth: true }
    }    
  ]
});

// Navigation Guard
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  const authRoutes = ['login', 'register'];

  // Verifica se a rota para a qual o usuário está indo (o 'to') exige autenticação
  if (to.meta.requiresAuth && !isAuthenticated) {
    // Se a rota exige login e o usuário NÃO está logado, vai para o login.
    next({ name: 'login' });
  } else if (authRoutes.includes(to.name as string) && isAuthenticated) {
    // Se a rota é de login/registro E o usuário JÁ ESTÁ logado...
    // ...redireciona para a página inicial (ou para o histórico).
    next({ name: 'home' }); 
  } else {
    // Em todos os outros casos, permite a navegação.
    next();
  }
});

export default router;
