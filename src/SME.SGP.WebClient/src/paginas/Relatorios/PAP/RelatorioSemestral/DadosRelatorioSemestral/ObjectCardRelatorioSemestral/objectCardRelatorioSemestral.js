import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';

const ObjectCardRelatorioSemestral = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestral.dadosAlunoObjectCard
  );

  return <DetalhesAluno dados={dadosAlunoObjectCard} />;
};

export default ObjectCardRelatorioSemestral;
