import produce from 'immer';

const inicial = {
  dadosDashboardFrequencia: {
    consideraHistorico: false,
  },
};

export default function dashboardFrequencia(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@dashboardFrequencia/setDadosDashboardFrequencia': {
        return {
          ...draft,
          dadosDashboardFrequencia: action.payload,
        };
      }
      case '@dashboardFrequencia/limparDadosDashboardFrequencia': {
        return {
          ...draft,
          dadosDashboardFrequencia: {
            consideraHistorico: false,
          },
        };
      }

      default:
        return draft;
    }
  });
}
