// Tipos
import Tipos from './types';

// Handlers
import Handlers from './handlers';

export const estadoInicial = {
  carregandoCalendario: false,
  carregandoMes: false,
  carregandoDia: false,
  eventos: {
    meses: [],
  },
  diaSelecionado: undefined,
};

export default (state, action) => {
  const { type, payload } = action;
  switch (type) {
    case Tipos.setarEventosMes:
      return Handlers.setarEventosMesHandler(state, payload);
    case Tipos.setarEventosDia:
      return Handlers.setarEventosDiaHandler(state, payload);
    case Tipos.setarCarregandoCalendario:
      return {
        ...state,
        carregandoCalendario: payload,
      };
    case Tipos.setarCarregandoMes:
      return {
        ...state,
        carregandoMes: payload,
      };
    case Tipos.setarCarregandoDia:
      return {
        ...state,
        carregandoDia: payload,
      };
    default:
      return state;
  }
};
