import produce from 'immer';

const inicial = {
  notificacoes: [],
  quantidade: 0,
};

export default function notificacoes(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@notificacoes/notificacoesLista': {
        draft.notificacoes = [];
        draft.notificacoes = action.payload;
        break;
      }
      case '@notificacoes/naoLidas': {
        draft.quantidade = action.payload;
        break;
      }
      default:
        break;
    }
  });
}
