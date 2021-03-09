import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';

const ObjectCardAcompanhamentoAprendizagem = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  return (
    <DetalhesAluno dados={dadosAlunoObjectCard} exibirBotaoImprimir={false} />
  );
};

export default ObjectCardAcompanhamentoAprendizagem;
