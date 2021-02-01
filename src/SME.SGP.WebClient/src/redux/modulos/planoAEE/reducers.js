import produce from 'immer';

const inicial = {
  dadosIniciaisPlanoAEE: {},
};

export default function PlanoAEE(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@planoAEE/setDadosIniciaisPlanoAEE': {
        return {
          ...draft,
          dadosIniciaisPlanoAEE: action.payload,
        };
      }
      case '@planoAEE/setLimparDadosPlanoAEE': {
        return {
          ...draft,
          dadosIniciaisPlanoAEE: {},
        };
      }
      default:
        return draft;
    }
  });
}
