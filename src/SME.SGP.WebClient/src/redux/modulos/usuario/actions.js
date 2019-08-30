export function turmasUusario(turmas) {
  return {
    type: '@usuario/turmasUusario',
    payload: turmas,
  };
}

export function selecionarTurma(turma) {
  return {
    type: '@usuario/selecionarTurma',
    payload: turma,
  };
}
