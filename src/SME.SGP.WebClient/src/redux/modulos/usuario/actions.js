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
