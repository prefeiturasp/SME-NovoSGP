import produce from 'immer';

const inicial = {
  dadosCollapseAtribuicaoResponsavel: {},
};

export default function collapseAtribuicaoResponsavel(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@collapseAtribuicaoResponsavel/setDadosCollapseAtribuicaoResponsavel': {
        return {
          ...draft,
          dadosCollapseAtribuicaoResponsavel: action.payload,
        };
      }
      case '@collapseAtribuicaoResponsavel/setLimparDadosAtribuicaoResponsavel': {
        return {
          ...draft,
          dadosCollapseAtribuicaoResponsavel: {},
        };
      }
      default:
        return draft;
    }
  });
}
