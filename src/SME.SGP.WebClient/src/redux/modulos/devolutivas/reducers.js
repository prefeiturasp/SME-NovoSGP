import produce from 'immer';

const inicial = {
  dadosPlanejamentos: {},
  paginaAtiva: null,
  numeroRegistros: null,
  alterouCaixaSelecao: false,
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
      case '@devolutivas/setNumeroRegistros': {
        return {
          ...draft,
          numeroRegistros: action.payload,
        };
      }

      case '@devolutivas/setAlterouCaixaSelecao': {
        console.log(action.payload);
        return {
          ...draft,
          alterouCaixaSelecao: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
