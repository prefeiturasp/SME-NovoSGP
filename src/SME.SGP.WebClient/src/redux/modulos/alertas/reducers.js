import produce from 'immer';

const inicial = {
  alertas: [],
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
      default:
        break;
    }
  });
}
