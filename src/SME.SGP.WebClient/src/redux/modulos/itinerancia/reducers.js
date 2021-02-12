import produce from 'immer';

const inicial = {
  questoesItinerancia: [],
  questoesItineranciaAluno: {},
  objetivos: [],
  objetivosOriginal: [],
};

export default function frequenciaPlanoAula(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@itinerancia/setQuestoesItinerancia': {
        return {
          ...draft,
          questoesItinerancia: action.payload,
        };
      }
      case '@itinerancia/setQuestoesItineranciaAluno': {
        return {
          ...draft,
          questoesItineranciaAluno: action.payload,
        };
      }

      case '@itinerancia/setObjetivosItinerancia': {
        return {
          ...draft,
          objetivos: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
