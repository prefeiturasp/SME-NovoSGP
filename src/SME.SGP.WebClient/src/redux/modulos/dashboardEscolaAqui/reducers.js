import produce from 'immer';

const inicial = {
  dadosDeLeituraDeComunicadosAgrupadosPorModalidade: [],
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
      case '@dashboardEscolaAqui/limparDadosDashboardEscolaAqui': {
        return {
          ...draft,
          dadosDeLeituraDeComunicadosAgrupadosPorModalidade: [],
        };
      }

      default:
        return draft;
    }
  });
}
