export function salvarRf(rf) {
  return {
    type: '@usuario/salvarRf',
    payload: rf,
  };
}

export function SalvarDadosLogin(Login) {
  return {
    type: '@usuario/salvarLogin',
    payload: Login,
  };
}

export function Deslogar() {
  return {
    type: '@usuario/deslogar',
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

export function filtroAtual(filtro) {
  return {
    type: '@usuario/filtroAtual',
    payload: filtro,
  };
}

export function salvarDadosUsuario(filtro) {
  return {
    type: '@usuario/salvarDadosUsuario',
    payload: filtro,
  };
}
