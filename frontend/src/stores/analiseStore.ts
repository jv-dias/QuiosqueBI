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

// ===================================================================
// NOVA INTERFACE PARA OS DADOS DE DEPURACAO
// ===================================================================
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
// ===================================================================

export const useAnaliseStore = defineStore('analise', () => {
  const resultados = ref<ResultadoGrafico[]>([])
  // ===================================================================
  // TIPO DO debugInfo AGORA É MAIS ESPECÍFICO
  const debugInfo = ref<DebugInfo | null>(null) 
  // ===================================================================
  const carregando = ref(false)
  const erro = ref<string | null>(null)
  
  // ===================================================================
  // NOVO ESTADO PARA CARREGAMENTO ESPECÍFICO DA DEPURACAO
  const carregandoDebug = ref(false); 
  // ===================================================================

  async function analisarArquivo(formData: FormData) {
    carregando.value = true
    erro.value = null
    resultados.value = []
    // ===================================================================
    // debugInfo NÃO É MAIS ATUALIZADO POR ESTA FUNCAO
    // debugInfo.value = null 
    // ===================================================================

    try {
      const urlDaApi = 'https://localhost:7169/api/analise/upload';

      const response = await axios.post<any>(urlDaApi, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      
      resultados.value = response.data.resultados;
      // Removido: debugInfo.value = response.data.debugInfo;

    } catch (e: any) {
      erro.value = e.response?.data || e.message || 'Ocorreu um erro desconhecido.';
      console.error(e)
    } finally {
      carregando.value = false
    }
  }

  // ===================================================================
  // NOVA ACTION PARA OBTER DADOS DE DEPURACAO
  // ===================================================================
  async function obterDadosDepuracao(formData: FormData) {
    carregandoDebug.value = true;
    erro.value = null; // Limpa erros gerais
    debugInfo.value = null; // Limpa informações de depuração antigas

    try {
      const urlDaApiDebug = 'https://localhost:7169/api/analise/debug'; 

      const response = await axios.post<DebugInfo>(urlDaApiDebug, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      
      debugInfo.value = response.data; // Atribui a resposta ao debugInfo
      
    } catch (e: any) {
      erro.value = e.response?.data || e.message || 'Ocorreu um erro desconhecido ao obter dados de depuração.';
      console.error(e);
    } finally {
      carregandoDebug.value = false;
    }
  }

  // Adicione um novo estado para o histórico
const historico = ref<any[]>([]); // Use 'any' por enquanto, pode criar a interface depois

// Adicione uma nova action para buscar o histórico
async function buscarHistorico() {
  carregando.value = true;
  erro.value = null;
  try {
    const urlDaApi = 'https://localhost:7169/api/analise/historico';
    const response = await axios.get(urlDaApi);
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
    carregandoDebug, // Retorna o novo estado
    analisarArquivo, 
    obterDadosDepuracao, // Retorna a nova action,
    historico,
    buscarHistorico
  }
})