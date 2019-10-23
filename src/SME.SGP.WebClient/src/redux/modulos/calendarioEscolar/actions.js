export function alternaMes(mes) {
  return {
    type: '@calendarioEscolar/alternaMes',
    payload: mes,
  };
}

export function selecionaDia(dia) {
  return {
    type: '@calendarioEscolar/selecionaDia',
    payload: dia,
  };
}
