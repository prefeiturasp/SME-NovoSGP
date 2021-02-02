import produce from 'immer';

const inicial = {
  formsQuestionarioDinamico: null,
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
      case '@questionarioDinamico/setLimparDadosQuestionarioDinamico': {
        return {
          ...draft,
          formsQuestionarioDinamico: null,
        };
      }
      default:
        return draft;
    }
  });
}
