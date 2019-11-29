import produce from 'immer';

const INICIAL = {
  disciplinaSelecionada: undefined
};

export default function bimestres(state = INICIAL, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@planoAula/SelecionarDisciplina':
        draft.disciplinaSelecionada = action.payload;
        break;
      default:
        break;
    }
  });
}
