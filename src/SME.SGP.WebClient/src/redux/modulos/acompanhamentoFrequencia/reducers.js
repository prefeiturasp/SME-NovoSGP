import produce from 'immer';

const inicial = {
  expandirLinhaFrequenciaAluno: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
  frequenciaAlunoCodigo: null,
  dadosFrequenciaAlunoObter: {},
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
      case '@acompanhamentoFrequencia/setDadosFrequenciaAlunoObter': {
        return {
          ...draft,
          dadosFrequenciaAlunoObter: action.payload,
        };
      }
      case '@acompanhamentoFrequencia/setFrequenciaAlunoCodigo': {
        return {
          ...draft,
          frequenciaAlunoCodigo: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
