import produce from 'immer';

const inicial = {
  rf: '',
  turmasUsuario: [],
  turmaSelecionada: [],
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/salvarRf': {
        draft.rf = action.payload;
        break;
      }
      case '@usuario/turmasUsuario': {
        draft.turmasUsuario = [];
        draft.turmasUsuario = action.payload;
        break;
      }
      case '@usuario/selecionarTurma': {
        draft.turmaSelecionada = [];
        draft.turmaSelecionada = action.payload;
        break;
      }
      case '@usuario/removerTurma': {
        draft.turmaSelecionada = [];
        break;
      }
      default:
        break;
    }
  });
}
