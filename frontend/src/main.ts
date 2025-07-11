// DENTRO DE src/main.ts

import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import ECharts from 'vue-echarts'
import { use } from 'echarts/core'
import { CanvasRenderer } from 'echarts/renderers'
import { BarChart, LineChart, PieChart, FunnelChart } from 'echarts/charts'

// 1. Importe o DataZoomComponent junto com os outros
import { 
  GridComponent, 
  TooltipComponent, 
  TitleComponent, 
  LegendComponent, 
  DataZoomComponent // <-- ADICIONE A VÍRGULA E O NOVO COMPONENTE AQUI
} from 'echarts/components'

// 2. Registra os componentes da ECharts que vamos usar
use([
  CanvasRenderer,
  BarChart,
  LineChart,
  PieChart,
  FunnelChart,
  GridComponent,
  TooltipComponent,
  TitleComponent,
  LegendComponent,
  DataZoomComponent // <-- ADICIONE O NOVO COMPONENTE AQUI TAMBÉM
]);

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.component('v-chart', ECharts)
app.use(createPinia())
app.use(router)

app.mount('#app')