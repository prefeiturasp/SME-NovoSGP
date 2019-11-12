import produce from 'immer';

const inicial = {
  meses: {
    1: {
      nome: 'Janeiro',
      className: 'd-flex border',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    2: {
      nome: 'Fevereiro',
      className: 'd-flex border border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    3: {
      nome: 'Março',
      className: 'd-flex border border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    4: {
      nome: 'Abril',
      className: 'd-flex border border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    5: {
      nome: 'Maio',
      className: 'd-flex border border-top-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    6: {
      nome: 'Junho',
      className: 'd-flex border border-top-0 border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    7: {
      nome: 'Julho',
      className: 'd-flex border border-top-0 border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    8: {
      nome: 'Agosto',
      className: 'd-flex border border-top-0 border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    9: {
      nome: 'Setembro',
      className: 'd-flex border border-top-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    10: {
      nome: 'Outubro',
      className: 'd-flex border border-top-0 border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    11: {
      nome: 'Novembro',
      className: 'd-flex border border-top-0 border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    12: {
      nome: 'Dezembro',
      className: 'd-flex border border-top-0 border-left-0',
      eventos: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
  },
  diaSelecionado: undefined,
};

export default function calendarioEscolar(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@calendarioEscolar/selecionaMes': {
        const meses = Object.assign({}, state.meses);
        const { estaAberto } =
          action.payload > 0 ? state.meses[action.payload] : false;

        Object.entries(meses).forEach(([indice, _mes]) => {
          meses[indice].estaAberto = false;
        });
        if (action.payload > 0) meses[action.payload].estaAberto = !estaAberto;

        draft.meses = meses;
        break;
      }
      case '@calendarioEscolar/selecionaDia': {
        let diaSelecionado = action.payload;
        if (state.diaSelecionado === diaSelecionado) diaSelecionado = undefined;
        draft.diaSelecionado = diaSelecionado;
        break;
      }
      case '@calendarioEscolar/atribuiEventosMes': {
        const { mes, eventos } = action.payload;
        const meses = Object.assign({}, state.meses);
        meses[mes].eventos = eventos;
        draft.meses = meses;
        break;
      }
      case '@calendarioEscolar/zeraCalendario': {
        const meses = Object.assign({}, state.meses);
        Object.entries(meses).forEach(([indice, _mes]) => {
          meses[indice].eventos = 0;
          meses[indice].estaAberto = false;
        });
        draft.meses = meses;
        break;
      }
      default:
        break;
    }
  });
}
