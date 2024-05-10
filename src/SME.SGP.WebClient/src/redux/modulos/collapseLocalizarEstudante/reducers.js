import produce from 'immer';

const inicial = {
  dadosIniciaisLocalizarEstudante: {},
  dadosCollapseLocalizarEstudante: {},
};

export default function collapseLocalizarEstudante(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@collapseLocalizarEstudante/setDadosIniciaisLocalizarEstudante': {
        return {
          ...draft,
          dadosIniciaisLocalizarEstudante: action.payload,
        };
      }
      case '@collapseLocalizarEstudante/setDadosCollapseLocalizarEstudante': {
        return {
          ...draft,
          dadosCollapseLocalizarEstudante: action.payload,
        };
      }
      case '@collapseLocalizarEstudante/setLimparDadosLocalizarEstudante': {
        return {
          ...draft,
          dadosIniciaisLocalizarEstudante: {},
          dadosCollapseLocalizarEstudante: {},
        };
      }
      default:
        return draft;
    }
  });
}
