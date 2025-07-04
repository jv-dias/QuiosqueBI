import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import DebugView from '../views/DebugView.vue' // Importando a nova view de depuração

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/debug', // Nova rota para o painel de depuração
      name: 'debug',
      component: DebugView
    }
  ],
  
})

export default router
