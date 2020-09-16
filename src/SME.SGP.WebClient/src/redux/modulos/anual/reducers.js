import produce from 'immer';

const inicial = {
  planoAnualEmEdicao: false,
  componenteCurricular: undefined,
};

export default function planoAnual(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@planoAnual/setPlanoAnualEmEdicao': {
        return {
          ...draft,
          planoAnualEmEdicao: action.payload,
        };
      }
      case '@planoAnual/setComponenteCurricularPlanoAnual': {
        return {
          ...draft,
          componenteCurricular: action.payload,
        };
      }
      case '@planoAnual/limparDadosPlanoAnual': {
        return {
          ...draft,
          planoAnualEmEdicao: false,
          componenteCurricular: undefined,
        };
      }

      default:
        return draft;
    }
  });
}
