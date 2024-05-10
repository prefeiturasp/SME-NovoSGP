import produce from 'immer';

const inicial = {
  alertas: [],
  confirmacao: {
    visivel: false,
    texto: '',
    titulo: '',
    resolve: null,
    textoOk: 'Sim',
    textoCancelar: 'Não',
  },
};

export default function alertas(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@alertas/exibir': {
        draft.alertas.push(action.payload);
        break;
      }
      case '@alertas/remover': {
        const indice = draft.alertas.findIndex(alerta => {
          return alerta.id === action.payload.id;
        });
        draft.alertas.splice(indice, 1);
        break;
      }
      case '@alertas/confirmar': {
        draft.confirmacao = {
          visivel: true,
          texto: action.payload.texto,
          textoNegrito: action.payload.textoNegrito,
          titulo: action.payload.titulo || 'Atenção',
          resolve: action.payload.resolve,
          textoOk: action.payload.textoOk || 'Sim',
          textoCancelar: action.payload.textoCancelar || 'Não',
          primeiroExibirTextoNegrito: action.payload.primeiroExibirTextoNegrito,
        };
        break;
      }
      case '@alertas/fecharConfirmacao': {
        draft.confirmacao = {
          visivel: false,
          texto: '',
          titulo: '',
          resolve: null,
        };
        break;
      }
      default:
        break;
    }
  });
}
