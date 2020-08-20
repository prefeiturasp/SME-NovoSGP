import produce from 'immer';

const inicial = {
  dadosObservacoes: [],
  observacaoEmEdicao: [],
  novaObservacao: '',
};

export default function ObservacoesChat(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@observacoesChat/setDadosObservacoesChat': {
        return {
          ...draft,
          dadosObservacoes: action.payload,
        };
      }
      case '@observacoesChat/setObservacaoEmEdicao': {
        return {
          ...draft,
          observacaoEmEdicao: action.payload,
        };
      }
      case '@observacoesChat/setNovaObservacao': {
        return {
          ...draft,
          novaObservacao: action.payload,
        };
      }
      case '@observacoesChat/limparDadosObservacoesChat': {
        return {
          ...draft,
          dadosObservacoes: [],
          observacaoEmEdicao: [],
          novaObservacao: '',
        };
      }

      default:
        return draft;
    }
  });
}
