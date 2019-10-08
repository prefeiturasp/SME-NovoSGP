import produce from 'immer';

const inicial = {
  rf: '',
  token: '',
  dataLogin: null,
  logado: false,
  turmasUsuario: [],
  turmaSelecionada: [],
  filtroAtual: {},
  dadosUsuario: [],
  meusDados: {
    nome: 'Teste com Sobrenome Um tanto Quanto Maior',
    rf: '123456',
    cpf: '12345678901',
    empresa: 'SME',
    foto: 'https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg'
  },
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
        draft.turmaSelecionada = action.payload;
        break;
      case '@usuario/removerTurma':
        draft.turmaSelecionada = [];
        break;
      case '@usuario/meusDados': {
        draft.meusDados = action.payload;
        break;
      }
      case '@usuario/filtroAtual':
        draft.filtroAtual = action.payload;
        break;
      case '@usuario/salvarDadosUsuario':
        draft.dadosUsuario = action.payload;
        break;
      default:
        break;
    }
  });
}
