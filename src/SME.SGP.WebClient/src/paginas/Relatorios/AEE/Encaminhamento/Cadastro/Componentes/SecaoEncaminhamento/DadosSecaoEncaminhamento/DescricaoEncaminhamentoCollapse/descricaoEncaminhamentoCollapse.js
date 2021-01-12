import React from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const DescricaoEncaminhamentoCollapse = () => {
  const configCabecalho = {
    altura: '50px',
    corBorda: Base.AzulBordaCard,
  };

  const onClickCardCollapse = () => {};

  return (
    <div className="mt-2 mb-2">
      <CardCollapse
        key="descricao-encaminhamento-collapse-key"
        titulo="Descrição do encaminhamento"
        indice="descricao-encaminhamento-collapse-indice"
        alt="descricao-encaminhamento-collapse-alt"
        onClick={onClickCardCollapse}
        configCabecalho={configCabecalho}
      >
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento Descrição do encaminhamento Descrição do encaminhamento
        Descrição do encaminhamento Descrição do encaminhamento Descrição do
        encaminhamento
      </CardCollapse>
    </div>
  );
};

export default DescricaoEncaminhamentoCollapse;
