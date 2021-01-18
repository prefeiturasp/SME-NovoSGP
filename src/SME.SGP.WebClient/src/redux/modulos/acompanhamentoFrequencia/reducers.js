import produce from 'immer';

const inicial = {
  expandirLinhaFrequenciaAluno: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
};

export default function AcompanhamentoFrequencia(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@acompanhamentoFrequencia/setExpandirLinhaFrequenciaAluno': {
        return {
          ...draft,
          expandirLinhaFrequenciaAluno: action.payload,
        };
      }
      case '@acompanhamentoFrequencia/setDadosModalAnotacao': {
        return {
          ...draft,
          dadosModalAnotacao: action.payload,
        };
      }
      case '@acompanhamentoFrequencia/setExibirModalAnotacao': {
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
