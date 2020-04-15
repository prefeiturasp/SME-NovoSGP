import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';

const ObjectCardConselhoClasse = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.conselhoClasse.dadosAlunoObjectCard
  );

  return <DetalhesAluno dados={dadosAlunoObjectCard} />;
};

export default ObjectCardConselhoClasse;
