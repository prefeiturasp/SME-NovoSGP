import React from 'react';
import { useSelector } from 'react-redux';

const InfoQuantidadeImagens = () => {
  const dadosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAcompanhamentoAprendizagem
  );

  return (
    <span className="font-weight-bold">
      Carregue até {dadosAcompanhamentoAprendizagem?.quantidadeFotos} fotos da
      criança
    </span>
  );
};

export default InfoQuantidadeImagens;
