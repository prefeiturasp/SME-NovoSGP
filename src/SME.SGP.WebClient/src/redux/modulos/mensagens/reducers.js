import produce from 'immer';

const inicial = {
  exibirMensagemSessaoExpirou: false,
};

export default function mensagens(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/setExibirMensagemSessaoExpirou':
        draft.exibirMensagemSessaoExpirou = action.payload;
        break;
      default:
        break;
    }
  });
}
