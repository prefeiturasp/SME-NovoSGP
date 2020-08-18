import produce from 'immer';

const inicial = {
  dadosPlanejamentos: {},
  paginaAtiva: null,
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
      case '@devolutivas/setPaginaAtiva': {
        return {
          ...draft,
          paginaAtiva: action.payload,
        };
      }
      case '@devolutivas/limparDadosPlanejamento': {
        return {
          ...draft,
          dadosPlanejamentos: {},
          paginaAtiva: null,
        };
      }

      default:
        return draft;
    }
  });
}
