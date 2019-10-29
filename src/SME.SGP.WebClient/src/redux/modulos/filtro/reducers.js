import produce from 'immer';

const inicial = {
  anosLetivos: [],
  modalidades: [],
  periodos: [],
  dres: [],
  unidadesEscolares: [],
  turmas: [],
};

export default function filtro(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@filtro/salvarAnosLetivos':
        draft.anosLetivos = action.payload;
        break;
      case '@filtro/salvarModalidades':
        draft.modalidades = action.payload;
        break;
      case '@filtro/salvarPeriodos':
        draft.periodos = action.payload;
        break;
      case '@filtro/salvarDres':
        draft.dres = action.payload;
        break;
      case '@filtro/salvarUnidadesEscolares':
        draft.unidadesEscolares = action.payload;
        break;
      case '@filtro/salvarTurmas':
        draft.turmas = action.payload;
        break;
      case '@filtro/limparDadosFiltro':
        draft.anosLetivos = [];
        draft.modalidades = [];
        draft.periodos = [];
        draft.dres = [];
        draft.unidadesEscolares = [];
        draft.turmas = [];
        break;
      default:
        break;
    }
  });
}
