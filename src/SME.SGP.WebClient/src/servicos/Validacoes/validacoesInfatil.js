import modalidade from '~/dtos/modalidade';

const obterModalidadeFiltroPrincipal = (modalidades, turmaSelecionada) => {
  const temSomenteUmaModalidade = modalidades && modalidades.length === 1;

  let modalidadeAtual = 0;

  if (temSomenteUmaModalidade) {
    modalidadeAtual = String(modalidades[0].valor);
  } else {
    modalidadeAtual =
      turmaSelecionada && turmaSelecionada.modalidade
        ? String(turmaSelecionada.modalidade)
        : modalidade.FUNDAMENTAL;
  }
  return modalidadeAtual;
};

const ehTurmaInfantil = (modalidades, turmaSelecionada) => {
  return (
    obterModalidadeFiltroPrincipal(modalidades, turmaSelecionada) ===
    String(modalidade.INFANTIL)
  );
};

export { ehTurmaInfantil, obterModalidadeFiltroPrincipal };
