import produce from 'immer';

const inicial = {
  dadosObservacoes: [],
  observacaoEmEdicao: [],
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
      case '@observacoesChat/limparDadosObservacoesChat': {
        return {
          ...draft,
          dadosObservacoes: [],
          observacaoEmEdicao: [],
        };
      }

      default:
        return draft;
    }
  });
}
