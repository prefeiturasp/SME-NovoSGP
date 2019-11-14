export function selecionaMes(mes) {
  return {
    type: '@calendarioProfessor/selecionaMes',
    payload: mes,
  };
}

export function selecionaDia(dia) {
  return {
    type: '@calendarioProfessor/selecionaDia',
    payload: dia,
  };
}

export function atribuiEventosMes(mes, eventos) {
  return {
    type: '@calendarioProfessor/atribuiEventosMes',
    payload: {
      mes,
      eventos,
    },
  };
}

export function zeraCalendario() {
  return {
    type: '@calendarioProfessor/zeraCalendario',
  };
}
