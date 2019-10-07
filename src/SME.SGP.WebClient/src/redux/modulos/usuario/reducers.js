import produce from 'immer';

const inicial = {
  rf: '',
  token: '',
  usuario: '',
  dataLogin: null,
  logado: false,
  turmasUsuario: [],
  turmaSelecionada: [],
  filtroAtual: {},
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
        draft.usuario = action.payload.usuario;
        draft.dataLogin = new Date();
        break;
      case '@usuario/deslogar':
        draft.rf = '';
        draft.token = '';
        draft.dataLogin = null;
        draft.logado = false;
        draft.usuario = '';
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
      case '@usuario/filtroAtual':
        draft.turmaSelecionada = [];
        break;
      default:
        break;
    }
  });
}
