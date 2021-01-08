import produce from 'immer';

const inicial = {
  expandirLinhaAusenciaAluno: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
};

export default function EncaminhamentoAEE(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@encaminhamentoAEE/setExpandirLinhaAusenciaAluno': {
        return {
          ...draft,
          expandirLinhaAusenciaAluno: action.payload,
        };
      }
      case '@encaminhamentoAEE/setDadosModalAnotacao': {
        return {
          ...draft,
          dadosModalAnotacao: action.payload,
        };
      }
      case '@encaminhamentoAEE/setExibirModalAnotacao': {
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
