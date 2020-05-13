// Tipos
import Tipos from './types';

// Handlers
import Handlers from './handlers';

export const estadoInicial = {
  meses: [
    {
      numeroMes: 1,
      nome: 'Janeiro',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 2,
      nome: 'Fevereiro',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 3,
      nome: 'Março',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 4,
      nome: 'Abril',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 5,
      nome: 'Maio',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 6,
      nome: 'Junho',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 7,
      nome: 'Julho',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 8,
      nome: 'Agosto',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 9,
      nome: 'Setembro',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 10,
      nome: 'Outubro',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 11,
      nome: 'Novembro',
      eventos: 0,
      estaAberto: false,
    },
    {
      numeroMes: 12,
      nome: 'Dezembro',
      eventos: 0,
      estaAberto: false,
    },
  ],
  diaSelecionado: undefined,
  eventoAulaCalendarioEdicao: {},
  dadosAulaFrequencia: {},
};

export default (state, action) => {
  const { payload, type } = action;
  switch (type) {
    case Tipos.selecionarMes:
      return Handlers.selecionarMesHandler(state, payload);
    case Tipos.selecionarDia:
      return Handlers.selecionarDiaHandler(state, payload);
    case Tipos.zeraCalendario:
      return Handlers.zeraCalendario(state);
    default:
      return state;
  }
};
