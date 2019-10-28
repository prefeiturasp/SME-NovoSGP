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
  dadosUsuario: [],
  modificarSenha: '',
  meusDados: {
    foto:
      'https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg',
  },
  possuiPerfilSmeOuDre: false,
  menu: []
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
        draft.dataLogin = new Date();
        draft.logado = true;
        draft.usuario = action.payload.usuario;
        draft.modificarSenha = action.payload.modificarSenha;
        draft.possuiPerfilSmeOuDre = action.payload.possuiPerfilSmeOuDre;
        draft.menu = action.payload.menu;
        break;
      case '@usuario/deslogar':
        draft.rf = '';
        draft.token = '';
        draft.dataLogin = null;
        draft.logado = false;
        draft.usuario = '';
        draft.modificarSenha = '';
        draft.turmasUsuario = [];
        draft.turmaSelecionada = [];
        draft.dadosUsuario = [];
        draft.menu = [];
        break;
      case '@usuario/selecionarTurma':
        draft.turmaSelecionada = action.payload;
        break;
      case '@usuario/removerTurma':
        draft.turmaSelecionada = [];
        break;
      case '@usuario/meusDados':
        draft.meusDados = action.payload;
        break;
      case '@usuario/meusDadosSalvarEmail':
        draft.meusDados.email = action.payload;
        break;
      case '@usuario/filtroAtual':
        draft.filtroAtual = action.payload;
        break;
      case '@usuario/salvarDadosUsuario':
        draft.dadosUsuario = action.payload;
        break;
      case '@usuario/setMenu':
        draft.menu = action.payload;
        break;
      default:
        break;
    }
  });
}
