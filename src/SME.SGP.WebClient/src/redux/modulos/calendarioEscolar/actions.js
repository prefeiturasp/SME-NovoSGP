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
