import produce from 'immer';

const inicial = {
  dadosCollapseAtribuicaoResponsavel: {
    codigoRF: 123456,
    nomeServidor: 'Nome teste',
  },
};

export default function collapseAtribuicaoResponsavel(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@collapseAtribuicaoResponsavel/setDadosCollapseAtribuicaoResponsavel': {
        return {
          ...draft,
          dadosCollapseLocalizarEstudante: action.payload,
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
