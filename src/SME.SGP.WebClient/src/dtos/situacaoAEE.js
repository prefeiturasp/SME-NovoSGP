/**
 * @enum {Number}
 */
const situacaoAEE = {
  /** Em digitação */
  Rascunho: 1,
  /** Aguardando validação da coordenação */
  Encaminhado: 2,
  /** Aguardando análise do AEE */
  Analise: 3,
  /** Finalizado */
  Finalizado: 4,
  /** Encerrado */
  Encerrado: 5,
  /** Aguardando atribuição de responsável */
  AtribuicaoResponsavel: 6,
  /** Deferido */
  Deferido: 7,
  /** Indeferido */
  Indeferido: 8,
  /** Devolvido pela coordenação */
  Devolvido: 9,
  /**  Aguardando devolutiva coordenação */
  DevolutivaCoordenacao: 10,
  /**  Aguardando devolutiva PAAI */
  DevolutivaPAAI: 11,
};

export default situacaoAEE;
