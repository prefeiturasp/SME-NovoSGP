import produce from 'immer';

const inicial = {
  turmasUsuario: [],
  turmaSelecionada: [],
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/turmasUsuario': {
        action.payload.map(turma => draft.turmasUsuario.push(turma));
        break;
      }
      case '@usuario/selecionarTurma': {
        draft.turmaSelecionada.splice(0);
        action.payload.map(turma => draft.turmaSelecionada.push(turma));
        break;
      }
      case '@usuario/removerTurma': {
        draft.turmaSelecionada.splice(0);
        break;
      }
      default:
        break;
    }
  });
}
