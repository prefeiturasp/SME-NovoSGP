export function salvarRf(rf) {
  return {
    type: '@usuario/salvarRf',
    payload: rf.trim(),
  };
}

export function salvarDadosLogin(Login) {
  return {
    type: '@usuario/salvarLogin',
    payload: Login,
  };
}

export function salvarLoginRevalidado(Login) {
  return {
    type: '@usuario/salvarLoginRevalidado',
    payload: Login,
  };
}

export function Deslogar() {
  return {
    type: '@usuario/deslogar',
  };
}

export function DeslogarSessaoExpirou() {
  return {
    type: '@usuario/deslogarSessaoExpirou',
  };
}

export function turmasUsuario(turmas) {
  return {
    type: '@usuario/turmasUsuario',
    payload: turmas,
  };
}

export function selecionarTurma(turma) {
  return {
    type: '@usuario/selecionarTurma',
    payload: turma,
  };
}

export function removerTurma() {
  return {
    type: '@usuario/removerTurma',
  };
}

export function setarConsideraHistorico(payload) {
  return {
    type: '@usuario/setarConsideraHistorico',
    payload,
  };
}

export function meusDados(dados) {
  return {
    type: '@usuario/meusDados',
    payload: dados,
  };
}

export function filtroAtual(filtro) {
  return {
    type: '@usuario/filtroAtual',
    payload: filtro,
  };
}

export function meusDadosSalvarEmail(email) {
  return {
    type: '@usuario/meusDadosSalvarEmail',
    payload: email,
  };
}

export function salvarDadosUsuario(filtro) {
  return {
    type: '@usuario/salvarDadosUsuario',
    payload: filtro,
  };
}

export function setMenu(menu) {
  return {
    type: '@usuario/setMenu',
    payload: menu,
  };
}

export function setPermissoes(permissoes) {
  return {
    type: '@usuario/setPermissoes',
    payload: permissoes,
  };
}

export function setModificarSenha(modificarSenha) {
  return {
    type: '@usuario/setModificarSenha',
    payload: modificarSenha,
  };
}

export function setLogado(logado) {
  return {
    type: '@usuario/setLogado',
    payload: logado,
  };
}
