export function setExibirMensagemSessaoExpirou(sessaoExpirou) {
  return {
    type: '@usuario/setExibirMensagemSessaoExpirou',
    payload: sessaoExpirou,
  };
}
