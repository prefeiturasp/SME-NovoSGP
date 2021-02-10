import produce from 'immer';

const inicial = {
  dadosPlanejamentos: {},
  paginaAtiva: null,
  numeroRegistros: null,
  alterouCaixaSelecao: false,
  planejamentoExpandido: false,
  planejamentoSelecionado: null,
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
        return {
          ...draft,
          alterouCaixaSelecao: action.payload,
        };
      }
      case '@devolutivas/setPlanejamentoExpandido': {
        return {
          ...draft,
          planejamentoExpandido: action.payload,
        };
      }
      case '@devolutivas/setPlanejamentoSelecionado': {
        return {
          ...draft,
          planejamentoSelecionado: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
