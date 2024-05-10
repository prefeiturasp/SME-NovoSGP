export function selecionarDre(dreId) {
  return {
    type: '@atribuicaoEsporadica/selecionarDre',
    payload: dreId,
  };
}

export function selecionarUe(ueId) {
  return {
    type: '@atribuicaoEsporadica/selecionarUe',
    payload: ueId,
  };
}

export function selecionarAnoLetivo(anoLetivo) {
  return {
    type: '@atribuicaoEsporadica/selecionarAnoLetivo',
    payload: anoLetivo,
  };
}
