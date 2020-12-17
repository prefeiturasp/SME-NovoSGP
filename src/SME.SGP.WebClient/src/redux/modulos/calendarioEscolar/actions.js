export function selecionaMes(mes) {
  return {
    type: '@calendarioEscolar/selecionaMes',
    payload: mes,
  };
}

export function selecionaDia(dia) {
  return {
    type: '@calendarioEscolar/selecionaDia',
    payload: dia,
  };
}

export function atribuiEventosMes(mes, eventos) {
  return {
    type: '@calendarioEscolar/atribuiEventosMes',
    payload: {
      mes,
      eventos,
    },
  };
}

export function zeraCalendario() {
  return {
    type: '@calendarioEscolar/zeraCalendario',
  };
}

export function salvarEventoCalendarioEdicao(
  tipoCalendario,
  eventoSme,
  dre,
  unidadeEscolar,
  mes,
  dia
) {
  return {
    type: '@calendarioEscolar/salvarEventoCalendarioEdicao',
    payload: {
      tipoCalendario,
      eventoSme,
      dre,
      unidadeEscolar,
      mes,
      dia,
    },
  };
}

export function setFiltroCalendarioEscolar(filtroCalendarioEscolar) {
  return {
    type: '@calendarioEscolar/filtroCalendarioEscolar',
    payload: {
      filtroCalendarioEscolar,
    },
  };
}
