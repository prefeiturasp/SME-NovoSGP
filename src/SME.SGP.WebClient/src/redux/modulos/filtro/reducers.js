import produce from 'immer';

const inicial = {
  anosLetivos: [],
  modalidades: [],
  periodos: [],
  dres: [],
  unidadesEscolares: [],
  turmas: [],
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@usuario/salvarAnosLetivos':
        draft.anosLetivos = action.payload;
        break;
      case '@usuario/salvarModalidades':
        draft.modalidades = action.payload;
        break;
      case '@usuario/salvarPeriodos':
        draft.periodos = action.payload;
        break;
      case '@usuario/salvarDres':
        draft.dres = action.payload;
        break;
      case '@usuario/salvarUnidadesEscolares':
        draft.unidadesEscolares = action.payload;
        break;
      case '@usuario/salvarTurmas':
        draft.turmas = action.payload;
        break;
      default:
        break;
    }
  });
}
