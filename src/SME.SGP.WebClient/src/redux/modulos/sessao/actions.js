export function LimparSessao(disciplina) {
  return {
    type: '@sessao/limpar',
    payload: disciplina,
  };
}
