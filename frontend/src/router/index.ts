import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import DebugView from '../views/DebugView.vue' 
import HistoricoView from '../views/HistoricoView.vue'
import DetalheAnaliseView from '../views/DetalheAnaliseView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/debug', 
      name: 'debug',
      component: DebugView
    },    
    {
      path: '/historico',
      name: 'historico',      
      component: () => import('../views/HistoricoView.vue')
    },    
    {
      path: '/historico/:id', // O ':id' torna a rota dinâmica
      name: 'detalhe-analise',
      component: () => import('../views/DetalheAnaliseView.vue')
    }
  ],
  
})

export default router
