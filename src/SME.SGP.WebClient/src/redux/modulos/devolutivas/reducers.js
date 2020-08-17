import produce from 'immer';

const inicial = {
  dadosPlanejamentos: {},
};

export default function devolutivas(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@devolutivas/setDadosPlanejamentos': {
        return {
          ...draft,
          dadosPlanejamentos: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
