import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia' // <-- 1. Importe o Pinia

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia()) // <-- 2. Diga ao Vue para usar o Pinia
app.use(router)

app.mount('#app')