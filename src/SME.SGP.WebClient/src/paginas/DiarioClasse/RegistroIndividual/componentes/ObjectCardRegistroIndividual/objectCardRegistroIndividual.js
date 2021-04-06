import React from 'react';
import { useSelector } from 'react-redux';

import { DetalhesAluno } from '~/componentes';

const ObjectCardRegistroIndividual = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );

  const desabilitarCampos = useSelector(
    state => state.registroIndividual.desabilitarCampos
  );

  return (
    <DetalhesAluno
      exibirBotaoImprimir={false}
      exibirFrequencia={false}
      dados={dadosAlunoObjectCard}
      permiteAlterarImagem={!desabilitarCampos}
    />
  );
};

export default ObjectCardRegistroIndividual;
