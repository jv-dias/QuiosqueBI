import { defineStore } from 'pinia'
import { ref } from 'vue'
import axios from 'axios'

interface DadosGrafico {
  categoria: string;
  valor: number;
}

export interface ResultadoGrafico {
  titulo: string;
  tipoGrafico: string;
  dados: DadosGrafico[];
}

interface AnaliseSugeridaDebug {
    titulo_grafico: string;
    tipo_grafico: string;
    indice_dimensao: number;
    indice_metrica: number;
}

export interface DebugInfo {
    cabecalhosDoArquivo: string[];
    dadosBrutosDoArquivo: Record<string, any>[]; // Array de objetos dinâmicos
    planoDaIA: AnaliseSugeridaDebug[];
}

// Use a porta 7169 que está mapeada para o container
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:7169/api';

export const useAnaliseStore = defineStore('analise', () => {
  const resultados = ref<ResultadoGrafico[]>([])
  const debugInfo = ref<DebugInfo | null>(null)
  const carregando = ref(false)
  const erro = ref<string | null>(null)
  const carregandoDebug = ref(false);

  async function analisarArquivo(formData: FormData) {
    carregando.value = true
    erro.value = null
    resultados.value = []
    try {
      const response = await axios.post<any>(`${API_BASE_URL}/analise/upload`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });

      resultados.value = response.data.resultados;

    } catch (e: any) {
      erro.value = e.response?.data || e.message || 'Ocorreu um erro desconhecido.';
      console.error(e)
    } finally {
      carregando.value = false
    }
  }

  async function obterDadosDepuracao(formData: FormData) {
    carregandoDebug.value = true;
    erro.value = null;
    debugInfo.value = null;

    try {
      const response = await axios.post<DebugInfo>(`${API_BASE_URL}/analise/debug`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });

      debugInfo.value = response.data;

    } catch (e: any) {
      erro.value = e.response?.data || e.message || 'Ocorreu um erro desconhecido ao obter dados de depuração.';
      console.error(e);
    } finally {
      carregandoDebug.value = false;
    }
  }

  const historico = ref<any[]>([]);

  async function buscarHistorico() {
    carregando.value = true;
    erro.value = null;
    try {
      const response = await axios.get(`${API_BASE_URL}/analise/historico`);
      historico.value = response.data;
    } catch (e: any) {
      erro.value = e.response?.data || e.message || 'Ocorreu um erro ao buscar o histórico.';
      console.error(e);
    } finally {
      carregando.value = false;
    }
  }

  return {
    resultados,
    carregando,
    erro,
    debugInfo,
    carregandoDebug,
    analisarArquivo,
    obterDadosDepuracao,
    historico,
    buscarHistorico
  }
})
