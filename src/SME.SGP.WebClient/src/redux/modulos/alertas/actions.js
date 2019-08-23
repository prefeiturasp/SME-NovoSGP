export function exibir(alerta) {
  return {
    type: '@alertas/exibir',
    payload: alerta,
  };
}

export function removerAlerta(id) {
  return {
    type: '@alertas/remover',
    payload: { id },
  };
}
