import produce from 'immer';

const inicial = {
  meses: {
    1: {
      name: 'Janeiro',
      className: 'd-flex border',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    2: {
      name: 'Fevereiro',
      className: 'd-flex border border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    3: {
      name: 'Março',
      className: 'd-flex border border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    4: {
      name: 'Abril',
      className: 'd-flex border border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    5: {
      name: 'Maio',
      className: 'd-flex border border-top-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    6: {
      name: 'Junho',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 6,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    7: {
      name: 'Julho',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    8: {
      name: 'Agosto',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    9: {
      name: 'Setembro',
      className: 'd-flex border border-top-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    10: {
      name: 'Outubro',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    11: {
      name: 'Novembro',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
    12: {
      name: 'Dezembro',
      className: 'd-flex border border-top-0 border-left-0',
      appointments: 0,
      chevronColor: '#C4C4C4',
      isOpen: false,
    },
  },
  diaSelecionado: undefined,
};

export default function calendarioEscolar(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@calendarioEscolar/alternaMes': {
        const meses = Object.assign({}, state.meses);
        const estaAberto = state.months[action.month].isOpen;

        meses.forEach((indice, _mes) => (meses[indice].isOpen = false));
        meses[action.payload].isOpen = !estaAberto;

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
