export function notificacoesLista(lista) {
  return {
    type: '@notificacoes/notificacoesLista',
    payload: lista,
  };
}

export function naoLidas(quantidade) {
  return {
    type: '@notificacoes/naoLidas',
    payload: quantidade,
  };
}
