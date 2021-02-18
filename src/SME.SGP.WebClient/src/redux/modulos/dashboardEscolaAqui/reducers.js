import produce from 'immer';

const inicial = {
  dadosDeLeituraDeComunicadosAgrupadosPorModalidade: [],
  dadosDeLeituraDeComunicadosPorTurmas: [],
};

export default function dashboardEscolaAqui(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@dashboardEscolaAqui/setDadosDeLeituraDeComunicadosAgrupadosPorModalidade': {
        return {
          ...draft,
          dadosDeLeituraDeComunicadosAgrupadosPorModalidade: action.payload,
        };
      }
      case '@dashboardEscolaAqui/setDadosDeLeituraDeComunicadosPorTurmas': {
        return {
          ...draft,
          dadosDeLeituraDeComunicadosPorTurmas: action.payload,
        };
      }
      case '@dashboardEscolaAqui/limparDadosDashboardEscolaAqui': {
        return {
          ...draft,
          dadosDeLeituraDeComunicadosAgrupadosPorModalidade: [],
          dadosDeLeituraDeComunicadosPorTurmas: [],
        };
      }

      default:
        return draft;
    }
  });
}
