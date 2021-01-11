import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';

const ObjectCardEncaminhamento = () => {
  // TODO - falta endpoint
  const dadosEstudanteObjectCardEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEstudanteObjectCardEncaminhamento
  );

  return <DetalhesAluno dados={dadosEstudanteObjectCardEncaminhamento} />;
};

export default ObjectCardEncaminhamento;
