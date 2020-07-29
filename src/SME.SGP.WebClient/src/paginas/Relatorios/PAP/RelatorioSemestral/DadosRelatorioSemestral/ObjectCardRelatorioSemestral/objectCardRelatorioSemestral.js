import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';

const ObjectCardRelatorioSemestral = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestralPAP.dadosAlunoObjectCard
  );

  const relatorioSemestralAlunoId = useSelector(
    store =>
      store.relatorioSemestralPAP.dadosRelatorioSemestral.relatorioSemestralAlunoId
  );
  
  return <DetalhesAluno dados={dadosAlunoObjectCard} desabilitarImprimir={!relatorioSemestralAlunoId} />;
};

export default ObjectCardRelatorioSemestral;
