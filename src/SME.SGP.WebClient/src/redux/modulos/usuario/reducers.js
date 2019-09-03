import produce from 'immer';

const inicial = {
  turmasUusario: [],
  turmaSelecionada: [],
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/turmasUusario': {
        action.payload.map(turma => draft.turmasUusario.push(turma));
        break;
      }
      case '@usuario/selecionarTurma': {
        draft.turmaSelecionada.splice(0);
        action.payload.map(turma => draft.turmaSelecionada.push(turma));
        break;
      }
      default:
        break;
    }
  });
}
