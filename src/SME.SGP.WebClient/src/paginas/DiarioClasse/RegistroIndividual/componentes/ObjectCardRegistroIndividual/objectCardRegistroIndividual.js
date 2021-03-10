import React from 'react';
import { useSelector } from 'react-redux';

import { DetalhesAluno } from '~/componentes';

const ObjectCardRegistroIndividual = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );

  return (
    <DetalhesAluno
      exibirBotaoImprimir={false}
      exibirFrequencia={false}
      dados={dadosAlunoObjectCard}
    />
  );
};

export default ObjectCardRegistroIndividual;
