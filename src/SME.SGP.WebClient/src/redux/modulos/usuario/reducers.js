import produce from 'immer';

const inicial = {
  rf: '',
  token: '',
  usuario: '',
  dataLogin: null,
  logado: false,
  turmasUsuario: [],
  turmaSelecionada: {},
  filtroAtual: {},
  dadosUsuario: [],
  modificarSenha: '',
  meusDados: {
    foto: '',
  },
  possuiPerfilSmeOuDre: false,
  possuiPerfilDre: false,
  possuiPerfilSme: false,
  ehProfessorCj: false,
  ehProfessor: false,
  ehProfessorPoa: false,
  menu: [],
  permissoes: [],
  sessaoExpirou: false,
  ehProfessorCjInfantil: false,
  ehProfessorInfantil: false,
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/salvarRf':
        draft.rf = action.payload;
        break;
      case '@usuario/turmasUsuario':
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
        draft.possuiPerfilDre = action.payload.possuiPerfilDre;
        draft.possuiPerfilSme = action.payload.possuiPerfilSme;
        draft.ehProfessorCj = action.payload.ehProfessorCj;
        draft.ehProfessor = action.payload.ehProfessor;
        draft.menu = action.payload.menu;
        draft.ehProfessorPoa = action.payload.ehProfessorPoa;
        draft.dataHoraExpiracao = action.payload.dataHoraExpiracao;
        draft.sessaoExpirou = false;
        draft.ehProfessorInfantil = action.payload.ehProfessorInfantil;
        draft.ehProfessorCjInfantil = action.payload.ehProfessorCjInfantil;
        break;
      case '@usuario/salvarLoginRevalidado':
        draft.token = action.payload.token;
        draft.dataLogin = new Date();
        draft.dataHoraExpiracao = action.payload.dataHoraExpiracao;
        break;
      case '@usuario/deslogar':
        draft = inicial;
        localStorage.clear();
        window.location.reload(true);
        break;
      case '@usuario/deslogarSessaoExpirou':
        localStorage.clear();
        inicial.sessaoExpirou = true;
        draft = inicial;
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
      case '@usuario/setarConsideraHistorico':
        draft.turmaSelecionada = {
          ...state.turmaSelecionada,
          consideraHistorico: action.payload,
        };
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
      case '@usuario/setPermissoes':
        draft.permissoes = action.payload;
        break;
      default:
        break;
    }
  });
}
