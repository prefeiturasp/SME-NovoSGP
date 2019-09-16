export function menuRetraido(retraido) {
  return {
    type: '@navegacao/retraido',
    payload: retraido,
  };
}

export function rotaAtiva(route) {
  return {
    type: '@navegacao/rotaAtiva',
    payload: route,
  };
}

export function setRotas(routas) {
  return {
    type: '@navegacao/rotas',
    payload: routas,
  };
}
