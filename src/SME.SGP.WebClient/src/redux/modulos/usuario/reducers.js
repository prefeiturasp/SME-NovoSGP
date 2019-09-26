import produce from 'immer';

const inicial = {
  rf: '',
  turmasUsuario: [],
  turmaSelecionada: [],
  meusDados: {
    nome: 'Teste com Sobrenome Um tanto Quanto Maior',
    rf: '123456',
    cpf: '12345678901',
    empresa: 'SME',
    foto: 'https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg'
  }
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
      case '@usuario/meuDados': {
        draft.meusDados = action.payload;
        break;
      }
      default:
        break;
    }
  });
}
