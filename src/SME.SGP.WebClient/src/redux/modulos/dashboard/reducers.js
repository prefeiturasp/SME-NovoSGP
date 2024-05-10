import produce from 'immer';

const inicial = {
  dadosCardsDashboard: [],
  carregandoDadosCardsDashboard: false,
};

export default function dashboard(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@dashboard/setDadosCardsDashboard': {
        return {
          ...draft,
          dadosCardsDashboard: action.payload,
        };
      }
      case '@dashboard/setCarregandoDadosCardsDashboard': {
        return {
          ...draft,
          carregandoDadosCardsDashboard: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
