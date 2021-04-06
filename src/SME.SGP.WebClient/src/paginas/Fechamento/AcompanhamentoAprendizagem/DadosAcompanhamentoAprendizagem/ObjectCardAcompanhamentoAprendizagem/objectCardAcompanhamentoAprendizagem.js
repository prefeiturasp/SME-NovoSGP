import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';

const ObjectCardAcompanhamentoAprendizagem = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const desabilitarCamposAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .desabilitarCamposAcompanhamentoAprendizagem
  );

  return (
    <DetalhesAluno
      dados={dadosAlunoObjectCard}
      exibirBotaoImprimir={false}
      permiteAlterarImagem={!desabilitarCamposAcompanhamentoAprendizagem}
    />
  );
};

export default ObjectCardAcompanhamentoAprendizagem;
