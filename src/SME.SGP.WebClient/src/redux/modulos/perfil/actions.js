export function perfilSelecionado(perfil) {
  return {
    type: '@perfil/perfilSelecionado',
    payload: perfil,
  };
}