export type SugestaoFiltro = 'caixaSelecao' | 'controleIntervalo' | 'seletorData' | 'nenhum'
export type Relevancia = 'alta' | 'baixa'

export interface MetadadosColuna {
  nomeColuna: string
  tipoBase: string
  tipoSemantico: string
  sugestaoFiltro: SugestaoFiltro
  relevancia: Relevancia
  opcoes?: string[]
  minimo?: number
  maximo?: number
}
