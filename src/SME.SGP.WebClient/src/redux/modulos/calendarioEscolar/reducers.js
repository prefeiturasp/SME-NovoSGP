import produce from 'immer';

const inicial = {
  meses: {
    1: {
      name: 'Janeiro',
      className: 'd-flex border',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    2: {
      name: 'Fevereiro',
      className: 'd-flex border border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    3: {
      name: 'Março',
      className: 'd-flex border border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    4: {
      name: 'Abril',
      className: 'd-flex border border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    5: {
      name: 'Maio',
      className: 'd-flex border border-top-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    6: {
      name: 'Junho',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 6,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    7: {
      name: 'Julho',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    8: {
      name: 'Agosto',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    9: {
      name: 'Setembro',
      className: 'd-flex border border-top-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    10: {
      name: 'Outubro',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    11: {
      name: 'Novembro',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
    12: {
      name: 'Dezembro',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      estaAberto: false,
    },
  },
  diaSelecionado: undefined,
};

export default function calendarioEscolar(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@calendarioEscolar/alternaMes': {
        const meses = Object.assign({}, state.meses);
        const { estaAberto } = state.meses[action.payload];

        Object.entries(meses).forEach(([indice, _mes]) => {
          meses[indice].estaAberto = false;
        });
        meses[action.payload].estaAberto = !estaAberto;

        draft.months = meses;
        break;
      }
      case '@calendarioEscolar/selecionaDia': {
        let diaSelecionado = action.payload;
        if (state.diaSelecionado === diaSelecionado) diaSelecionado = undefined;
        draft.diaSelecionado = diaSelecionado;
        break;
      }
      default:
        break;
    }
  });
}
