export function perfilSelecionado(perfil) {
  return {
    type: '@perfil/perfilSelecionado',
    payload: perfil,
  };
}

export function setarPerfis(perfis) {
  return {
    type: '@perfil/perfis',
    payload: perfis,
  };
}
