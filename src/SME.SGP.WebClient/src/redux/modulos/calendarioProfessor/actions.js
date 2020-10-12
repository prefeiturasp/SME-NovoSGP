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

export function salvarEventoAulaCalendarioEdicao(
  tipoCalendario,
  eventoSme,
  dre,
  unidadeEscolar,
  turma,
  mes,
  dia
) {
  return {
    type: '@calendarioProfessor/salvarEventoAulaCalendarioEdicao',
    payload: {
      tipoCalendario,
      eventoSme,
      dre,
      unidadeEscolar,
      turma,
      mes,
      dia,
    },
  };
}

export function salvarDadosAulaFrequencia(disciplinaId, dia, aulaId) {
  return {
    type: '@calendarioProfessor/salvarDadosAulaFrequencia',
    payload: {
      disciplinaId,
      dia,
      aulaId,
    },
  };
}
