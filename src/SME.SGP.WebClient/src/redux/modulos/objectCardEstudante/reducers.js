import produce from 'immer';

const inicial = {
  dadosObjectCardEstudante: {},
};

export default function objectCardEstudante(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@objectCardEstudante/setDadosObjectCardEstudante': {
        return {
          ...draft,
          dadosObjectCardEstudante: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
