import modalidade from '~/dtos/modalidade';

const ehTurmaInfantil = (modalidades, turmaSelecionada) => {
  const temSomenteUmaModalidade = modalidades && modalidades.length === 1;

  let ehModalidadeInfantil = false;

  if (temSomenteUmaModalidade) {
    ehModalidadeInfantil =
      String(modalidades[0].valor) === String(modalidade.INFANTIL);
  } else {
    ehModalidadeInfantil =
      String(turmaSelecionada.modalidade) === String(modalidade.INFANTIL);
  }
  return ehModalidadeInfantil;
};

export { ehTurmaInfantil };
