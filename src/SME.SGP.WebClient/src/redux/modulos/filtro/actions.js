export function salvarAnosLetivos(anosLetivos) {
  return {
    type: '@filtro/salvarAnosLetivos',
    payload: anosLetivos,
  };
}

export function salvarModalidades(modalidades) {
  return {
    type: '@filtro/salvarModalidades',
    payload: modalidades,
  };
}

export function salvarPeriodos(periodos) {
  return {
    type: '@filtro/salvarPeriodos',
    payload: periodos,
  };
}

export function salvarDres(dres) {
  return {
    type: '@filtro/salvarDres',
    payload: dres,
  };
}

export function salvarUnidadesEscolares(unidadesEscolares) {
  return {
    type: '@filtro/salvarUnidadesEscolares',
    payload: unidadesEscolares,
  };
}

export function salvarTurmas(turmas) {
  return {
    type: '@filtro/salvarTurmas',
    payload: turmas,
  };
}

export function limparDadosFiltro() {
  return {
    type: '@filtro/limparDadosFiltro',
  };
}
