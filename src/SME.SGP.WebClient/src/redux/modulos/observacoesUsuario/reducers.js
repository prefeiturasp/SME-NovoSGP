import produce from 'immer';

const inicial = {
  dadosObservacoes: [],
  observacaoEmEdicao: null,
  novaObservacao: '',
};

export default function ObservacoesUsuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@observacoesUsuario/setDadosObservacoesUsuario': {
        return {
          ...draft,
          dadosObservacoes: action.payload,
        };
      }
      case '@observacoesUsuario/setObservacaoEmEdicao': {
        return {
          ...draft,
          observacaoEmEdicao: action.payload,
        };
      }
      case '@observacoesUsuario/setNovaObservacao': {
        return {
          ...draft,
          novaObservacao: action.payload,
        };
      }
      case '@observacoesUsuario/limparDadosObservacoesUsuario': {
        return {
          ...draft,
          dadosObservacoes: [],
          observacaoEmEdicao: null,
          novaObservacao: '',
        };
      }

      default:
        return draft;
    }
  });
}
