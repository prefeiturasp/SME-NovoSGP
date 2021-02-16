import produce from 'immer';

const inicial = {
  questionarioDinamicoExpandirLinhaAusenciaEstudante: [],
  formsQuestionarioDinamico: null,
  questionarioDinamicoEmEdicao: false,
  questionarioDinamicoDadosModalAnotacao: null,
  questionarioDinamicoExibirModalAnotacao: false,
  resetarTabela: false,
};

export default function questionarioDinamico(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@questionarioDinamico/setFormsQuestionarioDinamico': {
        return {
          ...draft,
          formsQuestionarioDinamico: action.payload,
        };
      }
      case '@questionarioDinamico/setQuestionarioDinamicoEmEdicao': {
        return {
          ...draft,
          questionarioDinamicoEmEdicao: action.payload,
        };
      }
      case '@questionarioDinamico/setQuestionarioDinamicoDadosModalAnotacao': {
        return {
          ...draft,
          questionarioDinamicoDadosModalAnotacao: action.payload,
        };
      }
      case '@questionarioDinamico/setQuestionarioDinamicoExibirModalAnotacao': {
        return {
          ...draft,
          questionarioDinamicoExibirModalAnotacao: action.payload,
        };
      }
      case '@questionarioDinamico/setQuestionarioDinamicoExpandirLinhaAusenciaEstudante': {
        return {
          ...draft,
          questionarioDinamicoExpandirLinhaAusenciaEstudante: action.payload,
        };
      }
      case '@questionarioDinamico/setLimparDadosQuestionarioDinamico': {
        return {
          ...draft,
          formsQuestionarioDinamico: null,
          questionarioDinamicoEmEdicao: false,
          questionarioDinamicoDadosModalAnotacao: null,
          questionarioDinamicoExpandirLinhaAusenciaEstudante: [],
          questionarioDinamicoExibirModalAnotacao: false,
        };
      }
      case '@questionarioDinamico/setResetarTabela': {
        return {
          ...draft,
          resetarTabela: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
