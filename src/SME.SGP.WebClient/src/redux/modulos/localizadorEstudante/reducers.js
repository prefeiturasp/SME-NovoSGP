import produce from 'immer';

const inicial = {
  codigosAluno: [],
};

export default function localizadorEstudante(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@localizadorEstudante/setAlunosCodigo':
        draft.codigosAluno = action.payload;
        break;
      default:
        break;
    }
  });
}
