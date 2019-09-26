import produce from 'immer';

const inicial = {
  rf: '',
  token: '',
  dataLogin: null,
  logado: false,
  turmasUsuario: [],
  turmaSelecionada: [],
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/salvarRf':
        draft.rf = action.payload;
        break;
      case '@usuario/turmasUsuario':
        draft.turmasUsuario = [];
        draft.turmasUsuario = action.payload;
        break;
      case '@usuario/salvarLogin':
        draft.rf = action.payload.rf;
        draft.token = action.payload.token;
        draft.logado = true;
        draft.dataLogin = new Date();
        break;
      case '@usuario/deslogar':
        draft.rf = '';
        draft.token = '';
        draft.dataLogin = null;
        draft.logado = false;
        draft.turmasUsuario = [];
        draft.turmaSelecionada = [];
        break;
      case '@usuario/selecionarTurma':
        draft.turmaSelecionada = [];
        draft.turmaSelecionada = action.payload;
        break;
      case '@usuario/removerTurma':
        draft.turmaSelecionada = [];
        break;
      default:
        break;
    }
  });
}
