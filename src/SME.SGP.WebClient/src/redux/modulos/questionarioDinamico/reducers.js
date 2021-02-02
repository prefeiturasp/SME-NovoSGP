import produce from 'immer';

const inicial = {
  formsQuestionarioDinamico: null,
  questionarioDinamicoEmEdicao: false,
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
      case '@questionarioDinamico/setLimparDadosQuestionarioDinamico': {
        return {
          ...draft,
          formsQuestionarioDinamico: null,
          questionarioDinamicoEmEdicao: false,
        };
      }
      default:
        return draft;
    }
  });
}
