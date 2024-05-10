import produce from 'immer';

const inicial = {
  expandirLinhaAusenciaEstudante: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
};

export default function ListaFrequenciaPorBimestre(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@listaFrequenciaPorBimestre/setExpandirLinhaAusenciaEstudante': {
        return {
          ...draft,
          expandirLinhaAusenciaEstudante: action.payload,
        };
      }
      case '@listaFrequenciaPorBimestre/setDadosModalAnotacao': {
        return {
          ...draft,
          dadosModalAnotacao: action.payload,
        };
      }
      case '@listaFrequenciaPorBimestre/setExibirModalAnotacao': {
        return {
          ...draft,
          exibirModalAnotacao: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
